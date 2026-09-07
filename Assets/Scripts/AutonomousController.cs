using System.Collections.Generic;
using UnityEngine;

public class AutonomousController : MonoBehaviour
{
    // =====================================================
    // REFERENCES
    // =====================================================

    [Header("References")]

    public PerceptionProcessor perceptionProcessor;

    public CarsController carsController;


    // =====================================================
    // WAYPOINT NAVIGATION
    // =====================================================

    [Header("Waypoint Navigation")]

    [Tooltip("Parent object containing ordered road waypoints.")]
    public Transform waypointContainer;

    [Tooltip("Ordered road waypoints used for autonomous navigation.")]
    public Transform[] waypoints;

    [Tooltip("How far ahead along the waypoint path to aim.")]
    public float lookAheadDistance = 12f;

    [Tooltip("Maximum waypoint steering contribution.")]
    [Range(0f, 1f)]
    public float waypointSteeringStrength = 0.8f;

    [Tooltip("If true, automatically loads all child transforms of waypointContainer.")]
    public bool autoLoadWaypoints = true;


    // =====================================================
    // SPEED CONTROL
    // =====================================================

    [Header("Speed Control")]

    [Tooltip("Maximum autonomous driving speed.")]
    public float targetSpeedKmh = 45f;

    [Tooltip("Distance below which speed is reduced.")]
    public float slowDownDistance = 20f;

    [Tooltip("Distance below which strong braking begins.")]
    public float brakingDistance = 10f;

    [Tooltip("Emergency braking distance.")]
    public float emergencyBrakingDistance = 4f;


    // =====================================================
    // OBSTACLE AVOIDANCE
    // =====================================================

    [Header("Obstacle Avoidance")]

    [Tooltip("Maximum horizontal position for an object to be considered ahead.")]
    [Range(0.1f, 1f)]
    public float obstacleDetectionWidth = 0.45f;

    [Tooltip("How strongly the vehicle steers away from obstacles.")]
    [Range(0f, 1f)]
    public float obstacleSteeringStrength = 0.5f;

    [Tooltip("Minimum distance at which obstacle avoidance steering is applied.")]
    public float avoidanceDistance = 15f;


    // =====================================================
    // THROTTLE SETTINGS
    // =====================================================

    [Header("Throttle")]

    [Range(0f, 1f)]
    public float maximumThrottle = 0.35f;

    [Tooltip("How aggressively throttle is reduced near target speed.")]
    public float speedControlStrength = 0.04f;


    // =====================================================
    // DEBUG
    // =====================================================

    [Header("Debug")]

    public bool enableDebugLogs = true;

    public float logInterval = 1f;

    private float logTimer = 0f;


    // =====================================================
    // WAYPOINT STATE
    // =====================================================

    private int currentWaypointIndex = 0;


    // =====================================================
    // UNITY
    // =====================================================

    private void Awake()
    {
        // -----------------------------------------------
        // FIND PERCEPTION PROCESSOR
        // -----------------------------------------------

        if (perceptionProcessor == null)
        {
            perceptionProcessor =
                FindFirstObjectByType<PerceptionProcessor>();
        }


        // -----------------------------------------------
        // FIND CAR CONTROLLER
        // -----------------------------------------------

        if (carsController == null)
        {
            carsController =
                GetComponent<CarsController>();
        }


        // -----------------------------------------------
        // VALIDATION
        // -----------------------------------------------

        if (perceptionProcessor == null)
        {
            Debug.LogError(
                "[AutonomousController] " +
                "PerceptionProcessor reference missing!"
            );
        }


        if (carsController == null)
        {
            Debug.LogError(
                "[AutonomousController] " +
                "CarsController reference missing!"
            );
        }


        // -----------------------------------------------
        // LOAD WAYPOINTS
        // -----------------------------------------------

        LoadWaypoints();
    }


    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        FindNearestWaypoint();
    }


    // =====================================================
    // UPDATE
    // =====================================================

    private void Update()
    {
        if (
            perceptionProcessor == null ||
            carsController == null
        )
        {
            return;
        }


        List<PerceivedObject> objects =
            perceptionProcessor.CurrentPerceptions;


        // -----------------------------------------------
        // CALCULATE STEERING
        // -----------------------------------------------

        float waypointSteering =
            CalculateWaypointSteering();


        float obstacleSteering =
            CalculateObstacleSteering(objects);


        float steering =
            waypointSteering +
            obstacleSteering;


        steering =
            Mathf.Clamp(
                steering,
                -1f,
                1f
            );


        // -----------------------------------------------
        // CALCULATE THROTTLE
        // -----------------------------------------------

        float throttle =
            CalculateThrottle(objects);


        // -----------------------------------------------
        // CALCULATE BRAKE
        // -----------------------------------------------

        float brake =
            CalculateBrake(objects);


        // -----------------------------------------------
        // SEND CONTROL
        // -----------------------------------------------

        carsController.SetAutonomousControl(
            steering,
            throttle,
            brake
        );


        // -----------------------------------------------
        // DEBUG
        // -----------------------------------------------

        HandleDebugLogging(
            objects,
            waypointSteering,
            obstacleSteering,
            steering,
            throttle,
            brake
        );
    }


    // =====================================================
    // LOAD WAYPOINTS
    // =====================================================

    private void LoadWaypoints()
    {
        if (
            !autoLoadWaypoints ||
            waypointContainer == null
        )
        {
            return;
        }


        Transform[] children =
            waypointContainer
                .GetComponentsInChildren<Transform>();


        List<Transform> loadedWaypoints =
            new List<Transform>();


        foreach (
            Transform child
            in children
        )
        {
            // Do not include the parent itself
            if (child == waypointContainer)
            {
                continue;
            }

            loadedWaypoints.Add(child);
        }


        waypoints =
            loadedWaypoints.ToArray();


        Debug.Log(
            "[AutonomousController] Loaded " +
            waypoints.Length +
            " waypoints."
        );
    }


    // =====================================================
    // FIND NEAREST WAYPOINT
    // =====================================================

    private void FindNearestWaypoint()
    {
        if (
            waypoints == null ||
            waypoints.Length == 0
        )
        {
            Debug.LogWarning(
                "[AutonomousController] " +
                "No waypoints available!"
            );

            return;
        }


        float closestDistance =
            float.MaxValue;

        int closestIndex = 0;


        for (
            int i = 0;
            i < waypoints.Length;
            i++
        )
        {
            if (waypoints[i] == null)
            {
                continue;
            }


            float distance =
                Vector3.Distance(
                    transform.position,
                    waypoints[i].position
                );


            if (
                distance <
                closestDistance
            )
            {
                closestDistance =
                    distance;

                closestIndex =
                    i;
            }
        }


        currentWaypointIndex =
            closestIndex;
    }


    // =====================================================
    // WAYPOINT STEERING
    // =====================================================

    private float CalculateWaypointSteering()
    {
        if (
            waypoints == null ||
            waypoints.Length == 0
        )
        {
            return 0f;
        }


        Transform targetWaypoint =
            FindLookAheadWaypoint();


        if (targetWaypoint == null)
        {
            return 0f;
        }


        Vector3 direction =
            targetWaypoint.position -
            transform.position;


        direction.y = 0f;


        if (direction.sqrMagnitude < 0.01f)
        {
            return 0f;
        }


        /*
         * Convert target direction into
         * the vehicle's local coordinate system.
         *
         * local X:
         * negative = target left
         * positive = target right
         */

        Vector3 localDirection =
            transform.InverseTransformDirection(
                direction.normalized
            );


        float angle =
            Mathf.Atan2(
                localDirection.x,
                localDirection.z
            ) *
            Mathf.Rad2Deg;


        /*
         * Convert angle into normalized steering.
         */

        float steering =
            angle / 45f;


        steering =
            Mathf.Clamp(
                steering,
                -1f,
                1f
            );


        return steering *
               waypointSteeringStrength;
    }


    // =====================================================
    // FIND LOOK-AHEAD WAYPOINT
    // =====================================================

    private Transform FindLookAheadWaypoint()
    {
        if (
            waypoints == null ||
            waypoints.Length == 0
        )
        {
            return null;
        }


        Transform bestWaypoint =
            null;


        float accumulatedDistance =
            0f;


        int index =
            currentWaypointIndex;


        for (
            int i = 0;
            i < waypoints.Length;
            i++
        )
        {
            Transform waypoint =
                waypoints[index];


            if (waypoint == null)
            {
                index =
                    GetNextWaypointIndex(index);

                continue;
            }


            float distance =
                Vector3.Distance(
                    transform.position,
                    waypoint.position
                );


            accumulatedDistance +=
                distance;


            if (
                accumulatedDistance >=
                lookAheadDistance
            )
            {
                bestWaypoint =
                    waypoint;

                currentWaypointIndex =
                    index;

                break;
            }


            index =
                GetNextWaypointIndex(index);
        }


        // Fallback
        if (bestWaypoint == null)
        {
            bestWaypoint =
                waypoints[
                    currentWaypointIndex
                ];
        }


        return bestWaypoint;
    }


    // =====================================================
    // NEXT WAYPOINT
    // =====================================================

    private int GetNextWaypointIndex(
        int index
    )
    {
        index++;


        if (
            index >=
            waypoints.Length
        )
        {
            index = 0;
        }


        return index;
    }


    // =====================================================
    // OBSTACLE STEERING
    // =====================================================

    private float CalculateObstacleSteering(
        List<PerceivedObject> objects
    )
    {
        PerceivedObject closest =
            FindClosestObjectAhead(objects);


        if (closest == null)
        {
            return 0f;
        }


        if (
            closest.distanceMeters >
            avoidanceDistance
        )
        {
            return 0f;
        }


        /*
         * Steer AWAY from the object.
         *
         * Object left  -> steer right
         * Object right -> steer left
         */

        float avoidance =
            -closest.relativeX;


        /*
         * Scale avoidance based on distance.
         *
         * Closer object = stronger response.
         */

        float distanceFactor =
            1f -
            Mathf.Clamp01(
                closest.distanceMeters /
                avoidanceDistance
            );


        avoidance *=
            distanceFactor;


        avoidance *=
            obstacleSteeringStrength;


        return Mathf.Clamp(
            avoidance,
            -1f,
            1f
        );
    }


    // =====================================================
    // THROTTLE
    // =====================================================

    private float CalculateThrottle(
        List<PerceivedObject> objects
    )
    {
        float speed =
            carsController.CurrentSpeed;


        // -----------------------------------------------
        // EMERGENCY OBJECT
        // -----------------------------------------------

        PerceivedObject closest =
            FindClosestObjectAhead(objects);


        if (closest != null)
        {
            if (
                closest.distanceMeters <=
                emergencyBrakingDistance
            )
            {
                return 0f;
            }


            if (
                closest.distanceMeters <=
                brakingDistance
            )
            {
                return 0.05f;
            }


            if (
                closest.distanceMeters <=
                slowDownDistance
            )
            {
                return 0.15f;
            }
        }


        // -----------------------------------------------
        // SPEED CONTROL
        // -----------------------------------------------

        float speedError =
            targetSpeedKmh -
            speed;


        if (speedError <= 0f)
        {
            return 0f;
        }


        float throttle =
            speedError *
            speedControlStrength;


        return Mathf.Clamp(
            throttle,
            0f,
            maximumThrottle
        );
    }


    // =====================================================
    // BRAKING
    // =====================================================

    private float CalculateBrake(
        List<PerceivedObject> objects
    )
    {
        PerceivedObject closest =
            FindClosestObjectAhead(objects);


        if (closest == null)
        {
            // Brake if we're significantly
            // above the target speed.
            float speed =
                carsController.CurrentSpeed;


            if (
                speed >
                targetSpeedKmh + 5f
            )
            {
                return Mathf.Clamp01(
                    (
                        speed -
                        targetSpeedKmh
                    ) / 20f
                );
            }


            return 0f;
        }


        // -----------------------------------------------
        // EMERGENCY
        // -----------------------------------------------

        if (
            closest.distanceMeters <=
            emergencyBrakingDistance
        )
        {
            return 1f;
        }


        // -----------------------------------------------
        // NORMAL BRAKING
        // -----------------------------------------------

        if (
            closest.distanceMeters <=
            brakingDistance
        )
        {
            return 0.6f;
        }


        // -----------------------------------------------
        // SPEED-BASED BRAKING
        // -----------------------------------------------

        float currentSpeed =
            carsController.CurrentSpeed;


        if (
            currentSpeed >
            targetSpeedKmh + 5f
        )
        {
            return Mathf.Clamp01(
                (
                    currentSpeed -
                    targetSpeedKmh
                ) / 20f
            );
        }


        return 0f;
    }


    // =====================================================
    // FIND CLOSEST OBJECT
    // =====================================================

    private PerceivedObject FindClosestObjectAhead(
        List<PerceivedObject> objects
    )
    {
        if (
            objects == null ||
            objects.Count == 0
        )
        {
            return null;
        }


        PerceivedObject closest =
            null;


        float closestDistance =
            float.MaxValue;


        foreach (
            PerceivedObject obj
            in objects
        )
        {
            if (obj == null)
            {
                continue;
            }


            if (!obj.valid)
            {
                continue;
            }


            if (
                obj.distanceMeters <= 0f
            )
            {
                continue;
            }


            /*
             * Ignore objects that are far outside
             * the vehicle's forward region.
             */

            if (
                Mathf.Abs(
                    obj.relativeX
                ) >
                obstacleDetectionWidth
            )
            {
                continue;
            }


            if (
                obj.distanceMeters <
                closestDistance
            )
            {
                closest =
                    obj;

                closestDistance =
                    obj.distanceMeters;
            }
        }


        return closest;
    }


    // =====================================================
    // DEBUG LOGGING
    // =====================================================

    private void HandleDebugLogging(
        List<PerceivedObject> objects,
        float waypointSteering,
        float obstacleSteering,
        float steering,
        float throttle,
        float brake
    )
    {
        if (!enableDebugLogs)
        {
            return;
        }


        logTimer +=
            Time.deltaTime;


        if (
            logTimer <
            logInterval
        )
        {
            return;
        }


        logTimer = 0f;


        PerceivedObject closest =
            FindClosestObjectAhead(objects);


        if (closest == null)
        {
            Debug.Log(
                "[AUTONOMOUS] " +
                "No obstacle | " +
                "WaypointSteer: " +
                waypointSteering.ToString("F2") +
                " | Steering: " +
                steering.ToString("F2") +
                " | Throttle: " +
                throttle.ToString("F2") +
                " | Brake: " +
                brake.ToString("F2") +
                " | Speed: " +
                carsController.CurrentSpeed
                    .ToString("F1") +
                " km/h"
            );

            return;
        }


        Debug.Log(
            "[AUTONOMOUS] " +
            closest.className.ToUpper() +
            " | Distance: " +
            closest.distanceMeters
                .ToString("F2") +
            " m" +
            " | Relative X: " +
            closest.relativeX
                .ToString("F2") +
            " | WaypointSteer: " +
            waypointSteering
                .ToString("F2") +
            " | ObstacleSteer: " +
            obstacleSteering
                .ToString("F2") +
            " | Steering: " +
            steering
                .ToString("F2") +
            " | Throttle: " +
            throttle
                .ToString("F2") +
            " | Brake: " +
            brake
                .ToString("F2") +
            " | Speed: " +
            carsController.CurrentSpeed
                .ToString("F1") +
            " km/h"
        );
    }
}
using System.Collections.Generic;
using UnityEngine;

public class PerceptionProcessor : MonoBehaviour
{
    // =====================================================
    // CAMERA CONFIGURATION
    // =====================================================

    [Header("Camera Calibration")]

    [Tooltip("Width of the YOLO input image in pixels.")]
    public float imageWidth = 640f;

    [Tooltip("Height of the YOLO input image in pixels.")]
    public float imageHeight = 360f;

    [Tooltip("Vertical field of view of the Unity camera.")]
    public float verticalFOV = 60f;

    // Derived focal length in pixels
    private float focalLengthPixels;


    // =====================================================
    // DETECTION SETTINGS
    // =====================================================

    [Header("Detection Settings")]

    [Range(0f, 1f)]
    public float confidenceThreshold = 0.5f;


    // =====================================================
    // OBJECT REFERENCE HEIGHTS
    // =====================================================

    [Header("Reference Object Heights (metres)")]

    public float personHeight = 1.70f;
    public float carHeight = 1.50f;
    public float truckHeight = 3.00f;
    public float busHeight = 3.20f;
    public float motorcycleHeight = 1.40f;
    public float autorickshawHeight = 1.80f;


    // =====================================================
    // DEBUGGING
    // =====================================================

    [Header("Debug")]

    public bool enableDebugLogs = true;


    // =====================================================
    // INITIALIZATION
    // =====================================================

    private void Awake()
    {
        CalculateFocalLength();

        Debug.Log(
            "[PerceptionProcessor] Initialized | " +
            "Image: " +
            imageWidth +
            "x" +
            imageHeight +
            " | Vertical FOV: " +
            verticalFOV +
            "° | Focal Length: " +
            focalLengthPixels.ToString("F2") +
            " px"
        );
    }


    // =====================================================
    // CAMERA CALIBRATION
    // =====================================================

    private void CalculateFocalLength()
    {
        /*
         * Unity provides the vertical field of view.
         *
         * For a 640x360 image:
         *
         * focal length =
         * imageHeight /
         * (2 * tan(verticalFOV / 2))
         */

        float verticalFOVRadians =
            verticalFOV * Mathf.Deg2Rad;

        focalLengthPixels =
            imageHeight /
            (
                2f *
                Mathf.Tan(
                    verticalFOVRadians / 2f
                )
            );
    }


    // =====================================================
    // MAIN PROCESSING FUNCTION
    // =====================================================

    public List<PerceivedObject> ProcessDetections(
        DetectionResponse response
    )
    {
        List<PerceivedObject> perceivedObjects =
            new List<PerceivedObject>();

        if (
            response == null ||
            response.detections == null
        )
        {
            return perceivedObjects;
        }

        foreach (
            Detection detection
            in response.detections
        )
        {
            // Ignore low-confidence detections
            if (
                detection.confidence <
                confidenceThreshold
            )
            {
                continue;
            }

            float distance =
                EstimateDistance(detection);

            float relativeX =
                CalculateRelativeX(detection);

            PerceivedObject perceived =
                new PerceivedObject();

            // ==========================================
            // RAW YOLO DATA
            // ==========================================

            perceived.className =
                detection.@class;

            perceived.confidence =
                detection.confidence;

            perceived.x1 =
                detection.x1;

            perceived.y1 =
                detection.y1;

            perceived.x2 =
                detection.x2;

            perceived.y2 =
                detection.y2;


            // ==========================================
            // DERIVED PERCEPTION
            // ==========================================

            perceived.distanceMeters =
                distance;

            perceived.relativeX =
                relativeX;

            perceived.valid =
                distance > 0f;


            // ==========================================
            // ADD TO PERCEPTION LIST
            // ==========================================

            perceivedObjects.Add(
                perceived
            );


            // ==========================================
            // DEBUG LOGGING
            // ==========================================

            if (enableDebugLogs)
            {
                Debug.Log(
                    "[PERCEPTION] " +
                    perceived.className.ToUpper() +
                    " | Confidence: " +
                    (
                        perceived.confidence * 100f
                    ).ToString("F1") +
                    "%" +
                    " | Distance: " +
                    perceived.distanceMeters
                        .ToString("F2") +
                    " m" +
                    " | Relative X: " +
                    perceived.relativeX
                        .ToString("F2")
                );
            }
        }

        return perceivedObjects;
    }


    // =====================================================
    // MONOCULAR DISTANCE ESTIMATION
    // =====================================================

    private float EstimateDistance(
        Detection detection
    )
    {
        /*
         * Monocular distance estimation:
         *
         *              f × H
         *       D =  ---------
         *                h
         *
         * D = estimated distance
         * f = focal length in pixels
         * H = assumed real-world object height
         * h = bounding-box height in pixels
         */

        float boundingBoxHeight =
            Mathf.Abs(
                detection.y2 -
                detection.y1
            );

        // Prevent division by zero
        if (
            boundingBoxHeight <= 1f
        )
        {
            return -1f;
        }

        float referenceHeight =
            GetReferenceHeight(
                detection.@class
            );

        // Unsupported class
        if (
            referenceHeight <= 0f
        )
        {
            return -1f;
        }

        float distance =
            (
                focalLengthPixels *
                referenceHeight
            ) /
            boundingBoxHeight;

        return distance;
    }


    // =====================================================
    // REFERENCE OBJECT HEIGHT
    // =====================================================

    private float GetReferenceHeight(
        string className
    )
    {
        if (
            string.IsNullOrEmpty(className)
        )
        {
            return -1f;
        }

        switch (
            className.ToLower()
        )
        {
            case "person":
            case "pedestrian":
                return personHeight;

            case "car":
                return carHeight;

            case "truck":
                return truckHeight;

            case "bus":
                return busHeight;

            case "motorcycle":
            case "motorbike":
                return motorcycleHeight;

            case "autorickshaw":
            case "auto-rickshaw":
            case "auto":
                return autorickshawHeight;

            default:
                return -1f;
        }
    }


    // =====================================================
    // RELATIVE X POSITION
    // =====================================================

    private float CalculateRelativeX(
        Detection detection
    )
    {
        /*
         * Find the horizontal centre of
         * the YOLO bounding box.
         */

        float boundingBoxCenterX =
            (
                detection.x1 +
                detection.x2
            ) / 2f;


        /*
         * Normalize:
         *
         * 0   = left
         * 0.5 = centre
         * 1   = right
         */

        float normalizedX =
            boundingBoxCenterX /
            imageWidth;


        /*
         * Convert to:
         *
         * -1 = far left
         *  0 = centre
         * +1 = far right
         */

        float relativeX =
            (
                normalizedX -
                0.5f
            ) * 2f;


        return Mathf.Clamp(
            relativeX,
            -1f,
            1f
        );
    }
}
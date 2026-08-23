using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    [Header("Vehicle Prefabs")]
    public GameObject[] vehiclePrefabs;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Number of Vehicles")]
    public int vehicleCount = 5;

    private List<GameObject> activeVehicles = new List<GameObject>();

    public void Initialize()
    {
        SpawnTraffic();
    }

    void SpawnTraffic()
    {
        if (vehiclePrefabs.Length == 0)
        {
            Debug.LogWarning("No vehicle prefabs assigned.");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        for (int i = 0; i < vehicleCount; i++)
        {
            GameObject prefab =
                vehiclePrefabs[
                    Random.Range(0, vehiclePrefabs.Length)
                ];

            Transform spawn =
                spawnPoints[
                    Random.Range(0, spawnPoints.Length)
                ];

            GameObject vehicle =
                Instantiate(
                    prefab,
                    spawn.position,
                    spawn.rotation
                );

            activeVehicles.Add(vehicle);

            NPCVehicle npc = vehicle.GetComponent<NPCVehicle>();

            if (npc != null)
            {
                npc.InitializeAgent();
            }
        }

        Debug.Log(
            "Spawned " + activeVehicles.Count + " vehicles."
        );
    }

    public void ResetTraffic()
    {
        foreach (GameObject vehicle in activeVehicles)
        {
            if (vehicle != null)
                Destroy(vehicle);
        }

        activeVehicles.Clear();

        SpawnTraffic();
    }

    public List<GameObject> GetActiveVehicles()
    {
        return activeVehicles;
    }

    public NPCVehicle GetNearestVehicle(Vector3 position)
    {
        NPCVehicle nearest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in activeVehicles)
        {
            if (obj == null)
                continue;

            NPCVehicle vehicle =
                obj.GetComponent<NPCVehicle>();

            if (vehicle == null)
                continue;

            float distance =
                Vector3.Distance(
                    position,
                    vehicle.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = vehicle;
            }
        }

        return nearest;
    }
}
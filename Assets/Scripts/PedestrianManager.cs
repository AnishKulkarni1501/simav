using System.Collections.Generic;
using UnityEngine;

public class PedestrianManager : MonoBehaviour
{
    public GameObject pedestrianPrefab;

    public Transform[] spawnPoints;

    public Transform[] destinationPoints;

    public int pedestrianCount = 3;

    private List<GameObject> activePedestrians =
        new List<GameObject>();

    public void Initialize()
    {
        SpawnPedestrians();
    }

    void SpawnPedestrians()
    {
        if (pedestrianPrefab == null)
        {
            Debug.LogWarning(
                "No pedestrian prefab assigned."
            );

            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                "No pedestrian spawn points."
            );

            return;
        }

        for (int i = 0; i < pedestrianCount; i++)
        {
            Transform spawn =
                spawnPoints[
                    Random.Range(
                        0,
                        spawnPoints.Length
                    )
                ];

            GameObject pedestrian =
                Instantiate(
                    pedestrianPrefab,
                    spawn.position,
                    spawn.rotation
                );

            activePedestrians.Add(pedestrian);

            NPCPedestrian npc =
                pedestrian.GetComponent<NPCPedestrian>();

            if (npc != null)
            {
                npc.InitializeAgent();

                if (destinationPoints.Length > 0)
                {
                    Transform destination =
                        destinationPoints[
                            Random.Range(
                                0,
                                destinationPoints.Length
                            )
                        ];

                    npc.SetDestination(
                        destination.position
                    );
                }
            }
        }

        Debug.Log(
            "Spawned " +
            activePedestrians.Count +
            " pedestrians."
        );
    }

    public void ResetPedestrians()
    {
        foreach (GameObject pedestrian
                 in activePedestrians)
        {
            if (pedestrian != null)
                Destroy(pedestrian);
        }

        activePedestrians.Clear();

        SpawnPedestrians();
    }

    public List<GameObject> GetActivePedestrians()
    {
        return activePedestrians;
    }
}
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public TrafficManager trafficManager;
    public PedestrianManager pedestrianManager;

    private ScenarioType currentScenario;

    public void StartScenario(
        ScenarioType scenario)
    {
        currentScenario = scenario;

        Debug.Log(
            "Starting Scenario: " +
            currentScenario
        );

        switch (currentScenario)
        {
            case ScenarioType.NormalTraffic:
                NormalTraffic();
                break;

            case ScenarioType.VehicleCutIn:
                VehicleCutIn();
                break;

            case ScenarioType.PedestrianCrossing:
                PedestrianCrossing();
                break;

            case ScenarioType.RoadConstruction:
                RoadConstruction();
                break;

            case ScenarioType.EmergencyVehicle:
                EmergencyVehicle();
                break;
        }
    }

    void NormalTraffic()
    {
        Debug.Log("Normal traffic started.");
    }

    void VehicleCutIn()
    {
        Debug.Log("Vehicle cut-in scenario started.");

        NPCVehicle vehicle =
            trafficManager.GetNearestVehicle(
                transform.position
            );

        if (vehicle != null)
        {
            vehicle.SetSpeed(12f);

            // Simple cut-in direction.
            vehicle.transform.position +=
                vehicle.transform.right * 3f;
        }
    }

    void PedestrianCrossing()
    {
        Debug.Log(
            "Pedestrian crossing scenario started."
        );

        foreach (
            GameObject pedestrian
            in pedestrianManager.GetActivePedestrians())
        {
            NPCPedestrian npc =
                pedestrian.GetComponent<NPCPedestrian>();

            if (npc != null)
            {
                npc.ResumeAgent();
            }
        }
    }

    void RoadConstruction()
    {
        Debug.Log(
            "Road construction scenario started."
        );

        // Later:
        // Spawn barriers
        // Close lanes
        // Spawn construction vehicles
    }

    void EmergencyVehicle()
    {
        Debug.Log(
            "Emergency vehicle scenario started."
        );

        // Later:
        // Spawn ambulance
        // Give it priority
        // Make surrounding traffic yield
    }

    public void ResetScenario()
    {
        Debug.Log("Resetting scenario.");
    }

    public ScenarioType GetCurrentScenario()
    {
        return currentScenario;
    }
}
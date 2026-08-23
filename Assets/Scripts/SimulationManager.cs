using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public TrafficManager trafficManager;
    public PedestrianManager pedestrianManager;
    public ScenarioManager scenarioManager;

    void Start()
    {
        StartSimulation();
    }

    public void StartSimulation()
    {
        Debug.Log("Starting Simulation");

        trafficManager.Initialize();
        pedestrianManager.Initialize();

        scenarioManager.StartScenario(
            ScenarioType.VehicleCutIn
        );
    }

    public void ResetSimulation()
    {
        Debug.Log("Resetting Simulation");

        trafficManager.ResetTraffic();
        pedestrianManager.ResetPedestrians();

        scenarioManager.ResetScenario();
    }
}
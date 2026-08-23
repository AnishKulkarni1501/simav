public interface ISimulatedAgent
{
    void InitializeAgent();
    void SetDestination(UnityEngine.Vector3 destination);
    void StopAgent();
    void ResumeAgent();
}
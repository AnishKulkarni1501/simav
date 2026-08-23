using UnityEngine;
using Unity.Barracuda;

public class ModelLoader : MonoBehaviour
{
public NNModel modelAsset;
private Model runtimeModel;
private IWorker worker;

void Start()
{
    runtimeModel = Unity.Barracuda.ModelLoader.Load(modelAsset);
    worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);

    Debug.Log("Model loaded successfully");
}

void OnDestroy()
{
    worker.Dispose();
}

}

using UnityEngine;
using Unity.Barracuda;   // ← confirm this is present and exactly this

public class ObjectDetection : MonoBehaviour
{
    public NNModel modelAsset;
    public RenderTexture inputTexture;

    private Model runtimeModel;
    private IWorker worker;
    private Tensor inputTensor;

    private int numClasses = 5;
    private float confidenceThreshold = 0.5f;
    private string[] classNames = { "person", "car", "bike", "truck", "bus" };

    void Start()
    {
       // runtimeModel = ModelLoader.DontDestroyOnLoad();  // correct per docs
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
        Debug.Log("Model loaded successfully");
    }

    void Update()
    {
        inputTensor?.Dispose();
        inputTensor = new Tensor(inputTexture, 3);

        worker.Execute(inputTensor);

        Tensor output = worker.PeekOutput();
        ProcessOutput(output);
        // Do NOT dispose output — worker owns it via PeekOutput
    }

    void ProcessOutput(Tensor output)
    {
        const int numAnchors = 8400;
        float imgWidth  = 640f;
        float imgHeight = 640f;
        int printed = 0;

        float[] data = output.ToReadOnlyArray();

        for (int i = 0; i < numAnchors; i++)
        {
            float cx = data[0 * numAnchors + i];
            float cy = data[1 * numAnchors + i];
            float w  = data[2 * numAnchors + i];
            float h  = data[3 * numAnchors + i];

            float maxScore = 0f;
            int   classId  = -1;

            for (int c = 0; c < numClasses; c++)
            {
                float score = data[(4 + c) * numAnchors + i];
                if (score > maxScore) { maxScore = score; classId = c; }
            }

            if (maxScore < confidenceThreshold || classId < 0) continue;

            float xMin = (cx - w / 2f) * imgWidth;
            float yMin = (cy - h / 2f) * imgHeight;

            if (printed++ < 5)
            {
                Debug.Log($"Detected: {classNames[classId]} | Conf: {maxScore:F2}");
                Debug.Log($"Box → x:{xMin:F1}, y:{yMin:F1}, w:{w*imgWidth:F1}, h:{h*imgHeight:F1}");
            }
        }
    }

    void OnDestroy()
    {
        inputTensor?.Dispose();
        worker?.Dispose();
    }
}
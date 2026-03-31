using UnityEngine;
using Unity.Barracuda;

public class ObjectDetection : MonoBehaviour
{
    public NNModel modelAsset;

    private Model runtimeModel;
    private IWorker worker;

    public RenderTexture inputTexture;

    private Tensor inputTensor;

    private int numClasses = 5;
    private float confidenceThreshold = 0.5f;

    private string[] classNames = { "person", "car", "bike", "truck", "bus" };

    void Start()
    {
        runtimeModel = ModelLoader.Load(modelAsset.modelData.Value);  // ✅ Pass raw bytes
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);

        Debug.Log("Model loaded successfully");
    }

    void Update()
    {
        inputTensor = new Tensor(inputTexture, 3);

        worker.Execute(inputTensor);

        Tensor output = worker.PeekOutput();

        ProcessOutput(output);

        inputTensor.Dispose();
        output.Dispose();
    }

    void ProcessOutput(Tensor output)
    {
        int numPredictions = 8400;

        float imgWidth = 640f;
        float imgHeight = 640f;

        int printed = 0;

        for (int i = 0; i < numPredictions; i++)
        {
            float x = Sigmoid(output[0, 0, i, 0]);
            float y = Sigmoid(output[0, 0, i, 1]);
            float w = Sigmoid(output[0, 0, i, 2]);
            float h = Sigmoid(output[0, 0, i, 3]);

            float objConf = Sigmoid(output[0, 0, i, 4]);

            float maxClass = 0f;
            int classId = -1;

            for (int c = 0; c < numClasses; c++)
            {
                float classScore = Sigmoid(output[0, 0, i, 5 + c]);

                if (classScore > maxClass)
                {
                    maxClass = classScore;
                    classId = c;
                }
            }

            float confidence = objConf * maxClass;

            if (confidence > confidenceThreshold && printed < 5)
            {
                printed++;

                float xMin = (x - w / 2f) * imgWidth;
                float yMin = (y - h / 2f) * imgHeight;

                float boxWidth = w * imgWidth;
                float boxHeight = h * imgHeight;

                yMin = imgHeight - yMin;

                Debug.Log($"Detected: {classNames[classId]} | Conf: {confidence:F2}");
                Debug.Log($"Box → x:{xMin:F1}, y:{yMin:F1}, w:{boxWidth:F1}, h:{boxHeight:F1}");
            }
        }
    }

    float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    void OnDestroy()
    {
        worker.Dispose();
    }
}
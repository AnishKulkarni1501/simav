using UnityEngine;
using Unity.Barracuda;

public class ObjectDetection : MonoBehaviour
{
    public NNModel modelAsset;
    public RenderTexture inputTexture;

    private Model runtimeModel;
    private IWorker worker;

    private int numClasses = 80;
    private float confidenceThreshold = 0.4f;

    void Start()
    {
        // ✅ FIXED: Use Barracuda ModelLoader
        runtimeModel = Unity.Barracuda.ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);

        Debug.Log("Model loaded successfully");
    }

    void Update()
{
    RenderTexture resized = RenderTexture.GetTemporary(640, 640);
    Graphics.Blit(inputTexture, resized);

    Tensor inputTensor = Preprocess(resized);

    worker.Execute(inputTensor);

    Tensor output = worker.CopyOutput(); // ✅ FIX

    Debug.Log($"📐 Output shape: {output.shape}");

    ProcessOutput(output);

    inputTensor.Dispose();
    output.Dispose();

    worker.FlushSchedule(); // ✅ FIX

    RenderTexture.ReleaseTemporary(resized);
}

    // ✅ Preprocess → NCHW
    Tensor Preprocess(RenderTexture rt)
    {
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);

        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        Color[] pixels = tex.GetPixels();

        Tensor tensor = new Tensor(1, 3, rt.height, rt.width);

        for (int i = 0; i < pixels.Length; i++)
        {
            Color c = pixels[i];

            int x = i % rt.width;
            int y = i / rt.width;

            tensor[0, 0, y, x] = c.r;
            tensor[0, 1, y, x] = c.g;
            tensor[0, 2, y, x] = c.b;
        }

        Destroy(tex); // ✅ prevent memory leak

        return tensor;
    }

    void ProcessOutput(Tensor output)
    {
        int numPredictions = output.shape[2]; // assuming (1,1,25200,85)

        int printed = 0;

        for (int i = 0; i < numPredictions; i++)
        {
            float x = output[0, 0, i, 0];
            float y = output[0, 0, i, 1];
            float w = output[0, 0, i, 2];
            float h = output[0, 0, i, 3];

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

            // Debug first few
            if (i < 5)
            {
                Debug.Log($"[DEBUG] objConf: {objConf:F4}, class: {maxClass:F4}, final: {confidence:F4}");
            }

            if (confidence > confidenceThreshold && printed < 5)
            {
                printed++;

                float xMin = x - w / 2f;
                float yMin = y - h / 2f;

                Debug.Log($"✅ Detected: {GetClassName(classId)} | Conf: {confidence:F2}");
                Debug.Log($"📦 Box → x:{xMin:F1}, y:{yMin:F1}, w:{w:F1}, h:{h:F1}");
            }
        }
    }

    float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    string GetClassName(int id)
    {
        switch (id)
        {
            case 0: return "person";
            case 2: return "car";
            case 3: return "motorcycle";
            case 5: return "bus";
            case 7: return "truck";
            default: return "other";
        }
    }

    void OnDestroy()
    {
        worker?.Dispose();
    }
}
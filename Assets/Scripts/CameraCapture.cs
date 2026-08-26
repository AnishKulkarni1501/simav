using UnityEngine;
using System.IO;

public class CameraCapture : MonoBehaviour
{
    public Camera targetCamera;

    public int width = 640;
    public int height = 360;

    // How often we attempt a new detection.
    // 0.3 = approximately 3.3 requests/sec.
    public float detectionInterval = 0.3f;
    public float CurrentSteering { get; private set; }
    public float FrontDistance { get; private set; }

    private DetectionClient detectionClient;

    private float timer = 0f;

    // Prevent sending another image while the previous
    // request is still being processed.
    private bool waitingForDetection = false;

    void Start()
    {
        detectionClient = GetComponent<DetectionClient>();

        if (detectionClient == null)
        {
            Debug.LogError(
                "DetectionClient component not found on MLSystem!"
            );
        }
        else
        {
            Debug.Log(
                "DetectionClient found successfully."
            );
        }
    }

    void Update()
    {
        if (detectionClient == null)
            return;

        // Don't start another request while one is running.
        if (waitingForDetection)
            return;

        timer += Time.deltaTime;

        if (timer >= detectionInterval)
        {
            timer = 0f;

            CaptureImage();
        }
    }

    void CaptureImage()
    {
        waitingForDetection = true;

        RenderTexture rt = new RenderTexture(
            width,
            height,
            24
        );

        targetCamera.targetTexture = rt;

        RenderTexture.active = rt;

        Texture2D image = new Texture2D(
            width,
            height,
            TextureFormat.RGB24,
            false
        );

        targetCamera.Render();

        image.ReadPixels(
            new Rect(0, 0, width, height),
            0,
            0
        );

        image.Apply();

        targetCamera.targetTexture = null;
        RenderTexture.active = null;

        byte[] bytes = image.EncodeToJPG();

        // Send image to detection server.
        detectionClient.SendImage(
            bytes,
            OnDetectionFinished
        );

        // Save locally for debugging.
        string path =
            Application.dataPath +
            "/captured_frame.jpg";

        File.WriteAllBytes(path, bytes);

        Destroy(rt);
        Destroy(image);
    }

    // Called when the server has finished processing
    // the current frame.
    void OnDetectionFinished()
    {
        waitingForDetection = false;
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DetectionClient : MonoBehaviour
{
    private string serverUrl =
        "http://localhost:8000/detect";

    // Reference to the visualizer
    public DetectionVisualizer visualizer;

    public void SendImage(
        byte[] imageBytes,
        Action onComplete
    )
    {
        StartCoroutine(
            SendImageCoroutine(
                imageBytes,
                onComplete
            )
        );
    }

    private IEnumerator SendImageCoroutine(
        byte[] imageBytes,
        Action onComplete
    )
    {
        WWWForm form = new WWWForm();

        form.AddBinaryData(
            "file",
            imageBytes,
            "frame.jpg",
            "image/jpeg"
        );

        using (UnityWebRequest request =
               UnityWebRequest.Post(
                   serverUrl,
                   form
               ))
        {
            Debug.Log(
                "Sending image to detection server..."
            );

            yield return request.SendWebRequest();

            if (
                request.result !=
                UnityWebRequest.Result.Success
            )
            {
                Debug.LogError(
                    "Detection server error: " +
                    request.error
                );

                onComplete?.Invoke();

                yield break;
            }

            string json =
                request.downloadHandler.text;

            // Convert JSON response into Unity objects
            DetectionResponse response =
                JsonUtility.FromJson<DetectionResponse>(
                    json
                );

            // ==========================================
            // SEND DETECTIONS TO VISUALIZER
            // ==========================================

            if (visualizer != null)
            {
                visualizer.UpdateDetections(response);
            }
            else
            {
                Debug.LogWarning(
                    "DetectionVisualizer is not assigned!"
                );
            }

            // ==========================================
            // DEBUG LOGGING
            // ==========================================

            foreach (
                Detection detection
                in response.detections
            )
            {
                Debug.Log(
                    "Detected: " +
                    detection.@class +
                    " | Confidence: " +
                    (
                        detection.confidence * 100f
                    ).ToString("F1") +
                    "%"
                );
            }

            Debug.Log(
                "Inference time: " +
                response.inference_time_ms +
                " ms"
            );

            // Tell CameraCapture that this request
            // has finished.
            onComplete?.Invoke();
        }
    }
}
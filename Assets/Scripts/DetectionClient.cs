using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DetectionClient : MonoBehaviour
{
    private string serverUrl =
        "http://localhost:8000/detect";


    // ==========================================
    // REFERENCES
    // ==========================================

    // Reference to the visualizer
    public DetectionVisualizer visualizer;

    // Reference to the perception processor
    public PerceptionProcessor perceptionProcessor;


    // ==========================================
    // SEND IMAGE
    // ==========================================

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


    // ==========================================
    // SEND IMAGE COROUTINE
    // ==========================================

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


            // ==========================================
            // SERVER ERROR CHECK
            // ==========================================

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


            // ==========================================
            // GET SERVER RESPONSE
            // ==========================================

            string json =
                request.downloadHandler.text;


            // ==========================================
            // CONVERT JSON RESPONSE
            // ==========================================

            DetectionResponse response =
                JsonUtility.FromJson<DetectionResponse>(
                    json
                );


            // ==========================================
            // SAFETY CHECK
            // ==========================================

            if (response == null)
            {
                Debug.LogError(
                    "Detection server returned an invalid response."
                );

                onComplete?.Invoke();

                yield break;
            }


            // ==========================================
            // SEND DETECTIONS TO VISUALIZER
            // ==========================================

            if (visualizer != null)
            {
                visualizer.UpdateDetections(
                    response
                );
            }
            else
            {
                Debug.LogWarning(
                    "DetectionVisualizer is not assigned!"
                );
            }


            // ==========================================
            // SEND DETECTIONS TO PERCEPTION PROCESSOR
            // ==========================================

            if (perceptionProcessor != null)
            {
                perceptionProcessor.ProcessDetections(
                    response
                );
            }
            else
            {
                Debug.LogWarning(
                    "PerceptionProcessor is not assigned!"
                );
            }


            // ==========================================
            // DEBUG LOGGING
            // ==========================================

            if (
                response.detections != null
            )
            {
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
            }


            // ==========================================
            // INFERENCE TIME
            // ==========================================

            Debug.Log(
                "Inference time: " +
                response.inference_time_ms +
                " ms"
            );


            // ==========================================
            // TELL CAMERACAPTURE REQUEST IS COMPLETE
            // ==========================================

            onComplete?.Invoke();
        }
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SendFrame : MonoBehaviour
{
    public Camera cam;
    public string serverUrl = "http://127.0.0.1:5000/detect";

    void Start()
    {
        StartCoroutine(SendFrames());
    }

    IEnumerator SendFrames()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // slower for testing

            // Create RenderTexture
            RenderTexture rt = new RenderTexture(640, 640, 24);
            cam.targetTexture = rt;

            // Capture image
            Texture2D tex = new Texture2D(640, 640, TextureFormat.RGB24, false);
            cam.Render();
            RenderTexture.active = rt;

            tex.ReadPixels(new Rect(0, 0, 640, 640), 0, 0);
            tex.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;

            // Convert to JPG
            byte[] bytes = tex.EncodeToJPG();

            // Send to server
            WWWForm form = new WWWForm();
            form.AddBinaryData("image", bytes, "frame.jpg", "image/jpeg");

            UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("SERVER RESPONSE: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("ERROR: " + www.error);
            }

            Destroy(rt);
            Destroy(tex);
        }
    }
}
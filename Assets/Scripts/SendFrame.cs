using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendFrame : MonoBehaviour
{
    public string serverUrl = "http://127.0.0.1:5000/detect";

    public BoundingBoxDrawer drawer;
    public RawImage display;

    void Start()
    {
        StartCoroutine(SendFrames());
    }

    IEnumerator SendFrames()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (display == null || display.texture == null)
            {
                Debug.LogError("No image assigned to RawImage!");
                yield break;
            }

            // ✅ Get texture from RawImage
            Texture2D sourceTex = display.texture as Texture2D;

            if (sourceTex == null)
            {
                Debug.LogError("Texture is not readable or not Texture2D!");
                yield break;
            }

            // ✅ Copy texture (safe way)
            Texture2D tex = new Texture2D(sourceTex.width, sourceTex.height, TextureFormat.RGB24, false);
            tex.SetPixels(sourceTex.GetPixels());
            tex.Apply();

            // ✅ Send to server
            byte[] bytes = tex.EncodeToJPG();

            WWWForm form = new WWWForm();
            form.AddBinaryData("image", bytes, "frame.jpg", "image/jpeg");

            UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                Debug.Log("SERVER RESPONSE: " + json);

                Detection[] detections = JsonHelper.FromJson<Detection>(json);

                if (drawer != null)
                {
                    drawer.DrawBoxes(detections, tex.width, tex.height);
                }
            }
            else
            {
                Debug.LogError("ERROR: " + www.error);
            }

            Destroy(tex); // safe cleanup
        }
    }
}
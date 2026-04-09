using UnityEngine;
using System.Collections.Generic;

public class BoundingBoxDrawer : MonoBehaviour
{
    public GameObject boxPrefab;
    public RectTransform canvas;

    private List<GameObject> boxes = new List<GameObject>();

    public void DrawBoxes(Detection[] detections, float imgWidth, float imgHeight)
    {
        foreach (var box in boxes)
            Destroy(box);
        boxes.Clear();

        float canvasWidth = canvas.rect.width;
        float canvasHeight = canvas.rect.height;

        foreach (var det in detections)
        {
            GameObject newBox = Instantiate(boxPrefab, canvas);
            boxes.Add(newBox);

            RectTransform rt = newBox.GetComponent<RectTransform>();

            // ✅ top-left anchor
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            float x = (det.x1 / imgWidth) * canvasWidth;
            float y = (det.y1 / imgHeight) * canvasHeight;
            float w = ((det.x2 - det.x1) / imgWidth) * canvasWidth;
            float h = ((det.y2 - det.y1) / imgHeight) * canvasHeight;

            rt.anchoredPosition = new Vector2(x, -y);
            rt.sizeDelta = new Vector2(w, h);
        }
    }
}
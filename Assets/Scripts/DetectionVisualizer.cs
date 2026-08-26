using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetectionVisualizer : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform detectionOverlay;
    public GameObject boundingBoxPrefab;

    [Header("Detection Settings")]
    [Range(0f, 1f)]
    public float confidenceThreshold = 0.5f;

    [Header("Visual Settings")]
    public float borderThickness = 3f;
    public float labelOffset = 4f;

    private List<GameObject> activeBoxes =
        new List<GameObject>();

    public void UpdateDetections(
        DetectionResponse response
    )
    {
        ClearBoxes();

        if (response == null ||
            response.detections == null)
        {
            return;
        }

        foreach (Detection detection
                 in response.detections)
        {
            if (detection.confidence <
                confidenceThreshold)
            {
                continue;
            }

            CreateBoundingBox(detection);
        }
    }

    private void CreateBoundingBox(
        Detection detection
    )
    {
        // =================================================
        // CREATE MAIN CONTAINER
        // =================================================

        GameObject box =
            Instantiate(
                boundingBoxPrefab,
                detectionOverlay
            );

        activeBoxes.Add(box);

        RectTransform rect =
            box.GetComponent<RectTransform>();

        // =================================================
        // YOLO IMAGE SIZE
        // =================================================

        float imageWidth = 640f;
        float imageHeight = 360f;

        // =================================================
        // DETECTION SIZE
        // =================================================

        float boxWidth =
            detection.x2 - detection.x1;

        float boxHeight =
            detection.y2 - detection.y1;

        // =================================================
        // NORMALIZE
        // =================================================

        float normalizedX =
            detection.x1 / imageWidth;

        float normalizedY =
            detection.y1 / imageHeight;

        float normalizedWidth =
            boxWidth / imageWidth;

        float normalizedHeight =
            boxHeight / imageHeight;

        // =================================================
        // CANVAS SIZE
        // =================================================

        float canvasWidth =
            detectionOverlay.rect.width;

        float canvasHeight =
            detectionOverlay.rect.height;

        // =================================================
        // CONVERT TO UI COORDINATES
        // =================================================

        float x =
            normalizedX * canvasWidth;

        float y =
            (
                1f -
                normalizedY -
                normalizedHeight
            ) * canvasHeight;

        float finalWidth =
            normalizedWidth * canvasWidth;

        float finalHeight =
            normalizedHeight * canvasHeight;

        // =================================================
        // POSITION CONTAINER
        // =================================================

        rect.anchorMin =
            new Vector2(0f, 0f);

        rect.anchorMax =
            new Vector2(0f, 0f);

        rect.pivot =
            new Vector2(0f, 0f);

        rect.anchoredPosition =
            new Vector2(x, y);

        rect.sizeDelta =
            new Vector2(
                finalWidth,
                finalHeight
            );

        // =================================================
        // CREATE BORDER
        // =================================================

        Color detectionColor =
            GetDetectionColor(
                detection.@class
            );

        CreateBorder(
            box.transform,
            "TopBorder",
            new Vector2(
                finalWidth,
                borderThickness
            ),
            new Vector2(
                0f,
                finalHeight - borderThickness
            ),
            detectionColor
        );

        CreateBorder(
            box.transform,
            "BottomBorder",
            new Vector2(
                finalWidth,
                borderThickness
            ),
            new Vector2(
                0f,
                0f
            ),
            detectionColor
        );

        CreateBorder(
            box.transform,
            "LeftBorder",
            new Vector2(
                borderThickness,
                finalHeight
            ),
            new Vector2(
                0f,
                0f
            ),
            detectionColor
        );

        CreateBorder(
            box.transform,
            "RightBorder",
            new Vector2(
                borderThickness,
                finalHeight
            ),
            new Vector2(
                finalWidth - borderThickness,
                0f
            ),
            detectionColor
        );

        // =================================================
        // LABEL
        // =================================================

        TMP_Text label =
            box.GetComponentInChildren<TMP_Text>();

        if (label != null)
        {
            label.text =
                detection.@class.ToUpper() +
                " " +
                (
                    detection.confidence * 100f
                ).ToString("F1") +
                "%";

            RectTransform labelRect =
                label.GetComponent<RectTransform>();

            labelRect.anchorMin =
                new Vector2(0f, 1f);

            labelRect.anchorMax =
                new Vector2(0f, 1f);

            labelRect.pivot =
                new Vector2(0f, 0f);

            labelRect.anchoredPosition =
                new Vector2(
                    0f,
                    labelOffset
                );

            labelRect.sizeDelta =
                new Vector2(
                    180f,
                    30f
                );

            label.color =
                Color.white;

            label.fontSize =
                18f;

            label.alignment =
                TextAlignmentOptions.Left |
                TextAlignmentOptions.Midline;
        }
    }

    // =====================================================
    // CREATE ONE BORDER SIDE
    // =====================================================

    private void CreateBorder(
        Transform parent,
        string borderName,
        Vector2 size,
        Vector2 position,
        Color color
    )
    {
        GameObject border =
            new GameObject(
                borderName,
                typeof(RectTransform),
                typeof(Image)
            );

        border.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
            border.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 0f);

        rect.anchorMax =
            new Vector2(0f, 0f);

        rect.pivot =
            new Vector2(0f, 0f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            border.GetComponent<Image>();

        image.color =
            color;

        image.raycastTarget =
            false;
    }

    // =====================================================
    // DETECTION COLORS
    // =====================================================

    private Color GetDetectionColor(
        string className
    )
    {
        switch (className.ToLower())
        {
            case "car":
                return Color.green;

            case "bus":
                return Color.yellow;

            case "truck":
                return new Color(
                    1f,
                    0.5f,
                    0f
                );

            case "person":
            case "pedestrian":
                return Color.red;

            case "motorcycle":
            case "motorbike":
                return Color.cyan;

            case "bicycle":
                return Color.magenta;

            default:
                return Color.white;
        }
    }

    // =====================================================
    // CLEAR OLD BOXES
    // =====================================================

    private void ClearBoxes()
    {
        foreach (GameObject box
                 in activeBoxes)
        {
            if (box != null)
            {
                Destroy(box);
            }
        }

        activeBoxes.Clear();
    }
}
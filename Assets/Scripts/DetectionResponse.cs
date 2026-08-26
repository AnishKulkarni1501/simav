using System;

[Serializable]
public class DetectionResponse
{
    public Detection[] detections;

    public float inference_time_ms;
}
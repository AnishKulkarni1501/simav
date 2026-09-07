using System;

[Serializable]
public class PerceivedObject
{
    // ==========================================
    // RAW YOLO INFORMATION
    // ==========================================

    public string className;
    public float confidence;

    public float x1;
    public float y1;
    public float x2;
    public float y2;

    // ==========================================
    // DERIVED PERCEPTION
    // ==========================================

    // Estimated distance from camera in metres
    public float distanceMeters;

    // Horizontal position relative to camera
    // -1 = left
    //  0 = center
    // +1 = right
    public float relativeX;

    // ==========================================
    // TRACKING
    // ==========================================

    // Will be used in CHG-002.3
    public int trackId;

    // Will be used in CHG-002.3
    public float age;

    // Whether the perception is currently valid
    public bool valid;
}
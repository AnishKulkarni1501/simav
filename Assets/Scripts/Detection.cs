using System;

[Serializable]
public class Detection
{
    public string @class;
    public float confidence;

    public float x1;
    public float y1;
    public float x2;
    public float y2;
}
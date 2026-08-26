using TMPro;
using UnityEngine;

public class VehicleUI : MonoBehaviour
{
    public CarsController car;
    public CameraCapture cameraCapture;

    public TMP_Text speedText;
    public TMP_Text surfaceText;
  
    public TMP_Text collisionText;
    public TMP_Text fpsText;

    void Update()
    {
        if (car == null || cameraCapture == null)
            return;

        speedText.text =
            $"Speed: {car.CurrentSpeed:F1} km/h";

        surfaceText.text =
            $"Surface: {car.CurrentMaterial}";

        collisionText.text =
            $"Collisions: {car.CollisionCount}";

        fpsText.text =
            $"FPS: {car.CurrentFPS:F0}";
    }
}
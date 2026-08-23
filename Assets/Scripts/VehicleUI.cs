using TMPro;
using UnityEngine;

public class VehicleUI : MonoBehaviour
{
    public CarsController car;

    public TMP_Text speedText;
    public TMP_Text surfaceText;

    void Update()
    {
        speedText.text = $"Speed: {car.CurrentSpeed:F1} km/h";
        surfaceText.text = $"Surface: {car.CurrentMaterial}";
    }
}
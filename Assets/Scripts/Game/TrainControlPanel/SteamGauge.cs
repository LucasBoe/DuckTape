using UnityEngine;

public class SteamGauge : SectionSpecficUI
{
    [SerializeField] private RectTransform indicator;

    private const float minValue = 0f;
    private const float maxValue = 10f;
    private const float minAngle = 135f;
    private const float maxAngle = -145f;

    void FixedUpdate()
    {
        var speed = DriveHandler.Instance.Acceleration;

        // Clamp to safe range
        float clampedValue = Mathf.Clamp(speed, minValue, maxValue);

        // Map 0–30 ? 135° to -135°
        float normalized = (clampedValue - minValue) / (maxValue - minValue);
        float angle = Mathf.Lerp(minAngle, maxAngle, normalized);

        // Apply rotation (Z axis)
        if (indicator != null)
            indicator.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}

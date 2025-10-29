using UnityEngine;
using TMPro;

public class SandGauge : SectionSpecficUI
{
    [SerializeField] private RectTransform indicator;
    [SerializeField] private TextMeshProUGUI maxSandTmp;

    private const float minValue = 0f;
    private float maxValue = 10f;
    private const float minAngle = 120f;
    private const float maxAngle = -125f;

    void LateUpdate()
    {
        if (DriveHandler.Instance.Engine == null)
            return;

        maxValue = DriveHandler.Instance.Engine.EngineConfig.MaxSandStorage;
        maxSandTmp.text = maxValue + "t";

        var sand = DriveHandler.Instance.Engine.Sand;

        // Clamp to safe range
        float clampedValue = Mathf.Clamp(sand, minValue, maxValue);

        // Map 0–30 ? 135° to -135°
        float normalized = (clampedValue - minValue) / (maxValue - minValue);
        float angle = Mathf.Lerp(minAngle, maxAngle, normalized);

        // Apply rotation (Z axis)
        if (indicator != null)
            indicator.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}

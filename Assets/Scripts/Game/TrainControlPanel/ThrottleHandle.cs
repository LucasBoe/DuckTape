using UnityEngine;
using UnityEngine.EventSystems;

public class ThrottleHandle : SectionSpecficUI, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField] private float minAngle = -90f;
    [SerializeField] private float maxAngle = 50f;
    [SerializeField] private float rotationSpeed = 8f; // smooth speed (smaller = slower)

    private float targetAngle;
    private bool isDragging;

    private void Start()
    {
        if (handle == null)
            handle = GetComponent<RectTransform>();

        // initialize rotation
        targetAngle = handle.localEulerAngles.z;
        if (targetAngle > 180f) targetAngle -= 360f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // Get direction vector from handle center to mouse
        Vector2 handleCenter = RectTransformUtility.WorldToScreenPoint(null, handle.position);
        Vector2 dir = eventData.position - handleCenter;

        // Calculate angle in degrees, rotate 90° so up is center
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;

        // Clamp between limits
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        targetAngle = angle;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void Update()
    {
        // Smoothly interpolate towards the target angle for slower drag feel
        float current = handle.localEulerAngles.z;
        if (current > 180f) current -= 360f; // normalize

        float newAngle = Mathf.Lerp(current, targetAngle, Time.deltaTime * rotationSpeed);
        handle.localRotation = Quaternion.Euler(0f, 0f, newAngle);
    }

    // Optional normalized value (0–1)
    public float GetNormalizedValue()
    {
        return Mathf.InverseLerp(minAngle, maxAngle, targetAngle);
    }
}

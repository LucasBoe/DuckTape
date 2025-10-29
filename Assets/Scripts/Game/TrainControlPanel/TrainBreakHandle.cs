using UnityEngine;
using UnityEngine.EventSystems;

public class TrainBrakeHandle : SectionSpecficUI, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField] private float maxPullDistance = 150f;
    [SerializeField] private float activationThreshold = 100f;
    [SerializeField] private float returnSpeed = 5f;

    private Vector2 startPos;
    private bool isDragging = false;
    private float currentPull = 0f;
    private Vector2 dragOffset; 
    private Camera uiCamera;

    private void Start()
    {
        if (handle == null)
            handle = GetComponent<RectTransform>();

        startPos = handle.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        // Find the event camera (important for canvases in World/Camera space)
        uiCamera = eventData.pressEventCamera;

        // Calculate mouse offset so the handle doesn’t snap to cursor
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handle.parent as RectTransform,
            eventData.position,
            uiCamera,
            out Vector2 localMousePos
        );

        dragOffset = handle.anchoredPosition - localMousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handle.parent as RectTransform,
            eventData.position,
            uiCamera,
            out Vector2 localMousePos
        );

        Vector2 targetPos = localMousePos + dragOffset;
        float pullDistance = Mathf.Clamp(targetPos.y - startPos.y, -maxPullDistance, 0f);
        currentPull = -pullDistance; // store as positive distance
        handle.anchoredPosition = new Vector2(startPos.x, startPos.y + pullDistance);


        if (currentPull >= activationThreshold)
            DriveHandler.Instance.Break();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void Update()
    {
        if (!isDragging)
        {
            handle.anchoredPosition = Vector2.Lerp(
                handle.anchoredPosition,
                startPos,
                Time.deltaTime * returnSpeed
            );

            if (Vector2.Distance(handle.anchoredPosition, startPos) < 0.1f)
            {
                handle.anchoredPosition = startPos;
                DriveHandler.Instance.Unbreak();
                currentPull = 0f;
            }
        }
    }

    public float GetNormalizedPull()
    {
        return Mathf.InverseLerp(0f, maxPullDistance, currentPull);
    }
}

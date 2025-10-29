using UnityEngine;
using UnityEngine.EventSystems;

public class TrainHornHandle : SectionSpecficUI, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Setup")]
    [SerializeField] private RectTransform handle;
    [SerializeField] private float maxPullDistance = 150f;
    [SerializeField] private float activationThreshold = 100f;
    [SerializeField] private float returnSpeed = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource brakeAudioSource;
    [SerializeField] private float fadeSpeed = 2f;  // speed of fade in/out
    [SerializeField] private float maxVolume = 1f;  // peak volume when fully active

    [Header("Runtime")]
    public bool BrakeActivated { get; private set; }

    private Vector2 startPos;
    private bool isDragging = false;
    private float currentPull = 0f;
    private Vector2 dragOffset;
    private Camera uiCamera;

    private void Start()
    {
        if (handle == null)
            handle = GetComponent<RectTransform>();

        if (brakeAudioSource == null)
            brakeAudioSource = GetComponent<AudioSource>();

        brakeAudioSource.loop = true;
        brakeAudioSource.volume = 0f;
        startPos = handle.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        uiCamera = eventData.pressEventCamera;

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
        currentPull = -pullDistance;
        handle.anchoredPosition = new Vector2(startPos.x, startPos.y + pullDistance);

        bool wasActivated = BrakeActivated;
        BrakeActivated = currentPull >= activationThreshold;

        // Detect activation change
        if (BrakeActivated && !wasActivated)
            StartFadeIn();
        else if (!BrakeActivated && wasActivated)
            StartFadeOut();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    private void Update()
    {
        if (!isDragging)
        {
            handle.anchoredPosition = Vector2.Lerp(handle.anchoredPosition, startPos, Time.deltaTime * returnSpeed);

            if (Vector2.Distance(handle.anchoredPosition, startPos) < 0.1f)
            {
                handle.anchoredPosition = startPos;
                BrakeActivated = false;
                currentPull = 0f;
                StartFadeOut(); // stop sound if returning to top
            }
        }
    }

    // Fade coroutines
    private void StartFadeIn()
    {
        if (!brakeAudioSource.isPlaying)
            brakeAudioSource.Play();

        StopAllCoroutines();
        StartCoroutine(FadeAudio(brakeAudioSource.volume, maxVolume));
    }

    private void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeAudio(brakeAudioSource.volume, 0f, stopAfterFade: true));
    }

    private System.Collections.IEnumerator FadeAudio(float from, float to, bool stopAfterFade = false)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            brakeAudioSource.volume = Mathf.Lerp(from, to, t);
            yield return null;
        }

        brakeAudioSource.volume = to;
        if (stopAfterFade && Mathf.Approximately(to, 0f))
            brakeAudioSource.Stop();
    }

    // Optional normalized brake strength (0–1)
    public float GetNormalizedPull()
    {
        return Mathf.InverseLerp(0f, maxPullDistance, currentPull);
    }
}


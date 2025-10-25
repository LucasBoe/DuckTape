using System;
using System.Collections.Generic;
using SS;
using UnityEngine;
using UnityEngine.UI;

public class SectionUI : SectionSpecficUI, IDelayedStartObserver
{
    public Section section;
    [SerializeField] private Slider engineIconProgressionSlider;
    [SerializeField] private Image iconDummy;
    [SerializeField] private Sprite slopeUpSprite, slopeDownSprite;

    private void Awake()
    {
        iconDummy.gameObject.SetActive(false);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        DriveHandler.Instance.CurrentSectionChangedEvent.AddListener(OnCurrentSectionChanged);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        
        if (DriveHandler.InstanceExists)
            DriveHandler.Instance.CurrentSectionChangedEvent.RemoveListener(OnCurrentSectionChanged);
    }
    public void DelayedStart()
    {
        OnCurrentSectionChanged(section);
    }
    private void OnCurrentSectionChanged(Section section)
    {
        if (!section)
            return;
        
        this.section = section;
        
        iconDummy.transform.parent.DestroyAllChildren(iconDummy.transform);
        
        var slopes = AnimationCurveToFloatArray(section.SlopeOverSection);
        var high = GetClosestPoint(slopes, -100);
        var low = GetClosestPoint(slopes, 100);

        if (high.Item1 > 0)
            CreateIcon(high.Item2, slopeUpSprite);

        if (low.Item1 > 0)
            CreateIcon(low.Item2, slopeDownSprite);
    }

    private void CreateIcon(float x, Sprite sprite)
    {
        Image instance = Instantiate(iconDummy, iconDummy.transform.parent);
        instance.gameObject.SetActive(true);
        instance.sprite = sprite;
        (instance.transform as RectTransform).pivot = new Vector2(x, .5f);
        (instance.transform as RectTransform).anchoredPosition = Vector2.zero;
    }

    private Tuple<float, float> GetClosestPoint(float[] slopes, float goal)
    {
        int lastX = -1;
        float lastY = 0f;

        for (var index = 0; index < slopes.Length; index++)
        {
            float currentY = slopes[index];
            if (Distance(currentY, goal) < Distance(lastY, goal))
            {
                lastX = index;
                lastY = currentY;
            }
        }
        
        Debug.Log($"closest: {lastX}, {lastY}");
        
        return new Tuple<float, float>(lastX / 100f, lastY);

        float Distance(float x, float y)
        {
            return Mathf.Abs(x - y);
        }
    }

    void Update()
    {
        engineIconProgressionSlider.value = DriveHandler.Instance.Progression;
    }

    private float[] AnimationCurveToFloatArray(AnimationCurve curve)
    {
        List<float> floats = new();
        for (int i = 0; i < 100; i++)
        {
            float f = i / 100f;
            floats.Add(curve.Evaluate(f));
        }
        return floats.ToArray();
    }
}

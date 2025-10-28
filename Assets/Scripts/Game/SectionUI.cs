using System;
using System.Collections.Generic;
using SS;
using UnityEngine;
using UnityEngine.UI;

public class SectionUI : SectionSpecficUI, IDelayedStartObserver
{
    private WorldMapConnector section;
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
        OnCurrentSectionChanged(null);
    }
    private void OnCurrentSectionChanged(WorldMapConnector section)
    {
        if (!section)
            return;
        
        this.section = section;
        
        iconDummy.transform.parent.DestroyAllChildren(iconDummy.transform);

        foreach (Tuple<Sprite, float> icon in section.Section.GetIcons())
        {
            CreateIcon(icon.Item2, icon.Item1);
        }
    }

    private void CreateIcon(float x, Sprite sprite)
    {
        Image instance = Instantiate(iconDummy, iconDummy.transform.parent);
        instance.gameObject.SetActive(true);
        instance.sprite = sprite;
        (instance.transform as RectTransform).pivot = new Vector2(x, .5f);
        (instance.transform as RectTransform).anchoredPosition = Vector2.zero;
    }
    void Update()
    {
        engineIconProgressionSlider.value = DriveHandler.Instance.Progression;
    }
}

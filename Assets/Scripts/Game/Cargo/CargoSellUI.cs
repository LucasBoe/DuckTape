using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SectionSpecficUI : GamePhaseUI
{
    protected override void SetVisible(bool visible)
    {
        if (visible)
            TryShow();
        else
            TryHide();
    }
    private void TryShow()
    {
        transform.DOScaleY(1f, .3f).SetEase(Ease.OutBack);
    }
    private void TryHide()
    {
        transform.DOScaleY(0f, .3f).SetEase(Ease.InSine);
    }
}
public class CargoSellUI : SectionSpecficUI
{
    [SerializeField] private CargoConfigContainer cargos;
    [SerializeField] private GameObject dummyObject;
    
    List<SellUIElement> elements = new();
    private void Awake()
    {
        dummyObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StationHandler.Instance.EnterStationEvent.AddListener(OnEnterStation);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StationHandler.Instance.EnterStationEvent.RemoveListener(OnEnterStation);
    }
    private void OnEnterStation(WorldMapNode obj)
    {
        foreach (var element in elements)
            element.Instance.SetActive(obj.Config.Takes.Contains(element.Cargo));
    }
    private void Start()
    {
        foreach (var cargo in cargos.All)
        {
            var instance = Instantiate(dummyObject, dummyObject.transform.parent);
            var image = instance.GetComponentInChildren<Image>();
            var text = instance.GetComponentInChildren<TMP_Text>();
            
            image.sprite = cargo.Sprite;
            text.text = $"{cargo.Value}$";
            
            instance.SetActive(true);
            elements.Add(new SellUIElement()
            {
                Image = image,
                Text = text,
                Cargo = cargo,
                Instance = instance
            });
        }
    }
    
    public class SellUIElement
    {
        public CargoConfigBase Cargo;
        public GameObject Instance;
        public Image Image;
        public TMP_Text Text;
    }
}

using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
public class CargoSellUI : SectionSpecficUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CargoConfigContainer cargos;
    [SerializeField] private GameObject dummyObject;
    
    List<SellUIElement> elements = new();
    private WorldMapNode station;
    private bool isHovered;
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
    private void OnEnterStation(WorldMapNode station)
    {
        foreach (var element in elements)
            element.Instance.SetActive(station.Config.Takes.Contains(element.Cargo));
        
        this.station = station;
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
    private void Update()
    {
        if (!isHovered)
            return;
        
        if (!SpatialInventoryHandler.Instance.IsDragging)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            bool canSell = CanSellAtCurrentStation(SpatialInventoryHandler.Instance.CurrentItem);
            if (canSell)
                SpatialInventoryHandler.Instance.SellCurrentTo(station);
        }
    }
    private bool CanSellAtCurrentStation(SpatialInventoryItem cargo)
    {
        if (!StationHandler.Instance.CurrentStation.Config.Takes.Contains(cargo.Cargo))
            return false;

        if (cargo.OriginStationID == StationHandler.Instance.CurrentStation.ID)
            return false;
        
        return true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        
        bool isDragging = SpatialInventoryHandler.Instance.IsDragging;
        if (!isDragging)
            return;

        GetComponent<Image>().color = CanSellAtCurrentStation(SpatialInventoryHandler.Instance.CurrentItem) ? Color.green : Color.red;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        GetComponent<Image>().color = Color.gray;
    }
}

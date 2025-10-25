using System;
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
    private void Awake()
    {
        dummyObject.SetActive(false);
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
        }
    }
}

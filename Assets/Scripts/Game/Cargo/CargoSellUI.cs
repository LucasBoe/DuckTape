using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SectionSpecficUI : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool isVisible;
    protected abstract LoopSection GetAssociatedSection();
    protected virtual void OnEnable()
    {
        LoopEventHandler.Instance.LoopSectionSwitchedEvent.AddListener(OnLoopSectionSwitched);
    }
    protected virtual void OnDisable()
    {
        LoopEventHandler.Instance.LoopSectionSwitchedEvent.RemoveListener(OnLoopSectionSwitched);
    }
    private void OnLoopSectionSwitched(global::LoopSection section)
    {
        if (section == GetAssociatedSection())
            TryShow();
        else
            TryHide();
    }
    private void TryShow()
    {
        isVisible = true;
        transform.DOScaleY(1f, .3f).SetEase(Ease.OutBack);
    }
    private void TryHide()
    {
        isVisible = false;
        transform.DOScaleY(0f, .3f).SetEase(Ease.InSine);
    }
}
public class StationUI : SectionSpecficUI
{
    protected override LoopSection GetAssociatedSection() => LoopSection.Station;
}
public class DriveUI : SectionSpecficUI
{
    protected override LoopSection GetAssociatedSection() => LoopSection.Drive;
}
public class CargoSellUI : StationUI
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

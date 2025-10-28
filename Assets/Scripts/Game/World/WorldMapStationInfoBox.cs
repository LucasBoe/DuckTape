using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapStationInfoBox : MonoBehaviour
{
    [SerializeField] TMP_Text stationNameText;
    [SerializeField] Image takeIconDummy, sellIconDummy;
    
    private Tweener activeTween;
    private List<Image> iconInstances = new();

    private void Awake()
    {
        takeIconDummy.gameObject.SetActive(false);
        sellIconDummy.gameObject.SetActive(false);
        Hide();
    }
    public void Hide()
    {
        activeTween?.Kill();
        activeTween = transform.DOScale(0, .25f);
        
        foreach (var icon in iconInstances)
            Destroy(icon.gameObject);
        iconInstances.Clear();
    }
    public void Show(WorldMapNode station)
    {
        transform.position = station.transform.position + .5f * Vector3.down;
        
        activeTween?.Kill();
        activeTween = transform.DOScale(1, .25f);

        stationNameText.text = station.name;
        CreateIcons(takeIconDummy, station.Config.Takes);
        CreateIcons(sellIconDummy, station.Config.Sells);
    }
    private void CreateIcons(Image dummy, List<CargoConfigBase> cargos)
    {
        foreach (var cargo in cargos)
        {
            var instance = Instantiate(dummy, dummy.transform.parent);
            instance.sprite = cargo.Sprite;
            instance.gameObject.SetActive(true);
            iconInstances.Add(instance);
        }
    }
}

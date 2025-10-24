using System;
using System.Collections.Generic;
using SS;
using TMPro;
using UnityEngine;

public class WorldMapNode : MonoBehaviour, ISelectableWorldMapElement
{
    public StationConfig Config;
    [SerializeField] private SpriteRenderer nodeRenderer;
    [SerializeField] private List<StationTypeSpritePair> stationTypeSpritePairs;
    [SerializeField] private Sprite obscuredStationSprite;
    [SerializeField] private TMP_Text text;
    [SerializeField] private SpriteRenderer currentStationPointer;
    [SerializeField] private Collider2D collider;

    private bool isVisible;
    private bool isSelected;
    
    public void Apply(StationConfig config)
    {
        name = config.commaSeparatedListOfNames.Split(',').PickRandom();
        Config = config;
        text.text = name;
        text.gameObject.SetActive(false);
        
        nodeRenderer.sprite = RefreshSprite(config);
    }

    private Sprite RefreshSprite(StationConfig config)
    {
        foreach (var pair in stationTypeSpritePairs)
        {
            if (pair.Type == config.Type)
                return pair.Sprite;
        }
        
        return obscuredStationSprite;
    }

    private void OnMouseEnter()
    {
        text.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        text.gameObject.SetActive(isSelected);
    }
    private void OnMouseDown()
    {
        WorldMapHandler.Instance.TrySelect(this);
    }
    public void Refresh()
    {
        isVisible = StationHandler.Instance.CurrentStation == this || WorldMapHandler.Instance.ConnectionExists(StationHandler.Instance.CurrentStation, this);
        
        currentStationPointer.gameObject.SetActive(StationHandler.Instance.CurrentStation == this);
        nodeRenderer.sprite = isVisible ? RefreshSprite(Config) : obscuredStationSprite;
        text.gameObject.SetActive(isSelected);
        collider.enabled = isVisible;
    }
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        Refresh();
    }
}

[System.Serializable]
public class StationTypeSpritePair
{
    public StationType Type;
    public Sprite Sprite;
}
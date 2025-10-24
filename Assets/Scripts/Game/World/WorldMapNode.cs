using System.Collections.Generic;
using SS;
using TMPro;
using UnityEngine;

public class WorldMapNode : MonoBehaviour
{
    public StationConfig Config;
    [SerializeField] private SpriteRenderer nodeRenderer;
    [SerializeField] private List<StationTypeSpritePair> stationTypeSpritePairs;
    [SerializeField] private Sprite obscuredStationSprite;
    [SerializeField] private TMP_Text text;
    [SerializeField] private SpriteRenderer currentStationPointer;
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
        text.gameObject.SetActive(false);
    }
    public void Refresh()
    {
        bool shouldBeVisible = StationHandler.Instance.CurrentStation == this || WorldMapHandler.Instance.ConnectionExists(StationHandler.Instance.CurrentStation, this);
        
        currentStationPointer.gameObject.SetActive(StationHandler.Instance.CurrentStation == this);
        nodeRenderer.sprite = shouldBeVisible ? RefreshSprite(Config) : obscuredStationSprite;
    }
}

[System.Serializable]
public class StationTypeSpritePair
{
    public StationType Type;
    public Sprite Sprite;
}
using System.Collections.Generic;
using SS;
using TMPro;
using UnityEngine;

public class WorldMapNode : MonoBehaviour
{
    public StationConfig Config;
    [SerializeField] private SpriteRenderer nodeRenderer;
    [SerializeField] private List<StationTypeSpritePair> stationTypeSpritePairs;
    [SerializeField] private TMP_Text text;
    public void Apply(StationConfig config)
    {
        name = config.commaSeparatedListOfNames.Split(',').PickRandom();
        Config = config;
        text.text = name;
        text.gameObject.SetActive(false);
        
        foreach (var pair in stationTypeSpritePairs)
        {
            if (pair.Type == config.Type)
                nodeRenderer.sprite = pair.Sprite;
        }
    }
    private void OnMouseEnter()
    {
        text.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        text.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class StationTypeSpritePair
{
    public StationType Type;
    public Sprite Sprite;
}
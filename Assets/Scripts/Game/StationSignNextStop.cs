using System;
using TMPro;
using UnityEngine;

public class StationSignNextStop : MonoBehaviour
{
    [SerializeField] private SpriteRenderer borderSprite;
    [SerializeField] private TMP_Text nameText;

    private void Awake()
    {
        OnTargetStationChanged(null);
    }
    private void OnEnable()
    {
        StationHandler.Instance.TargetStationChangedEvent.AddListener(OnTargetStationChanged);
    }

    private void OnDisable()
    {
        StationHandler.Instance.TargetStationChangedEvent.RemoveListener(OnTargetStationChanged);
    }
    private void OnTargetStationChanged(WorldMapNode obj)
    {
        nameText.text = $"Next: {(obj ? obj.name : "???")}";
        nameText.color = obj ? Color.white : Color.red;
        borderSprite.color = obj ? Color.white : Color.red;
    }
}

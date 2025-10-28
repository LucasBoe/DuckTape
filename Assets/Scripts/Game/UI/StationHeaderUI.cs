using TMPro;
using UnityEngine;

public class StationHeaderUI : SectionSpecficUI
{
    [SerializeField] TMP_Text stationNameText;
    protected override void OnEnable()
    {
        base.OnEnable();
        StationHandler.Instance.EnterStationEvent.AddListener(OnStationEnter);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StationHandler.Instance.EnterStationEvent.RemoveListener(OnStationEnter);
    }
    private void OnStationEnter(WorldMapNode station)
    {
        stationNameText.text = station.name;
    }
}

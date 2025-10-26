using SS;
using UnityEngine;
using Event = SS.Event;

public class StationHandler : Singleton<StationHandler>
{
    public WorldMapNode CurrentStation { get; private set; }
    public WorldMapNode NextStation { get; private set; }
    public Event<WorldMapNode> TargetStationChangedEvent = new();
    public Event<WorldMapNode> EnterStationEvent = new();
    public void SetStation(WorldMapNode station)
    {
        CurrentStation = station;
    }
    public void ModifyTargetStation(WorldMapNode target)
    {
        NextStation = target;
        TargetStationChangedEvent?.Invoke(target);
    }

    public WorldMapNode EnterNextStation()
    {
        CurrentStation = NextStation;
        NextStation = null;
        TargetStationChangedEvent?.Invoke(null);
        EnterStationEvent?.Invoke(CurrentStation);
        return CurrentStation;
    }
}

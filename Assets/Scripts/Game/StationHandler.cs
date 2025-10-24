using SS;
using UnityEngine;

public class StationHandler : Singleton<StationHandler>
{
    public WorldMapNode CurrentStation { get; private set; }
    public WorldMapNode NextStation { get; private set; }
    public void SetStation(WorldMapNode station)
    {
        CurrentStation = station;
    }
    public void ModifyTargetStation(WorldMapNode selectedNode)
    {
        NextStation = selectedNode;
    }

    public WorldMapNode EnterNextStation()
    {
        CurrentStation = NextStation;
        NextStation = null;
        return CurrentStation;
    }
}

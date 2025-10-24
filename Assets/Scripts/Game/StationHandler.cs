using SS;
using UnityEngine;

public class StationHandler : Singleton<StationHandler>
{
    public WorldMapNode CurrentStation { get; private set; }
    public void SetStation(WorldMapNode station)
    {
        CurrentStation = station;
    }
}

using System.Collections.Generic;
using SS;
using UnityEngine;

public class SessionHandler : MonoBehaviour, IDelayedStartObserver
{
    [SerializeField] Train train;
    [SerializeField] List<WagonConfigBase> startupWagons;
    [SerializeField] private int startupMoney = 12;
    public void DelayedStart()
    {
        foreach (var wagon in startupWagons)
            train.AppendFromConfig(wagon);
        
        DriveHandler.Instance.AssignTrainInstance(train);
        MoneyHandler.Instance.ChangeMoney(startupMoney);

        var startStation = WorldMapHandler.Instance.PickStartStation();
        StationHandler.Instance.SetStation(startStation);
    }
}

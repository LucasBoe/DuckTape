using System.Collections.Generic;
using SS;
using UnityEngine;

public class SessionHandler : MonoBehaviour, IDelayedStartObserver
{
    [SerializeField] Train train;
    [SerializeField] List<WagonConfigBase> startupWagons;
    public void DelayedStart()
    {
        foreach (var wagon in startupWagons)
            train.AppendFromConfig(wagon);
    }
}

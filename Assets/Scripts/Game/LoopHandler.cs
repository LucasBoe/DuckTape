using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using SS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Event = SS.Event;

public enum LoopSection
{
    Station,
    Drive
}
public class LoopHandler : MonoBehaviour, IDelayedStartObserver
{
    [SerializeField] LoopSection currentSectionType = LoopSection.Station;
    [SerializeField] Section defaultSection;
    private bool driveToNextStation = false;
    private bool reachedEnd = false;
    
    [SerializeField] EnvironmentHandler envHandler;

    private string status = "IDLE";

    [Button]
    private void DriveToNextStation()
    {
        if (currentSectionType != LoopSection.Station)
           return;
        
        driveToNextStation = true;
    }
    public void DelayedStart()
    {
        StartCoroutine(LoopRoutine());
        DriveHandler.Instance.OnCurrentSectionEndReached.AddListener(OnCurrentSectionEndReached);
    }
    private void OnDestroy()
    {
        if (DriveHandler.InstanceExists)
            DriveHandler.Instance.OnCurrentSectionEndReached.RemoveListener(OnCurrentSectionEndReached);
    }
    private void OnCurrentSectionEndReached()
    {
        reachedEnd = true;
    }
    private IEnumerator LoopRoutine()
    {
        Refuel();
            
        while (true)
        {
            status = "STATION";
            LoopEventHandler.Instance.OnStationEnterEvent?.Invoke();
            LoopEventHandler.Instance.LoopSectionSwitchedEvent?.Invoke(LoopSection.Station);
            GamePhaseHandler.Instance.SetGamePhase(GamePhase.InStation);
            
            
            driveToNextStation = false;
            while (!driveToNextStation)
                yield return null;
            
            envHandler.BeginDriveEvent?.Invoke();
            currentSectionType = LoopSection.Drive;
            GamePhaseHandler.Instance.SetGamePhase(GamePhase.Driving);
            
            status = "DRIVE";
            DriveHandler.Instance.Unbreak();
            LoopEventHandler.Instance.OnStationExitEvent?.Invoke();
            LoopEventHandler.Instance.LoopSectionSwitchedEvent?.Invoke(LoopSection.Drive);

            reachedEnd = false;
            while (!reachedEnd)
                yield return null;
            
            envHandler.EndDriveEvent?.Invoke();
            currentSectionType = LoopSection.Station;
            GamePhaseHandler.Instance.SetGamePhase(GamePhase.EnteringStation);
            StationHandler.Instance.EnterNextStation();

            StatTracker.Instance.NumberOfStationsVisited++;
            
            yield return DriveHandler.Instance.AnimateToStillIn(50f);
        }
    }

    private void Refuel()
    {
        DriveHandler.Instance.Engine.Coal = DriveHandler.Instance.Engine.EngineConfig.MaxCoalStorage;
        DriveHandler.Instance.Engine.Sand = DriveHandler.Instance.Engine.EngineConfig.MaxSandStorage;
    }

    void OnGUI()
    {
        GUI.enabled = StationHandler.Instance.NextStation;
        if (GUILayout.Button("Drive To Next Station"))
        {
            GUI.enabled = currentSectionType == LoopSection.Station;
            DriveToNextStation();
        }
        GUI.enabled = true;
        if (currentSectionType == LoopSection.Drive)
        {
            if (GUILayout.Button($"Shovel ({DriveHandler.Instance.Engine.Coal})"))
            {
                DriveHandler.Instance.Shovel();
            }
            
            GUILayout.Box($"{DriveHandler.Instance.TotalWeight}kg");
            float sandLeft = Mathf.Round(DriveHandler.Instance.Engine.Sand * 10f) / 10f;
            GUILayout.HorizontalSlider(DriveHandler.Instance.Acceleration, 0f,
            ((EngineWagonConfig)DriveHandler.Instance.Engine.Config).MaxAccelleration);
            GUILayout.Box($"{(int)DriveHandler.Instance.Speed} m/s" + $" - {(int)DriveHandler.Instance.DistanceLeft} meters");
            
            if (DriveHandler.Instance.DoBreak)
            {
                if (GUILayout.Button($"({sandLeft}s) Stop Break"))
                {
                    DriveHandler.Instance.Unbreak();
                }
            }
            else
            {
                if (GUILayout.Button($"({sandLeft}s) Break)"))
                {
                    DriveHandler.Instance.Break();
                }
            }
        }
    }
}

public class LoopEventHandler : Singleton<LoopEventHandler>
{
    public Event OnStationEnterEvent = new();
    public Event OnStationExitEvent = new();
    public Event<LoopSection> LoopSectionSwitchedEvent = new();
}

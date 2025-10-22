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
        
        DriveHandler.Instance.ModifySection(defaultSection);
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
            
            
            driveToNextStation = false;
            while (!driveToNextStation)
                yield return null;
            
            envHandler.BeginDriveEvent?.Invoke();
            currentSectionType = LoopSection.Drive;
            
            status = "DRIVE";
            DriveHandler.Instance.Unbreak();
            LoopEventHandler.Instance.OnStationExitEvent?.Invoke();

            reachedEnd = false;
            while (!reachedEnd)
                yield return null;
            
            envHandler.EndDriveEvent?.Invoke();
            currentSectionType = LoopSection.Station;

            StatTracker.Instance.NumberOfStationsVisited++;
            
            yield return DriveHandler.Instance.AnimateToStillIn(50f);
        }
    }

    private void Refuel()
    {
        DriveHandler.Instance.Engine.Coal = 8;
        DriveHandler.Instance.Engine.Sand = Engine.MaxSand;
    }

    void OnGUI()
    {
        if (GUILayout.Button("Drive To Next Station"))
        {
            GUI.enabled = currentSectionType == LoopSection.Station;
            DriveToNextStation();
        }

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

        if (currentSectionType == LoopSection.Station)
        {
            var engine = DriveHandler.Instance.Engine;
            
            if (!engine)
                return;
            
            //coal
            int costPerCoal = 2;
            GUI.enabled = MoneyHandler.Instance.Money >= costPerCoal;
            if (GUILayout.Button($"{costPerCoal}$ Buy Coal ({engine.Coal})"))
            {
                MoneyHandler.Instance.ChangeMoney(-costPerCoal);
                engine.Coal++;
            }
            
            //sand
            int costPerSand = 2;
            GUI.enabled = MoneyHandler.Instance.Money >= costPerSand && engine.Sand < Engine.MaxSand;
            if (GUILayout.Button($"{costPerSand}$ Buy Sand ({engine.Sand}/{Engine.MaxSand})"))
            {
                MoneyHandler.Instance.ChangeMoney(-costPerSand);
                engine.Sand++;
            }
        }
    }
}

public class LoopEventHandler : Singleton<LoopEventHandler>
{
    public Event OnStationEnterEvent = new();
    public Event OnStationExitEvent = new();
}

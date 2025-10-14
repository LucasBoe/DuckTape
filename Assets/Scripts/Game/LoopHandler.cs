using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using SS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

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
        DriveHandler.Instance.OnCurrentSectionEndReached.RemoveListener(OnCurrentSectionEndReached);
    }
    private void OnCurrentSectionEndReached()
    {
        reachedEnd = true;
    }
    public IEnumerator LoopRoutine()
    {
        Refuel();
        while (true)
        {
            driveToNextStation = false;
            while (!driveToNextStation)
                yield return null;
            
            status = "DRIVE";
            DriveHandler.Instance.Unbreak();
            currentSectionType = LoopSection.Drive;
            envHandler.BeginDrive();

            reachedEnd = false;
            while (!reachedEnd)
                yield return null;
            
            status = "STATION";
            DriveHandler.Instance.Break();
            currentSectionType = LoopSection.Station;
            envHandler.EndDrive();
            
            Refuel();
        }
    }

    private void Refuel()
    {
        DriveHandler.Instance.Engine.Coal = 8;
        DriveHandler.Instance.Engine.Sand = 6f;
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

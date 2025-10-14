using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using SS;
using UnityEngine;
using UnityEngine.Serialization;

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
        }
    }
    void OnGUI()
    {
        GUILayout.Box(status + $" - {DriveHandler.Instance.Progression}");
        if (GUILayout.Button("Drive To Next Station"))
        {
            GUI.enabled = currentSectionType == LoopSection.Station;
            DriveToNextStation();
        }

        if (currentSectionType == LoopSection.Drive)
        {
            if (GUILayout.Button("Shovel"))
            {
                DriveHandler.Instance.Shovel();
            }

            if (DriveHandler.Instance.DoBreak)
            {
                if (GUILayout.Button("Stop Break"))
                {
                    DriveHandler.Instance.Unbreak();
                }
            }
            else
            {
                if (GUILayout.Button("Break"))
                {
                    DriveHandler.Instance.Break();
                }
            }
        }
    }
}

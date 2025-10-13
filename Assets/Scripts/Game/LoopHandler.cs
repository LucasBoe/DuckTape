using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using SS;
using UnityEngine;

public enum LoopSection
{
    Station,
    Drive
}
public class LoopHandler : MonoBehaviour, IDelayedStartObserver
{
    [SerializeField] LoopSection currentSection = LoopSection.Station;
    private bool driveToNextStation = false;
    
    [SerializeField] EnvironmentHandler envHandler;
    SpeedController speedController;

    private string status = "IDLE";

    private void Awake()
    {
        speedController = new SpeedController();
    }

    [Button]
    private void DriveToNextStation()
    {
        if (currentSection != LoopSection.Station)
           return;
        
        driveToNextStation = true;
    }
    public void DelayedStart()
    {
        StartCoroutine(LoopRoutine());
    }
    public IEnumerator LoopRoutine()
    {
        while (true)
        {
            while (!driveToNextStation)
                yield return null;


            status = "BEGIN DRIVE";
            currentSection = LoopSection.Drive;
            yield return envHandler.BeginDrive();
            status = "DRIVE";
            yield return new WaitForSeconds(10f);
            status = "END DRIVE";
            yield return envHandler.EndDrive();
            status = "STATION";
            
            driveToNextStation = false;
            currentSection = LoopSection.Station;
        }
    }
    void OnGUI()
    {
        GUI.enabled = currentSection == LoopSection.Station;
        GUILayout.Box(status);
        if (GUILayout.Button("Drive To Next Station"))
            DriveToNextStation();
    }
}

public class SpeedController
{
    public float MaxSpeed = 10f;
    public float CurrentSpeed { get; private set; }
}

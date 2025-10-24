using System;
using UnityEngine;
using TMPro;

public class MaintenanceHandler : MonoBehaviour
{
    [SerializeField] private TextMeshPro sandTmp, coalTmp;

    private void OnEnable()
    {
        LoopEventHandler.Instance.OnStationEnterEvent.AddListener(OnStationEnter);
        LoopEventHandler.Instance.OnStationExitEvent.AddListener(OnStationExit);
    }


    private void OnDisable()
    {
        LoopEventHandler.Instance.OnStationEnterEvent.RemoveListener(OnStationEnter);
        LoopEventHandler.Instance.OnStationExitEvent.RemoveListener(OnStationExit);
    }

    private void OnStationEnter()
    {
        sandTmp.enabled = true;
        coalTmp.enabled = true;
    }

    private void OnStationExit()
    {
        sandTmp.enabled = false;
        coalTmp.enabled = false;
    }

    public void UpdateUI()
    {
        var eng = DriveHandler.Instance.Engine;

        var sandRounded =  Mathf.Round(eng.Sand * 10f) / 10f;

        sandTmp.text = sandRounded + "/" + eng.EngineConfig.MaxSandStorage;
        coalTmp.text = eng.Coal + "/" + eng.EngineConfig.MaxCoalStorage;
    }
}

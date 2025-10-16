using System;
using UnityEngine;

public class StationEnvironment : MonoBehaviour, IEnvironmentAsset
{
    private EnvironmentHandler handler;
    private void OnEnable() => handler.EndDriveEvent.AddListener(OnEndDrive);
    private void OnDisable() => handler.EndDriveEvent.RemoveListener(OnEndDrive);
    private void OnEndDrive()
    {
        transform.position = new Vector3(100, 0, 0);
    }
    public void Refresh(float translation)
    {
        transform.Translate(-translation, 0, 0);
    }
    public void Connect(EnvironmentHandler handler)
    {
        this.handler = handler;
    }
}
using System;
using NaughtyAttributes;
using UnityEngine;
using Event = SS.Event;

public class StationEnvironment : MonoBehaviour, IEnvironmentAsset
{
    private EnvironmentHandler handler;
    private bool isInView  = false;
    public Event OnEnter = new(), OnExit = new();
    private void OnEnable() => handler.EndDriveEvent.AddListener(OnEndDrive);
    private void OnDisable() => handler.EndDriveEvent.RemoveListener(OnEndDrive);
    private void OnEndDrive()
    {
        transform.position = new Vector3(100, 0, 0);
    }
    public void Refresh(float translation)
    {
        transform.Translate(-translation, 0, 0);
        bool shouldBeInView = Mathf.Abs(transform.position.x) < 10f;
        
        if (isInView == shouldBeInView)
            return;
        
        isInView = shouldBeInView;
        (isInView ? OnEnter : OnExit)?.Invoke();
    }
    public void Connect(EnvironmentHandler handler)
    {
        this.handler = handler;
    }
}
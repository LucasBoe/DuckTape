using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using SS;
using UnityEngine;
using UnityEngine.UIElements;
using Event = UnityEngine.Event;

[Flags]
public enum GamePhase
{
    None            = 0,
    Driving         = 1 << 0, // 1
    EnteringStation = 1 << 1, // 2
    InStation       = 1 << 2, // 4
    Map             = 1 << 3, // 8
}

public class GamePhaseHandler : Singleton<GamePhaseHandler>
{
    private GamePhase currentPhase;
    private List<GamePhase> phaseOverrides = new();
    public GamePhase CurrentPhase
    {
        get
        {
            if (phaseOverrides.Count > 0)
                return phaseOverrides.Last();
            
            return currentPhase;
        }
    }

    public Event<GamePhase> PhaseChangedEvent = new();
    public void SetGamePhase(GamePhase phase, bool revokeOverrides = false)
    {
        if (revokeOverrides)
            phaseOverrides.Clear();
        
        if (phase == CurrentPhase)
            return;
        
        currentPhase = phase;
        FireEvent();
    }
    public void AddGamePhaseOverride(GamePhase phase)
    {
        var before = currentPhase;
        phaseOverrides.Add(phase);
        
        if (before != CurrentPhase)
            FireEvent();
    }
    public void RemoveGamePhaseOverride(GamePhase phase)
    {
        var before = currentPhase;
        phaseOverrides.Remove(phase);
        
        if (before != CurrentPhase)
            FireEvent();
    }
    private void FireEvent()
    {
        Debug.Log($"Notify Phase Changed: {CurrentPhase}");
        PhaseChangedEvent?.Invoke(CurrentPhase);
    }
}

public abstract class GamePhaseUI : MonoBehaviour
{
    [SerializeField, EnumFlags] protected GamePhase associatedPhases;
    [ShowNativeProperty] protected GamePhase currentPhase => GamePhaseHandler.InstanceExists ? GamePhaseHandler.Instance.CurrentPhase : GamePhase.None;
    [SerializeField, ReadOnly] protected bool isVisible = true;
    protected virtual void OnEnable()
    {
        GamePhaseHandler.Instance.PhaseChangedEvent.AddListener(OnPhaseChanged);
    }
    protected virtual void OnDisable()
    {
        GamePhaseHandler.Instance.PhaseChangedEvent.RemoveListener(OnPhaseChanged);
    }
    private void OnPhaseChanged(GamePhase phase)
    {
        bool shouldBeVisible = associatedPhases.HasFlag(phase);
        Debug.Log($"Phase Changed: {phase}, should be {shouldBeVisible} ({associatedPhases})");
        if (isVisible == shouldBeVisible)
            return;
        
        isVisible = shouldBeVisible;
        SetVisible(isVisible);
    }
    protected abstract void SetVisible(bool visible);
}
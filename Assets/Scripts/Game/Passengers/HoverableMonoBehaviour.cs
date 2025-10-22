using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using Event = SS.Event;

public class HoverableMonoBehaviourParent : MonoBehaviour
{
    
}
public class HoverableMonoBehaviour : HoverableMonoBehaviourParent
{
    public Event DestroyEvent = new();
    private void OnMouseEnter()
    {
        HoverHandler.Instance.TryHover(this);
    }

    void OnMouseExit()
    {
        HoverHandler.Instance.TryEndHover(this);
    }

    private void OnDestroy()
    {
        DestroyEvent?.Invoke();
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class HoverUINaked : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public virtual bool CheckSourceTypeMatch(HoverableMonoBehaviour behaviour)
    {
        return false;
    }
    public abstract HoverUINaked CreateInstance(HoverableMonoBehaviour behaviour);
    public bool IsHovered { get; private set; }
    public SS.Event HoverExitEvent = new();
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHovered = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        IsHovered = false;
        HoverExitEvent?.Invoke();
    }
}
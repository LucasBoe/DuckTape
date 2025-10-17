using SS;

[SingletonSettings(_lifetime: SingletonLifetime.Scene, _canBeGenerated: true, _eager: true)]
public class HoverHandler : Singleton<HoverHandler>
{
    private HoverableMonoBehaviour currentlyHovered;
    public Event<HoverableMonoBehaviour> BeginHoverEvent = new(), EndHoverEvent = new(), HoverChangedEvent = new();
    public void TryHover(HoverableMonoBehaviour hoverable)
    {
        if (currentlyHovered == hoverable)
            return;

        ChangeHover(hoverable);
    }
    public void TryEndHover(HoverableMonoBehaviour hoverable)
    {
        if (currentlyHovered != hoverable)
            return;
        
        ChangeHover(null);
    }
    private void ChangeHover(HoverableMonoBehaviour hover)
    {
        if (currentlyHovered != null)
            EndHoverEvent?.Invoke(currentlyHovered);
        
        currentlyHovered = hover;
        
        if (currentlyHovered != null)
            BeginHoverEvent?.Invoke(currentlyHovered);
        
        HoverChangedEvent?.Invoke(currentlyHovered);
    }
}
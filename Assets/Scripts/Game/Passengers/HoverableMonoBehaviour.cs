using UnityEngine;

public class HoverableMonoBehaviourParent : MonoBehaviour
{
    
}
public class HoverableMonoBehaviour : HoverableMonoBehaviourParent
{
    private void OnMouseEnter()
    {
        HoverHandler.Instance.TryHover(this);
    }

    void OnMouseExit()
    {
        HoverHandler.Instance.TryEndHover(this);
    }
}
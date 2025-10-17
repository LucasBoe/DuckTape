using UnityEngine;

public abstract class HoverUINaked : MonoBehaviour
{
    public virtual bool CheckSourceTypeMatch(HoverableMonoBehaviour behaviour)
    {
        return false;
    }
    public abstract HoverUINaked CreateInstance(HoverableMonoBehaviour behaviour);
}
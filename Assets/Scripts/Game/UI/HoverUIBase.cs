using NaughtyAttributes;
using UnityEngine;

public abstract class HoverUIBase<T> : HoverUINaked where T : HoverableMonoBehaviour
{
    [SerializeField] private bool followTarget;
    [SerializeField, ShowIf("followTarget")] private Vector2Int targetOffset;
    public T Source;
    public override bool CheckSourceTypeMatch(HoverableMonoBehaviour behaviour)
    {
        return behaviour is T;
    }
    protected abstract void Populate();
    protected virtual void Update()
    {
        if (followTarget)
            transform.position = Camera.main.WorldToScreenPoint(Source.transform.position) + new Vector3(targetOffset.x, targetOffset.y, 0);
    }
    public override HoverUINaked CreateInstance(HoverableMonoBehaviour behaviour)
    {
        T typedBehaviour = behaviour as T;
        var instance = Instantiate(this, transform.parent);
        instance.Source = typedBehaviour;
        instance.Populate();
        return instance;
    }
}
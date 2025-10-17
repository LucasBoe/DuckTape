using UnityEngine;

public abstract class HoverUIBase<T> : HoverUINaked where T : HoverableMonoBehaviour
{
    [SerializeField] private bool followTarget;
    public T Source;
    public override bool CheckSourceTypeMatch(HoverableMonoBehaviour behaviour)
    {
        return behaviour is T;
    }
    protected abstract void Populate();
    protected virtual void Update()
    {
        if (followTarget)
            transform.position = Camera.main.WorldToScreenPoint(Source.transform.position);
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
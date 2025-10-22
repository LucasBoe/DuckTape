using UnityEngine;

public class WagonShopWagon : HoverableMonoBehaviour
{
    [SerializeField] private BoxCollider2D collider;
    public WagonConfigBase Config;
    public void Apply(WagonConfigBase wagon)
    {
        var child = Instantiate(wagon.Prefab, transform);
        DestroyAll<CargoSlot>(child);
        DestroyAll<Collider2D>(child);
        DestroyAll<TrainWagonBase>(child);
        collider.size = new Vector2(wagon.Length, 1);
        collider.offset = new Vector2(-wagon.Length / 2f, .5f);
        
        Config = wagon;

    }
    private void DestroyAll<T>(TrainWagonBase root) where T : Component
    {
        foreach (var script in root.GetComponentsInChildren<T>())
        {
            /*
            if (script.gameObject == root.gameObject)
                continue;
            */
            Destroy(script);
        }
    }
}

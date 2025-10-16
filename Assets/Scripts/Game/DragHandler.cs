using SS;
using UnityEngine;

public class DragHandler : SingletonBehaviour<DragHandler>
{
    [SerializeField] private GameObject pickUpEffect, dropEffect;
    [SerializeField] private CargoConfigBase currentCargoConfig;
    [SerializeField] private CargoSlot currentCargoSlot;
    private GameObject dragVis;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) & !currentCargoConfig)
            OnMouseDown();

        if(Input.GetMouseButtonUp(0))
            OnMouseUp();

        if(currentCargoConfig && dragVis)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragVis.transform.position = mousePos;
        }
    }

    private void OnMouseDown()
    {
        if (RaycastMouseForCargoSlot(out var slot))
        {
            if (!slot.ContainsCargo)
                return;

            currentCargoSlot = slot;
            currentCargoConfig = slot.CargoInstance.Config;

            //Do pickup Effect
            PlayEffect(pickUpEffect, currentCargoSlot);

            //Destroys old cargo
            Destroy(currentCargoSlot.CargoInstance.gameObject);

            //create Drag Visualization
            dragVis = new GameObject();
            dragVis.AddComponent<SpriteRenderer>().sprite = currentCargoConfig.Sprite;

        }
    }
    private void OnMouseUp()
    {
        if (!currentCargoConfig)
            return;
        
        // Check if the ray hit something
        if (RaycastMouseForCargoSlot(out var targetCargo) && !targetCargo.ContainsCargo)
        {
            CargoSpawner.Instance.SpawnAtSlot(currentCargoConfig, targetCargo);
            PlayEffect(dropEffect, targetCargo);
        }
        else
        {
            CargoSpawner.Instance.SpawnAtSlot(currentCargoConfig, currentCargoSlot);
            PlayEffect(dropEffect, currentCargoSlot);
        }

        currentCargoConfig = null;
        currentCargoSlot = null;

        Destroy(dragVis);
    }

    private bool RaycastMouseForCargoSlot(out CargoSlot slot)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            slot = hit.collider.gameObject.GetComponent<CargoSlot>();
            return slot;
        }

        slot = null;
        return false;
    }
    private void PlayEffect(GameObject effectObject, CargoSlot targetSlot)
    {
        Instantiate(effectObject, targetSlot.transform.position, Quaternion.identity);
    }
}
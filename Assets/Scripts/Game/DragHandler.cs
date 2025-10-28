using NaughtyAttributes;
using SS;
using UnityEngine;
using UnityEngine.Serialization;

[SingletonSettings(SingletonLifetime.Scene, _canBeGenerated: false, _eager: true)]
public class DragHandler : SingletonBehaviour<DragHandler>
{
    [SerializeField] private GameObject pickUpEffect, dropEffect;
    [SerializeField, ReadOnly] private Cargo cargoInHand;
    [SerializeField, ReadOnly] private CargoSlot lastCargoSlot;
    [SerializeField, ReadOnly] private Transform dragRoot;
    public bool IsDragging => cargoInHand != null;
    public CargoConfigBase CurrentCargo => cargoInHand?.Config;
    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && cargoInHand)
            OnMouseUp();
        
        if(Input.GetMouseButtonDown(0) & !cargoInHand)
            OnMouseDown();

        if(cargoInHand && dragRoot)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragRoot.transform.position = mousePos;
        }
    }

    private void OnMouseDown()
    {
        if (RaycastMouseForCargoSlot(out var slot))
        {
            if (!slot.ContainsCargo)
                return;

            lastCargoSlot = slot;

            //Do pickup Effect
            PlayEffect(pickUpEffect, lastCargoSlot);

            //create Drag Visualization
            dragRoot = new GameObject("DRAG").transform;
            cargoInHand = lastCargoSlot.ExtractCargo();
            cargoInHand.transform.SetParent(dragRoot.transform);
            cargoInHand.transform.localPosition = Vector3.zero;
        }
    }
    private void OnMouseUp()
    {
        // Check if the ray hit something
        if (RaycastMouseForCargoSlot(out var targetSlot) && targetSlot.TryAssign(cargoInHand))
        {
            if (targetSlot is CargoSell)
            {
                //play sell effect
            }
            else
            {
                PlayEffect(dropEffect, targetSlot);
            }
        }
        else
        {
            lastCargoSlot.TryAssign(cargoInHand);
            PlayEffect(dropEffect, lastCargoSlot);
        }

        cargoInHand = null;
        lastCargoSlot = null;

        Destroy(dragRoot.gameObject);
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

    public bool CheckIsDraggingSellable()
    {
        if (!IsDragging)
            return false;

        return cargoInHand.OriginStationID < StatTracker.Instance.NumberOfStationsVisited;
    }
}
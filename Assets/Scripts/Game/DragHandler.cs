using SS;
using UnityEngine;

public class DragHandler : SingletonBehaviour<DragHandler>
{
    [SerializeField] private GameObject pickUpEffect, dropEffect;

    [SerializeField]private CargoConfigBase currentCargoConfig;
    [SerializeField]private CargoSlot currentCargoSlot;
    private GameObject dragVis;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) & !currentCargoConfig)
            OnMouseDown();

        if(Input.GetMouseButtonUp(0))
            OnMouseUp();

        if(currentCargoConfig && dragVis)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // For 2D, set Z to 0 so it stays in the same plane
            mousePos.z = 0;

            // Move this GameObject to that position
            dragVis.transform.position = mousePos;

        }
    }

    private void OnMouseDown()
    {
        // Convert mouse position to world coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<CargoSlot>())
            {
                currentCargoSlot = hit.collider.gameObject.GetComponent<CargoSlot>();

                if (!currentCargoSlot.CargoInstance)
                    return;

                currentCargoConfig = currentCargoSlot.CargoInstance.CargoConfig;

                if (!currentCargoConfig)
                {
                    currentCargoSlot = null;
                    return;
                }

                //Do pickup Effect
                Instantiate(pickUpEffect, currentCargoSlot.transform.position, Quaternion.identity);

                //Destroys old cargo
                Destroy(hit.collider.gameObject.GetComponent<CargoSlot>().CargoInstance.gameObject);

                //create Drag Visualization
                dragVis = new GameObject();
                dragVis.AddComponent<SpriteRenderer>().sprite = currentCargoConfig.Sprite;
            }
        }

    }

    private void OnMouseUp()
    {
        if (!currentCargoConfig)
            return;

        // Convert mouse position to world coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<CargoSlot>())
            {
                CargoSlot targetCargo = hit.collider.gameObject.GetComponent<CargoSlot>();

                if (targetCargo)
                {
                    if(targetCargo.CargoInstance != null)
                    {
                        CargoSpawner.Instance.SpawnAtSlot(currentCargoConfig, currentCargoSlot);

                        //Spawn effect of placing in slot
                        Instantiate(dropEffect, currentCargoSlot.transform.position, Quaternion.identity);
                    }

                    else
                    {
                        //Spawn new Cargo at Slot
                        CargoSpawner.Instance.SpawnAtSlot(currentCargoConfig, targetCargo);

                        //Spawn effect of placing in slot
                        Instantiate(dropEffect, targetCargo.transform.position, Quaternion.identity);
                    }

                }
            }
        }

        else
        {
            CargoSpawner.Instance.SpawnAtSlot(currentCargoConfig, currentCargoSlot);

            //Spawn effect of placing in slot
            Instantiate(dropEffect, currentCargoSlot.transform.position, Quaternion.identity);
        }

        currentCargoConfig = null;
        currentCargoSlot = null;

        Destroy(dragVis);

    }

}

using SS;
using UnityEngine;

public class DragHandler : SingletonBehaviour<DragHandler>
{
    [SerializeField] private CargoConfigBase currentCargoConfig;

    private CargoSlot currentCargoSlot;
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
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Fire a ray from the camera toward the mouse position
        Vector2 rayOrigin = Camera.main.transform.position;
        Vector2 direction = (mousePos - rayOrigin).normalized;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 10f);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            if(hit.collider.gameObject.GetComponent<CargoSlot>())
            {
                currentCargoSlot = hit.collider.gameObject.GetComponent<CargoSlot>();
                currentCargoConfig = currentCargoSlot.CargoInstance.CargoConfig;

                if (!currentCargoConfig)
                    return;

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

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Fire a ray from the camera toward the mouse position
        Vector2 rayOrigin = Camera.main.transform.position;
        Vector2 direction = (mousePos - rayOrigin).normalized;

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 10f);

        // Check if the ray hit something
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.GetComponent<CargoSlot>())
            {
                CargoSlot targetCargo = hit.collider.gameObject.GetComponent<CargoSlot>();

                if (targetCargo)
                {
                    //Spawn new Cargo at Slot
                    CargoSpawner.Instance.SpawnAtSlot(currentCargoConfig, targetCargo);
                }
            }
        }

        else
        {
            CargoSpawner.Instance.SpawnAtSlot(currentCargoConfig, currentCargoSlot);
        }

        currentCargoConfig = null;
        currentCargoSlot = null;

        Destroy(dragVis);

    }

}

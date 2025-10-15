using SS;
using UnityEngine;

public class DragHandler : SingletonBehaviour<DragHandler>
{
    [SerializeField] private Cargo currentCargo;

    private GameObject dragVis;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) & !currentCargo)
            OnMouseDown();

        if(Input.GetMouseButtonUp(0))
            OnMouseUp();

        if(!currentCargo)
        {
            //Drag Visual Logic
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
                currentCargo = hit.collider.gameObject.GetComponent<CargoSlot>().CargoInstance;
                print(currentCargo + "");


                //create Drag Visualization
                dragVis = new GameObject();
            }
        }

    }

    private void OnMouseUp()
    {
        if (!currentCargo)
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

                if (!targetCargo)
                {
                    //Spawn Cargo in Cargo Slot
                    //Destroy Old Cargo
                }

                //Destroy current Dragable Visualisation
                
            }
        }
    }

}

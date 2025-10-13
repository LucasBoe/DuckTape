using SS;
using UnityEngine;

public class DragHandler : SingletonBehaviour<DragHandler>
{
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
                CargoConfigBase currentCargo = hit.collider.gameObject.GetComponent<CargoSlot>().CargoInstance.Config;
                //create Drag Visualization
            }
        }

    }

    private void OnMouseUp()
    {
        
    }
}

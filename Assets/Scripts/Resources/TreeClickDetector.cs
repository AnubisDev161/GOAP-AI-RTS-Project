using UnityEngine;

public class TreeClickDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent<IClickable>(out IClickable clickable))
                {
                    clickable.OnClick();
                }

            }
        }
    }

   
}
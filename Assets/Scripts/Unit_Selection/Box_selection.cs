using UnityEngine;

public class Box_selection: MonoBehaviour
{

    private LineRenderer line_renderer;
    private Vector2 initial_mouse_position, current_mouse_position;
    private BoxCollider2D box_collider;


    void Start()
    {
        line_renderer = GetComponent<LineRenderer>();
        line_renderer.positionCount = 0;
    }

   // void Update()
   // {
   //     if (Input.GetMouseButtonDown(0) && ! Knight.mouse_over)
    //    {
     //     FindObjectOfType<>
     //   }
   // }
}

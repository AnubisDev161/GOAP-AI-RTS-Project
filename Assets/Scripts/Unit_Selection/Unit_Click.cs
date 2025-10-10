using UnityEngine;
using UnityEngine.InputSystem;

public class Unit_Click : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera player_camera;
    public LayerMask clickable;
    public LayerMask ground;
    private PlayerInput player_input;
    
    private void Awake()
    {
        player_input = GetComponent<PlayerInput>();
    }
    void Start()
    {
        player_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (hit.collider.gameObject.TryGetComponent<ISelectable_Character>(out ISelectable_Character character) && character.isSelectable())
                {

                    // if player hit a clickable object
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        // shift clicked
                        Unit_Selections.instance.shift_click_select(hit.collider.gameObject);
                    }
                    else
                    {   // normal click
                        Unit_Selections.instance.click_select(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                // if player didn't && was shift clicking
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    Unit_Selections.instance.deselect_all();
                }
            }
        }
    }
}

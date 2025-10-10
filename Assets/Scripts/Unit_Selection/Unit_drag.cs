using UnityEngine;

public class Unit_drag : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Camera player_cam;

    [SerializeField]
    RectTransform box_visual;

    Rect selection_box;

    Vector2 start_position, end_position;
    void Start()
    {
        player_cam = Camera.main;
        start_position = Vector2.zero;
        end_position = Vector2.zero;
        draw_visual();
    }

    // Update is called once per frame
    void Update()
    {
        //when clicked
        if (Input.GetMouseButtonDown(0))
        {
            start_position = Input.mousePosition;
            selection_box = new Rect();
        }

        //when dragging
        if (Input.GetMouseButton(0))
        {
            end_position= Input.mousePosition;
            draw_visual();
            draw_selection();
        }

        // when release click
        if (Input.GetMouseButtonUp(0))
        {
            select_units();
            start_position = Vector2.zero;
            end_position = Vector2.zero;
            draw_visual();

        }
    }
    private void draw_visual()
    {
        Vector2 box_start = start_position;
        Vector2 box_end = end_position;

        Vector2 box_centre = (box_start + box_end) /2;
        box_visual.position = box_centre;

        Vector2 box_size = new Vector2(Mathf.Abs(box_start.x - box_end.x), Mathf.Abs(box_start.y - box_end.y));

        box_visual.sizeDelta = box_size;
    }

    private void draw_selection()
    {
        // do x calculations
        if (Input.mousePosition.x < start_position.x)
        {
            //dragging left
            selection_box.xMin = Input.mousePosition.x;
            selection_box.xMax = start_position.x;
        }
        else
        {
            //dragging right
            selection_box.xMin = start_position.x;
            selection_box.xMax = Input.mousePosition.x;
        }

        // do y calculations
        if (Input.mousePosition.y < start_position.y) 
        {
            //dragging down
            selection_box.yMin = Input.mousePosition.y;
            selection_box.yMax = start_position.y;
        }
        else
        {
            //dragging up
            selection_box.yMin = start_position.y;
            selection_box.yMax = Input.mousePosition.y;
        }
    }
    private void select_units()
    {
        // loop through all units
        for (int character = 0; character < Unit_Selections.instance.unit_list.Count; character++) {

            if (Unit_Selections.instance.unit_list[character].TryGetComponent<ISelectable_Character>(out ISelectable_Character selectableCharacter) && selectableCharacter.isSelectable())
            {
                if (selection_box.Contains(player_cam.WorldToScreenPoint(Unit_Selections.instance.unit_list[character].transform.position)))
                {
                    // if any unit is within the selection box it will add it to the selected unit list
                    Unit_Selections.instance.drag_select(Unit_Selections.instance.unit_list[character]);
                }
            }
        }
    }
}

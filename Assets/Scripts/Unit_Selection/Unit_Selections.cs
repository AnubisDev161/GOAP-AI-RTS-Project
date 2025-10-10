using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class Unit_Selections : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> unit_list = new List<GameObject>();
    public List<GameObject> characters_selected = new List<GameObject>();
    private static Unit_Selections _instance;
    public static Unit_Selections instance {  get { return _instance; } }
    private InputSystem_Actions input_actions;
    private void Awake()
    {
        input_actions = new InputSystem_Actions();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else { 
            _instance = this;
        }
    }

    private void OnEnable()
    {
        input_actions.Player.Enable();
    }

    private void OnDisable()
    {
        input_actions.Player.Disable();

    }
   
    public void click_select(GameObject character_to_add)
    {
        deselect_all();
        characters_selected.Add(character_to_add);
        character_to_add.GetComponent<ISelectable_Character>().select();
    }

    public void shift_click_select(GameObject character_to_add)
    {
        if (!characters_selected.Contains(character_to_add)){
            characters_selected.Add(character_to_add);
            character_to_add.GetComponent<ISelectable_Character>().select();
        }
        else
        {
            character_to_add.GetComponent<ISelectable_Character>().deselect();
            characters_selected.Remove(character_to_add);
        }
    }
    public void drag_select(GameObject character_to_add)
    {
        if (!characters_selected.Contains(character_to_add)) {
            characters_selected.Add (character_to_add);
            character_to_add.GetComponent<ISelectable_Character>().select();

        }
    }
    public void deselect_all()
    {
        for (int character = 0; character < characters_selected.Count; character++)
        {
            if (characters_selected[character].gameObject != null)
            {
                characters_selected[character].GetComponent<ISelectable_Character>().deselect();
            }
            else
            {
                characters_selected.Remove(characters_selected[character]);
            }
        }
        characters_selected.Clear();
    }
    public void deselect_unit(GameObject unit_to_deselect)
    {
        characters_selected.Remove (unit_to_deselect);
    }
    
}

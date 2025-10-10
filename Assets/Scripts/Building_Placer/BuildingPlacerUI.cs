using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacerUI : MonoBehaviour
{
    [SerializeField] List<Button> building_buttons;

    [SerializeField] Button open_building_placer_button;

    [SerializeField] Button close_building_placer_button;
    public void ShowBuildingButtons(bool show)
    {
        foreach (var button in building_buttons)
        {
            button.gameObject.SetActive(show);
        }

    }

    public void ShowOpenBuildingPlacerButton(bool show)
    {
        open_building_placer_button.gameObject.SetActive(show);
        close_building_placer_button.gameObject.SetActive(!show);
    }

}

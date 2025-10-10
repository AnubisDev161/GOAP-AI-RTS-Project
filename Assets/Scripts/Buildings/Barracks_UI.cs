using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Barracks_UI : MonoBehaviour
{
    List<Button> recruitingButtons;
    [SerializeField] GameObject recruitingButtonsContainer;
    public void ShowButtons(bool show)
    {   
        recruitingButtonsContainer.gameObject.SetActive(show);
    }
}

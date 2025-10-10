using UnityEngine;

public class Selectable_object: MonoBehaviour
{
    [SerializeField] private GameObject selectionIndicator;

    public void SetSelected(bool selected)
    {
        if (selectionIndicator != null)
        {

            print("unselected!");
            selectionIndicator.SetActive(selected);

        }

       
        // Add any other selection logic here
    }
}

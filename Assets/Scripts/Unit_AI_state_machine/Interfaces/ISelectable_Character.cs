using UnityEngine;
using UnityEngine.EventSystems;

public interface ISelectable_Character
{
    void select();

    void deselect();

    bool isSelectable();
}

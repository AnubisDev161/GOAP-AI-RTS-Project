using UnityEngine;

public interface IClickable
{
    void OnClick();
    void OnDeselect();

    GameObject get_parent();

}

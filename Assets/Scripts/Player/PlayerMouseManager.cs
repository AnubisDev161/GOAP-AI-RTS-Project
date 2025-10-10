using UnityEngine;

public class PlayerMouseManager : MonoBehaviour
{
    [SerializeField] LayerMask clickLayer;
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckMouseHitClickableObject(hit);
            }
        }
    }

    public void CheckMouseHitClickableObject(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<IClickable>(out IClickable clickableComponent))
        {
            clickableComponent.OnClick();
        }
    }
}

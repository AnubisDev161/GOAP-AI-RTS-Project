using UnityEngine;
using UnityEngine.EventSystems;
public class Selection_rectangle: MonoBehaviour
{
    private Vector3 startPosition;
    private bool isSelecting = false;
    private Rect selectionRect;

    public Texture2D selectionHighlight;
    public Color selectionColor = new Color(0.8f, 0.8f, 0.95f, 0.25f);
    public Color borderColor = new Color(0.8f, 0.8f, 0.95f);

    void Update()
    {
        // Start selection
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()) // Ignore UI
            {
                isSelecting = true;
                startPosition = Input.mousePosition;
                selectionRect = new Rect();
            }
        }

        // Update selection
        if (Input.GetMouseButton(0) && isSelecting)
        {
            UpdateSelectionRect();
        }

        // End selection
        if (Input.GetMouseButtonUp(0) && isSelecting)
        {
            isSelecting = false;
            SelectObjectsInRectangle();
        }
    }

    void UpdateSelectionRect()
    {
        // Calculate rectangle coordinates
        Vector3 currentPosition = Input.mousePosition;

        selectionRect.xMin = Mathf.Min(startPosition.x, currentPosition.x);
        selectionRect.xMax = Mathf.Max(startPosition.x, currentPosition.x);
        selectionRect.yMin = Mathf.Min(Screen.height - startPosition.y, Screen.height - currentPosition.y);
        selectionRect.yMax = Mathf.Max(Screen.height - startPosition.y, Screen.height - currentPosition.y);
    }

    void SelectObjectsInRectangle()
    {
        // Convert rectangle to world space
        var camera = Camera.main;
        var min = camera.ScreenToWorldPoint(new Vector3(selectionRect.xMin, Screen.height - selectionRect.yMax, 0));
        var max = camera.ScreenToWorldPoint(new Vector3(selectionRect.xMax, Screen.height - selectionRect.yMin, 0));
        var selectionBounds = new Bounds();
        selectionBounds.SetMinMax(min, max);

        // Find all selectable objects in scene
        var selectables = FindObjectsOfType<Selectable_object>();


        // Clear previous selection
        foreach (var selectable in selectables)
        {
            selectable.SetSelected(false);
        }

        // Select objects within bounds
        foreach (var selectable in selectables)
        {
            var screenPos = camera.WorldToScreenPoint(selectable.transform.position);
            screenPos.y = Screen.height - screenPos.y; // Convert to GUI coordinates

            if (selectionRect.Contains(screenPos))
            {
                selectable.SetSelected(true);
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Draw the selection rectangle
            if (selectionHighlight != null)
            {
                GUI.DrawTexture(selectionRect, selectionHighlight);
            }
            else
            {
                // Draw filled rectangle
                GUI.color = selectionColor;
                GUI.DrawTexture(selectionRect, Texture2D.whiteTexture);

                // Draw border
                GUI.color = borderColor;
                GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, selectionRect.width, 1), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y, 1, selectionRect.height), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(selectionRect.x + selectionRect.width, selectionRect.y, 1, selectionRect.height), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(selectionRect.x, selectionRect.y + selectionRect.height, selectionRect.width, 1), Texture2D.whiteTexture);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursorManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Texture2D cursorTexture;  // Curseur personnalis�
    public Texture2D defaultTexture; // Curseur par d�faut
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private static bool isHoldingObject = false; // V�rifie si un objet est "tenu"

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHoldingObject)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isHoldingObject)
        {
            Cursor.SetCursor(defaultTexture, Vector2.zero, cursorMode);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isHoldingObject = !isHoldingObject; // Inverse l'�tat (toggle)
        Cursor.SetCursor(isHoldingObject ? cursorTexture : defaultTexture, hotSpot, cursorMode);
    }
}

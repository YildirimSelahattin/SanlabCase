using UnityEngine;

public interface IMouseDragHandler
{
    void OnMouseDown(Parts part, Vector3 mousePosition);
    void OnMouseDrag(Parts part, Vector3 mousePosition);
    void OnMouseUp(Parts part, Vector3 mousePosition);
}


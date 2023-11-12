using UnityEngine;

public class MouseDragHandler
{
    private float mouseZ;
    private Vector3 mOffSet;

    public void OnMouseDown(Parts part)
    {
        mouseZ = Camera.main.WorldToScreenPoint(part.gameObject.transform.position).z;
        mOffSet = part.gameObject.transform.position - GetMouseWorldPos(Input.mousePosition);

        if (part.transform.parent == part.targetTransform)
        {
            part.transform.parent = part.StartParent;
            CouplingManager.Instance.PlacedPartCount--;
            part.inSide = false;
        }
    }

    public void OnMouseDrag(Parts part)
    {
        part.transform.position = GetMouseWorldPos(Input.mousePosition) + mOffSet;
    }

    public void OnMouseUp(Parts part)
    {
        if (part.inSide)
        {
            foreach (var nextPart in part.nextEnabledParts)
            {
                Parts partsComponent = nextPart.GetComponent<Parts>();
                if (partsComponent != null)
                {
                    partsComponent.insertable = true;
                }
                RodBoltController rodBoltComponent = nextPart.GetComponent<RodBoltController>();
                if (rodBoltComponent != null)
                {
                    rodBoltComponent.insertable = true;
                }
            }
            part.targetTransform.GetComponent<BoxCollider>().enabled = false;
            part.MoveTargetWithAnimation();
            part.targetTransform.GetChild(0).gameObject.SetActive(false);
            part.inSide = false;
            CouplingManager.Instance.PlacedPartCount++;
        }
        else
        {
            part.targetTransform.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private Vector3 GetMouseWorldPos(Vector3 mousePosition)
    {
        mousePosition.z = mouseZ;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
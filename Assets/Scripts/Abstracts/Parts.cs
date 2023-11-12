using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Parts : MonoBehaviour
{
    public Transform targetTransform;
    private bool inSide = false;
    private Vector3 mOffSet;
    private float mouseZ;
    private Transform startParent;
    private Vector3 startPos;
    [SerializeField] private bool insertable;
    public List<GameObject> nextEnabledParts;
    public Transform StartParent => startParent;

    private void Start()
    {
        CouplingManager.Instance.AssemblyStarted += OnMontageStarted;
        SetBaseTransform();
    }

    void SetBaseTransform()
    {
        startParent = transform.parent;
        startPos = transform.position;
    }

    private void OnMouseDown()
    {
        mouseZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffSet = gameObject.transform.position - GetMouseWorldPos();

        if (transform.parent == targetTransform)
        {
            transform.parent = startParent;
            CouplingManager.Instance.PlacedPartCount--;
            inSide = false;
        }
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffSet;
    }

    private void OnMouseUp()
    {
        if (inSide)
        {
            foreach (var nextPart in nextEnabledParts)
            {
                Parts partsComponent = nextPart.GetComponent<Parts>();
                if (partsComponent != null)
                {
                    partsComponent.insertable = true;
                }
                RodBoltController RrodBoltComponent = nextPart.GetComponent<RodBoltController>();
                if (RrodBoltComponent != null)
                {
                    RrodBoltComponent.insertable = true;
                }
            }
            targetTransform.GetComponent<BoxCollider>().enabled = false;
            MoveTargetWithAnimation();
            targetTransform.GetChild(0).gameObject.SetActive(false);
            inSide = false;
            CouplingManager.Instance.PlacedPartCount++;
        }
        else
        {
            targetTransform.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZ;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public virtual void MoveTargetWithAnimation()
    {
        transform.SetParent(targetTransform);
        transform.DOLocalMove(Vector3.zero, 2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == targetTransform.name && transform.parent != targetTransform)
        {
            if (insertable == true)
            {
                other.transform.GetChild(0).gameObject.SetActive(true);
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CouplingManager.Instance.correctMat;
                inSide = true;
            }
            else
            {
                other.transform.GetChild(0).gameObject.SetActive(true);
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CouplingManager.Instance.incorrectMat;
                inSide = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == targetTransform.name)
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CouplingManager.Instance.correctMat;
            inSide = false;
        }
    }

    private void OnMontageStarted()
    {
        transform.SetParent(startParent);
        transform.position = startPos;
    }
}

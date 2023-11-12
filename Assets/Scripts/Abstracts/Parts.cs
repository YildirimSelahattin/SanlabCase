using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Parts : MonoBehaviour
{
    private MouseDragHandler mouseDragHandler;
    public Transform targetTransform;
    public bool insertable;
    public List<GameObject> nextEnabledParts;
    public bool inSide = false;
    private Transform startParent;
    public Transform StartParent => startParent;
    public Vector3 startPos;

    private void Start()
    {
        mouseDragHandler = new MouseDragHandler();
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
        mouseDragHandler.OnMouseDown(this);
    }

    private void OnMouseDrag()
    {
        mouseDragHandler.OnMouseDrag(this);
    }

    private void OnMouseUp()
    {
        mouseDragHandler.OnMouseUp(this);
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

    public virtual void MoveTargetWithAnimation()
    {
        transform.SetParent(targetTransform);
        transform.DOLocalMove(Vector3.zero, 2f);
    }

    private void OnMontageStarted()
    {
        transform.SetParent(startParent);
        transform.position = startPos;
    }
}

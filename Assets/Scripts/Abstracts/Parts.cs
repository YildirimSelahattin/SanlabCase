using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Parts : MonoBehaviour
{
    [SerializeField]
    private float rotateSensivity = .5f;
    public Transform targetTransform;

    [SerializeField] private int montageNumber;
    private Vector2 turn;
    private bool inSide = false;
    private Vector3 mOffSet;
    private float mZCoord;
    private Transform startParent;
    private Vector3 startPos;
    public Transform StartParent => startParent;

    private void Awake()
    {
        CouplingManager.Instance.MontageStarted += OnMontageStarted;
    }

    private void Start()
    {
        startParent = transform.parent;
        startPos = transform.position;
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffSet = gameObject.transform.position - GetMouseWorldPos();
        if (transform.parent == targetTransform)
        {

            if (montageNumber % 10 == 0 && CouplingManager.Instance.LastBMontagePartsCount > montageNumber)
            {
                CouplingManager.Instance.LastBMontagePartsCount = montageNumber - 10;
                Debug.Log("%10'a");
            }
            else if (CouplingManager.Instance.LastAMontagePartsCount > montageNumber)
            {
                CouplingManager.Instance.LastAMontagePartsCount = montageNumber - 1;
            }
            transform.parent = startParent;
            CouplingManager.Instance.CurrentMontageCount--;
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
            targetTransform.GetComponent<BoxCollider>().enabled = false;
            MoveTargetWithAnimation();
            targetTransform.GetChild(0).gameObject.SetActive(false);
            inSide = false;
            CouplingManager.Instance.CurrentMontageCount++;
        }
        else
        {
            targetTransform.GetComponent<BoxCollider>().enabled = true;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void RotateByMouse()
    {
        turn.x += Input.GetAxis("Mouse X") * rotateSensivity;
        turn.y += Input.GetAxis("Mouse Y") * rotateSensivity;
        transform.localRotation = Quaternion.Euler(turn.y, -turn.x, 0);
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
            if (montageNumber % 10 == 0)
            {
                if (montageNumber <= CouplingManager.Instance.LastBMontagePartsCount || montageNumber == CouplingManager.Instance.LastBMontagePartsCount + 10)
                {
                    other.transform.GetChild(0).gameObject.SetActive(true);
                    other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CouplingManager.Instance.correctMat;
                    CouplingManager.Instance.LastBMontagePartsCount = montageNumber;
                    inSide = true;
                }
                else
                {
                    other.transform.GetChild(0).gameObject.SetActive(true);
                    other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CouplingManager.Instance.incorrectMat;
                    inSide = false;
                }
            }
            else
            {
                if (montageNumber <= CouplingManager.Instance.LastAMontagePartsCount || montageNumber == CouplingManager.Instance.LastAMontagePartsCount + 1)
                {
                    other.transform.GetChild(0).gameObject.SetActive(true);
                    other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CouplingManager.Instance.correctMat;
                    CouplingManager.Instance.LastAMontagePartsCount = montageNumber;
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == targetTransform.name)
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material =  CouplingManager.Instance.correctMat;
            inSide = false;
        }
    }

    private void OnMontageStarted()
    {
        transform.SetParent(startParent);
        transform.position = startPos;
    }
}

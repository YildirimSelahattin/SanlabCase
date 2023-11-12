using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensivity = 1f;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float distanceFromTarget = 5.0f;
    [SerializeField]
    private float zoom;
    private Camera cam;
    private float rotationY;
    private float rotationX;
    private Vector3 startPos;
    private Quaternion startRot;

    private bool isRotateActive = false;

    public bool IsRotateActive => isRotateActive;

    private void Start()
    {
        cam = Camera.main;
        startPos = cam.transform.position;
        startRot = cam.transform.rotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRotateActive = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isRotateActive = false;
        }

        if (isRotateActive)
        {
            float mouseY = Input.GetAxis("Mouse X") * mouseSensivity;
            float mouseX = Input.GetAxis("Mouse Y") * mouseSensivity;

            rotationX += mouseX;
            rotationY += mouseY;
            rotationX = Mathf.Clamp(rotationX, -80, 80);

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
            transform.position = target.position - transform.forward * distanceFromTarget;
        }
        cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoom;
    }

    private void OnMontageStarted()
    {
        cam.transform.position = startPos;
        cam.transform.rotation = startRot;
    }

}

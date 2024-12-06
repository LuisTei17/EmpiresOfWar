using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 dragOrigin;
    public float minZoom = 20.0f;
    public float maxZoom = 100.0f; 

    public float dragSpeed = 40.0f;
    public float zoomSpeed = 15.0f;
    public float panSpeed = 15.0f;
    public float rotationSpeed = 50.0f;
    public Vector3 minBoundaries = new Vector3(-10, 7, -5);
    public Vector3 maxBoundaries = new Vector3(15, 7, 15);

    void Update()
    {
        HandleZoom();
        HandleDrag();
        HandlePan();
        HandleRotation();
        HandleKeyboardRotation();
    }

        void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.fieldOfView -= scroll * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minZoom, maxZoom);
    }

    void HandleDrag() {
       if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

        Vector3 right = transform.right;
        Vector3 forward = transform.forward;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = -(right * difference.x + forward * difference.y) * dragSpeed;

        transform.Translate(move, Space.World);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBoundaries.x, maxBoundaries.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBoundaries.y, maxBoundaries.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBoundaries.z, maxBoundaries.z);

        transform.position = clampedPosition;

        dragOrigin = Input.mousePosition;
    }

    void HandlePan()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 right = transform.right;
        Vector3 forward = transform.forward;

        right.y = 0;
        forward.y = 0;

        right.Normalize();
        forward.Normalize();

        Vector3 move = (right * horizontal + forward * vertical) * panSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBoundaries.x, maxBoundaries.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBoundaries.z, maxBoundaries.z);
        transform.position = clampedPosition;
    }

     void HandleRotation()
    {
        if (Input.GetMouseButton(2))
        {
            float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float vertical = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.up, horizontal, Space.World);

            transform.Rotate(Vector3.right, -vertical, Space.Self);
        }
    }

    void HandleKeyboardRotation()
    {
        float rotationInput = 0;

        if (Input.GetKey(KeyCode.Q))
            rotationInput = -1;
        else if (Input.GetKey(KeyCode.E))
            rotationInput = 1;

        if (rotationInput != 0)
            transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime, Space.World);
    }
}
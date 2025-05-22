using UnityEngine;

public class AircrafCameraController : MonoBehaviour
{
    public Transform target; // The aircraft to follow
    public float distance = 10.0f;
    public float height = 3.0f;
    public float rotationSpeed = 120.0f;
    public float zoomSpeed = 5.0f;
    public float minDistance = 5.0f;
    public float maxDistance = 30.0f;
    public float minPitch = -20f;
    public float maxPitch = 80f;

    private float yaw = 0.0f;
    private float pitch = 20.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse input for camera rotation using the new Input System
        Vector2 mouseDelta = UnityEngine.InputSystem.Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Mouse scroll for zoom
        float scroll = UnityEngine.InputSystem.Mouse.current.scroll.ReadValue().y * 0.1f;
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calculate camera position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + offset;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition,1f);

        // Look at the target 
        Quaternion lookRotation = Quaternion.LookRotation(target.position + 0.5f * height * Vector3.up - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
    }
}

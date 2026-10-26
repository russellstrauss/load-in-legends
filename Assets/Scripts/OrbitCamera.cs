using UnityEngine;
using UnityEngine.InputSystem;

// Third-person camera that follows a target and orbits around it.
// Hold right mouse button + move mouse: orbit
// Scroll wheel: zoom in/out
// Middle mouse click: snap back behind the target
public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                                  // drag the Forklift here
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);  // look at the cab, not the floor
    public bool followTargetRotation = true;                  // stay behind the forklift as it turns

    [Header("Orbit")]
    public float mouseSensitivity = 0.2f;
    public float minPitch = 5f;     // degrees; keeps the camera above the floor
    public float maxPitch = 80f;    // degrees; almost straight down
    public float defaultPitch = 20f;

    [Header("Zoom")]
    public float distance = 7f;
    public float minDistance = 3f;
    public float maxDistance = 15f;
    public float zoomStep = 1f;     // meters per scroll notch

    float yaw;      // left/right angle around the target, in degrees
    float pitch;    // up/down angle, in degrees

    void Start()
    {
        pitch = defaultPitch;
    }

    // LateUpdate runs after all movement for the frame,
    // so the camera always sees where the forklift ended up.

    public void SetTarget(Transform newTarget, bool followRotation)
    {
        target = newTarget;
        followTargetRotation = followRotation;
        yaw = followRotation ? 0f : newTarget.eulerAngles.y;   // start behind the new target
    }

    void LateUpdate()
    {
        if (target == null) return;

        Mouse mouse = Mouse.current;
        if (mouse != null)
        {
            if (mouse.rightButton.isPressed)
            {
                Vector2 delta = mouse.delta.ReadValue();
                yaw += delta.x * mouseSensitivity;
                pitch -= delta.y * mouseSensitivity;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }

            float scroll = mouse.scroll.ReadValue().y;
            if (scroll != 0f)
            {
                distance -= Mathf.Sign(scroll) * zoomStep;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
            }

            if (mouse.middleButton.wasPressedThisFrame)
            {
                yaw = 0f;
                pitch = defaultPitch;
            }
        }

        float baseYaw = followTargetRotation ? target.eulerAngles.y : 0f;
        Quaternion rotation = Quaternion.Euler(pitch, baseYaw + yaw, 0f);
        Vector3 focusPoint = target.position + targetOffset;

        transform.rotation = rotation;
        transform.position = focusPoint - rotation * Vector3.forward * distance;
    }
}
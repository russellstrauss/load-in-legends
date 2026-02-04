using UnityEngine;
using UnityEngine.InputSystem;

public class CubePlayerInput : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float gravity = -20f;

    [Header("Look")]
    public float lookSensitivity = 120f;   // degrees/sec
    public float minPitch = -80f;
    public float maxPitch = 80f;

    CharacterController controller;
    Transform cam;

    Vector3 velocity;
    float pitch;

    InputAction moveAction;
    InputAction lookAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Use the project-wide actions (InputSystem_Actions)
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");

        cam = Camera.main != null ? Camera.main.transform : null;
        if (cam != null)
            pitch = cam.localEulerAngles.x;
    }

    void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
    }

    void Update()
    {
        // --- LOOK (right stick) ---
        if (cam != null)
        {
            Vector2 look = lookAction.ReadValue<Vector2>();
            float yaw = look.x * lookSensitivity * Time.deltaTime;
            float pitchDelta = -look.y * lookSensitivity * Time.deltaTime;

            transform.Rotate(0f, yaw, 0f);

            pitch += pitchDelta;
            pitch = ClampAngle(pitch, minPitch, maxPitch);

            cam.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }

        // --- MOVE (left stick) ---
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = (transform.right * input.x + transform.forward * input.y);
        controller.Move(move * speed * Time.deltaTime);

        // --- GRAVITY ---
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    static float ClampAngle(float angle, float min, float max)
    {
        // Convert 0..360 into -180..180 for sane clamping
        angle = (angle > 180f) ? angle - 360f : angle;
        return Mathf.Clamp(angle, min, max);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

// Third-person on-foot movement. WASD moves relative to where the camera
// is looking, and the character turns to face the direction it walks.
// Uses a CharacterController: a capsule that moves and collides without
// being pushed around by physics, which suits walking characters.
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 4f;        // meters per second
    public float turnSpeed = 720f;      // degrees per second
    public float gravity = -20f;
    public Transform cameraTransform;   // leave empty to use the Main Camera

    CharacterController controller;
    InputAction moveAction;
    float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void OnEnable()
    {
        // Shared project-wide action: enable it, but never disable it here,
        // or the forklift would stop receiving input too.
        moveAction?.Enable();
        verticalVelocity = 0f;
    }

    // CharacterController movement goes in Update (it isn't a Rigidbody).
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Camera's forward and right, flattened onto the ground.
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;
        if (move.sqrMagnitude > 1f) move.Normalize();

        // Turn to face the walking direction.
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Gravity: keep a small downward push while grounded so we stay stuck to the floor.
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * walkSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
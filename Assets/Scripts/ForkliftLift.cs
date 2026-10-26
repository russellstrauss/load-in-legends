using UnityEngine;
using UnityEngine.InputSystem;

// Raises and lowers the forklift's carriage (the forks and backrest).
// Put this on the Forklift root, next to ForkliftController.
public class ForkliftLift : MonoBehaviour
{
    [Header("Parts")]
    public Transform carriage;          // the object holding the forks

    [Header("Lift")]
    public float liftSpeed = 0.4f;      // meters per second
    public float maxHeight = 2.5f;      // meters above the starting height

    InputAction liftAction;
    Vector3 carriageStartPosition;
    float currentHeight;

    void Awake()
    {
        // Remember where the carriage starts; that's the "fully lowered" height.
        carriageStartPosition = carriage.localPosition;

        // Prototype controls: R raises, F lowers (gamepad: right stick up/down).
        // Defined here so the shared InputSystem_Actions file stays untouched.
        // Once the controls are agreed on, move this into that file.
        liftAction = new InputAction("Lift", InputActionType.Value);
        liftAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/f")
            .With("Positive", "<Keyboard>/r");
        liftAction.AddBinding("<Gamepad>/rightStick/y");
    }

    void OnEnable()
    {
        liftAction.Enable();
    }

    void OnDisable()
    {
        liftAction.Disable();
    }

    void FixedUpdate()
    {
        float input = liftAction.ReadValue<float>();

        currentHeight += input * liftSpeed * Time.fixedDeltaTime;
        currentHeight = Mathf.Clamp(currentHeight, 0f, maxHeight);

        carriage.localPosition = carriageStartPosition + Vector3.up * currentHeight;
    }
}
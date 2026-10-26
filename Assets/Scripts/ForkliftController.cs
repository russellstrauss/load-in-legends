using UnityEngine;
using UnityEngine.InputSystem;

// Drives the forklift with WheelColliders.
// Front wheels push (drive), rear wheels steer, like a real forklift.
// Lifting comes later in its own script.
[RequireComponent(typeof(Rigidbody))]
public class ForkliftController : MonoBehaviour
{
    [Header("Wheels (drag the WheelCollider objects here)")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Driving")]
    public float motorTorque = 2500f;   // push strength of the front wheels
    public float brakeTorque = 6000f;   // used when stopping or reversing direction
    public float maxSteerAngle = 50f;   // forklifts turn very sharply
    public float maxSpeed = 5f;         // meters per second (about 18 km/h)
    public float steerSpeed = 120f;              // degrees per second the wheels can turn
    public float steerAngleAtMaxSpeed = 20f;     // less steering available when fast

    float currentSteerAngle;
    [Header("Physics")]
    // Relative to the Forklift root. Low and toward the rear,
    // because a real forklift carries a heavy counterweight at the back.
    public Vector3 centerOfMass = new Vector3(0f, 0.5f, -0.4f);

    Rigidbody body;
    InputAction moveAction;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.centerOfMass = centerOfMass;

        // Reuses the existing "Move" action (WASD / left stick) from the
        // project-wide InputSystem_Actions, so the shared file is unchanged.
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void OnEnable()
    {
        moveAction?.Enable();
    }

    // Physics code goes in FixedUpdate, which runs on Unity's fixed physics clock.
    void FixedUpdate()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        float throttle = input.y;   // W/S
        float steer = input.x;      // A/D

        // Positive when rolling forward, negative when rolling backward.
        float forwardSpeed = Vector3.Dot(body.linearVelocity, transform.forward);
        float torque = 0f;
        float brake = 0f;

        bool noInput = Mathf.Abs(throttle) < 0.05f;
        bool pressingAgainstMotion = throttle * forwardSpeed < 0f && Mathf.Abs(forwardSpeed) > 0.3f;
        bool tooFast = Mathf.Abs(forwardSpeed) > maxSpeed && throttle * forwardSpeed > 0f;

        if (noInput || pressingAgainstMotion)
        {
            brake = brakeTorque;          // stop before changing direction
        }
        else if (!tooFast)
        {
            torque = throttle * motorTorque;
        }

        frontLeft.motorTorque = torque;
        frontRight.motorTorque = torque;

        frontLeft.brakeTorque = brake;
        frontRight.brakeTorque = brake;
        rearLeft.brakeTorque = brake;
        rearRight.brakeTorque = brake;

        // Less steering at speed, full lock when slow.
        float speedFactor = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / maxSpeed);
        float allowedAngle = Mathf.Lerp(maxSteerAngle, steerAngleAtMaxSpeed, speedFactor);

        // Turn the wheels gradually toward the target, like a steering wheel.
        float targetAngle = -steer * allowedAngle;
        currentSteerAngle = Mathf.MoveTowards(currentSteerAngle, targetAngle, steerSpeed * Time.fixedDeltaTime);

        rearLeft.steerAngle = currentSteerAngle;
        rearRight.steerAngle = currentSteerAngle;
    }

    // Draws the center of mass as a red sphere in the Scene view.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(centerOfMass), 0.15f);
    }
}
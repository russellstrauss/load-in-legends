using UnityEngine;
using UnityEngine.InputSystem;

// Lets the player get in and out of the forklift with E.
// Put this on the Forklift root.
public class VehicleSeat : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public OrbitCamera orbitCamera;
    public ForkliftController driving;
    public ForkliftLift lift;
    public Transform exitPoint;         // where the player appears when getting out

    [Header("Settings")]
    public float enterDistance = 3f;    // how close the player must be to get in
    public bool startDriving = false;

    InputAction enterExitAction;
    bool isDriving;

    void Awake()
    {
        // E on keyboard, Y/Triangle on gamepad.
        enterExitAction = new InputAction("EnterExit", InputActionType.Button, "<Keyboard>/e");
        enterExitAction.AddBinding("<Gamepad>/buttonNorth");
    }

    void OnEnable()
    {
        enterExitAction.Enable();
    }

    void OnDisable()
    {
        enterExitAction.Disable();
    }

    void Start()
    {
        if (startDriving)
        {
            Enter();
        }
        else
        {
            // Start on foot with the forklift parked.
            driving.enabled = false;
            lift.enabled = false;
            orbitCamera.SetTarget(player.transform, false);
        }
    }

    void Update()
    {
        if (!enterExitAction.WasPressedThisFrame()) return;

        if (isDriving)
        {
            Exit();
        }
        else
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= enterDistance)
                Enter();
        }
    }

    void Enter()
    {
        isDriving = true;
        player.SetActive(false);        // hide the walking character
        driving.enabled = true;
        lift.enabled = true;
        orbitCamera.SetTarget(transform, true);
    }

    void Exit()
    {
        isDriving = false;
        driving.enabled = false;        // ForkliftController parks itself when disabled
        lift.enabled = false;

        // Move the player while inactive, so the CharacterController doesn't fight the teleport.
        player.transform.SetPositionAndRotation(exitPoint.position, exitPoint.rotation);
        player.SetActive(true);
        orbitCamera.SetTarget(player.transform, false);
    }

    // Shows the enter range as a yellow circle in the Scene view.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enterDistance);
    }
}
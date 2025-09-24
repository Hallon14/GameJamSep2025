using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    public InputAction moveAction;
    public InputAction chargeAction;
    public InputAction aimAction;
    public InputAction interactAction;
    public InputAction volleyAction;
    private Vector2 aimDirection;

    private bool inputsEnabled = true;

    //Charge started event
    public delegate void OnChargeStarted(Vector2 aimDirection);
    public static event OnChargeStarted onChargeStarted;

    //Charge ended event
    public delegate void OnChargeEnded();
    public static event OnChargeEnded onChargeEnded;

    //Interact event
    public delegate void OnInteract();
    public static event OnInteract onInteract;

    public delegate void OnVolley(Vector2 aimDirection);
    public static event OnVolley onVolley;

    void OnEnable()
    {
        PlayerHealth.onPlayerDeath += DisableInputs;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        chargeAction = InputSystem.actions.FindAction("Charge");
        aimAction = InputSystem.actions.FindAction("Aim");
        interactAction = InputSystem.actions.FindAction("Interact");
        volleyAction = InputSystem.actions.FindAction("Volley");

        // Ensure actions are enabled so movement & input work
        moveAction?.Enable();
        chargeAction?.Enable();
        aimAction?.Enable();
        interactAction?.Enable();
        volleyAction?.Enable();

    }

    void Update()
    {
        if (inputsEnabled)
        {
            CalculateShotDirection();
            CheckChargeAction();
            CheckInteractAction();
            CheckVolleyAction();
        }

    }

    public void DisableInputs()
    {
        inputsEnabled = false;
    }

    void CheckChargeAction()
    {
        if (chargeAction.WasPressedThisFrame())
        {
            
            onChargeStarted?.Invoke(aimDirection);
        }
        if (chargeAction.WasReleasedThisFrame())
        {
            
            onChargeEnded?.Invoke();
        }
    }

    void CheckVolleyAction()
    {
        if (volleyAction.WasPressedThisFrame())
        {
            onVolley?.Invoke(aimDirection);
        }
    }

    void CheckInteractAction()
    {
        if (interactAction.WasPressedThisFrame())
        {
            onInteract?.Invoke();
        }

    }

    void CalculateShotDirection()
    {
        Vector2 mousePosition = aimAction.ReadValue<Vector2>();

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 aimVector = mousePosition - new Vector2(transform.position.x, transform.position.y);
        //aimVector.Normalize();
        aimDirection = aimVector.normalized;
    }

    public Vector2 GetAimDirection()
    {
        return aimDirection;
    }


}

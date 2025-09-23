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

    //Charge started event
    public delegate void OnChargeStarted(Vector2 aimDirection);
    public static event OnChargeStarted onChargeStarted;

    //Charge ended event
    public delegate void OnChargeEnded();
    public static event OnChargeEnded onChargeEnded;

    //Interact event
    public delegate void OnInteract();
    public static event OnInteract onInteract;

    public delegate void OnVolley();
    public static event OnVolley onVolley;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        chargeAction = InputSystem.actions.FindAction("Charge");
        aimAction = InputSystem.actions.FindAction("Aim");
        interactAction = InputSystem.actions.FindAction("Interact");
        volleyAction = InputSystem.actions.FindAction("Volley");

    }

    void Update()
    {
        CalculateShotDirection();
        CheckChargeAction();
        CheckInteractAction();
        CheckVolleyAction();
    }

    void CheckChargeAction()
    {
        if (chargeAction.WasPressedThisFrame())
        {
            Debug.Log("charge started");
            onChargeStarted?.Invoke(aimDirection);
        }
        if (chargeAction.WasReleasedThisFrame())
        {
            Debug.Log("charge ended");
            onChargeEnded?.Invoke();
        }
    }

    void CheckVolleyAction()
    {
        if (volleyAction.WasPressedThisFrame())
        {
            Debug.Log("Volley started");
            onVolley?.Invoke();
        }
    }

    void CheckInteractAction()
    {
        if (interactAction.WasPressedThisFrame())
        {
            Debug.Log("interacted");
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

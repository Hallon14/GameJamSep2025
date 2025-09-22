using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    public InputAction moveAction;
    public InputAction pushAction;
    public InputAction pullAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        pushAction = InputSystem.actions.FindAction("Shoot");
    }

}

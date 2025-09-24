using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputHandler input;
    public float maxSpeed;
    public Transform sights;
    private bool canMove = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        PlayerHealth.onPlayerDeath += DisableMovement;
    }
    void OnDisable()
    {
        PlayerHealth.onPlayerDeath -= DisableMovement;
    }

    void Start()
    {
        input = GetComponent<InputHandler>();
    }

    public void DisableMovement()
    {
        canMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.position += (Vector3)input.moveAction.ReadValue<Vector2>() * maxSpeed * Time.deltaTime;
            Vector2 aimDirection = input.GetAimDirection();
            sights.position = aimDirection + (Vector2)transform.position;
            sights.up = aimDirection;
        }
    }
}

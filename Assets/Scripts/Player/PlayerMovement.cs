using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputHandler input;
    public float maxSpeed;
    public Transform sights;
    private bool canMove = true;
    private Rigidbody2D rb2D;
    private Vector2 inputMoveVector;
    private SpriteRenderer sprite;
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
        rb2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 force = inputMoveVector * maxSpeed;
            rb2D.AddForce(force);
        }
    }
    // Update is called once per frame
    void Update()
    {
        inputMoveVector = input.moveAction.ReadValue<Vector2>();

        if (canMove)
        {
            //transform.position += (Vector3)input.moveAction.ReadValue<Vector2>() * maxSpeed * Time.deltaTime;
            Vector2 aimDirection = input.GetAimDirection();
            sights.position = aimDirection + (Vector2)transform.position;
            sights.up = aimDirection;

            if (inputMoveVector.x > 0)
            {
                sprite.flipX = true;
            }
            else if (inputMoveVector.x < 0)
            {
                sprite.flipX = false;
            }

        }
    }
}

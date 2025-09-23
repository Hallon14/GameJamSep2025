using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputHandler input;
    public float maxSpeed;
    public Transform sights;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)input.moveAction.ReadValue<Vector2>() * maxSpeed * Time.deltaTime;

        Vector2 aimDirection = input.GetAimDirection();
        sights.position = aimDirection + (Vector2)transform.position;
        sights.up = aimDirection;
    }
}

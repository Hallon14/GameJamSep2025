using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputHandler input;
    public float maxSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)input.moveAction.ReadValue<Vector2>() * maxSpeed * Time.deltaTime;
    }
}

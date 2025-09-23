using UnityEngine;

public class BruteBehavior : MonoBehaviour
{
    public float maxSpeed = 3f;
    public float rotationspeed = 1f;
    public float radius = 2f;
    Rigidbody2D rb;
    Transform player;
    float angle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        angle = 0f;
    }

    void FixedUpdate()
    {
        if (player == null) return;
        // Calculate current direction from player to brute
        Vector2 toBrute = rb.position - (Vector2)player.position;
        float currentDistance = toBrute.magnitude;
        // If not at radius, move directly outward/inward to maintain radius
        if (Mathf.Abs(currentDistance - radius) > 0.05f)
        {
            Vector2 desiredPos = (Vector2)player.position + toBrute.normalized * radius;
            Vector2 newPos = Vector2.MoveTowards(rb.position, desiredPos, maxSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else
        {
            // Orbit around the player at the set radius
            angle = Mathf.Atan2(toBrute.y, toBrute.x);
            angle += rotationspeed * Time.fixedDeltaTime;
            Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Vector2 newPos = Vector2.MoveTowards(rb.position, orbitPos, maxSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }
}

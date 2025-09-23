using UnityEngine;

public class BruteBehavior : MonoBehaviour
{
    public float maxSpeed = 3f;
    private float rotationspeed = 120f; // degrees per second (360/5 = 72 for 5 seconds per orbit)
    public float radius = 2f;
    Rigidbody2D rb;
    Transform player;
    float angle;
    bool reachedRadius = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        // Set initial angle based on current position relative to player
        if (player != null)
        {
            Vector2 toBrute = rb.position - (Vector2)player.position;
            angle = Mathf.Atan2(toBrute.y, toBrute.x) * Mathf.Rad2Deg;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 toBrute = rb.position - (Vector2)player.position;
        float currentDistance = toBrute.magnitude;
        if (!reachedRadius)
        {
            // Move toward the radius
            if (Mathf.Abs(currentDistance - radius) > 0.05f)
            {
                Vector2 desiredPos = (Vector2)player.position + toBrute.normalized * radius;
                Vector2 newPos = Vector2.MoveTowards(rb.position, desiredPos, maxSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            else
            {
                reachedRadius = true;
                // Set angle for smooth orbit
                angle = Mathf.Atan2(toBrute.y, toBrute.x) * Mathf.Rad2Deg;
            }
        }
        else
        {
            // Use degrees per second for orbit
            angle += rotationspeed * Time.fixedDeltaTime;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            rb.MovePosition(orbitPos);
        }
    }
}

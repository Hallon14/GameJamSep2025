using UnityEngine;

public class ArcherAllyBehavior : MonoBehaviour
{

    private int ArcherHP;
    public float startSpeed = 8f; // Slightly faster than brute
    private float rotationspeed = 90f; // Faster orbit (degrees per second)
    public float radius = 2f; // Closer to player than brute
    Rigidbody2D rb;
    Transform player;
    float angle;
    bool reachedRadius = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ArcherHP = 1;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        if (player != null)
        {
            Vector2 toArcher = rb.position - (Vector2)player.position;
            angle = Mathf.Atan2(toArcher.y, toArcher.x) * Mathf.Rad2Deg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        Vector2 toArcher = rb.position - (Vector2)player.position;
        float currentDistance = toArcher.magnitude;
        if (!reachedRadius)
        {
            if (Mathf.Abs(currentDistance - radius) > 0.05f)
            {
                Vector2 desiredPos = (Vector2)player.position + toArcher.normalized * radius;
                Vector2 newPos = Vector2.MoveTowards(rb.position, desiredPos, startSpeed * Time.deltaTime);
                rb.MovePosition(newPos);
            }
            else
            {
                reachedRadius = true;
                angle = Mathf.Atan2(toArcher.y, toArcher.x) * Mathf.Rad2Deg;
            }
        }
        else
        {
            angle += rotationspeed * Time.deltaTime;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            rb.MovePosition(orbitPos);
        }
    }
}

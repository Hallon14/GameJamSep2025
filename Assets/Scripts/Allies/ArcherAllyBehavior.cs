using Unity.VisualScripting;
using UnityEngine;

public class ArcherAllyBehavior : MonoBehaviour
{
    public float separationForce = 5f;
    [Tooltip("Minimum spacing to keep from other archers")] public float separationDistance = 0.6f;
    [Tooltip("How strongly to apply separation positional offset (0-1 typical)")] public float separationWeight = 0.5f;

    public int maxHealth = 4;
    private int currentHealth;
    public float startSpeed = 8f; // Slightly faster than brute
    private float rotationSpeed = 90f; // Faster orbit (degrees per second)
    public float radius = 2f; // Closer to player than brute
    Rigidbody2D rb;
    Transform player;
    float angle;
    bool reachedRadius = false;

    // --- Separation (kinematic friendly) static registry ---
    private static readonly System.Collections.Generic.List<ArcherAllyBehavior> AllArchers = new System.Collections.Generic.List<ArcherAllyBehavior>();

    void OnEnable()
    {
        if (!AllArchers.Contains(this)) AllArchers.Add(this);
    }

    void OnDisable()
    {
        AllArchers.Remove(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        //ArcherHP = 1;
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
    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 toArcher = rb.position - (Vector2)player.position;
        float currentDistance = toArcher.magnitude;
        Vector2 targetPos;
        if (!reachedRadius)
        {
            if (Mathf.Abs(currentDistance - radius) > 0.05f)
            {
                Vector2 desiredPos = (Vector2)player.position + toArcher.normalized * radius;
                Vector2 newPos = Vector2.MoveTowards(rb.position, desiredPos, startSpeed * Time.fixedDeltaTime);
                targetPos = newPos;
            }
            else
            {
                reachedRadius = true;
                angle = Mathf.Atan2(toArcher.y, toArcher.x) * Mathf.Rad2Deg;
                targetPos = rb.position; // stay where we snapped
            }
        }
        else
        {
            angle -= rotationSpeed * Time.fixedDeltaTime;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            targetPos = orbitPos;
        }

        // --- Separation positional adjustment (works with kinematic bodies) ---
        Vector2 separationOffset = Vector2.zero;
        foreach (var other in AllArchers)
        {
            if (other == null || other == this) continue;
            Vector2 diff = targetPos - (Vector2)other.rb.position;
            float dist = diff.magnitude;
            if (dist > 0f && dist < separationDistance)
            {
                // Inverse proportional push (closer = stronger)
                float strength = (separationDistance - dist) / separationDistance; // 0..1
                separationOffset += diff.normalized * strength;
            }
        }
        if (separationOffset.sqrMagnitude > 0f)
        {
            separationOffset = Vector2.ClampMagnitude(separationOffset, separationDistance) * separationWeight;
            targetPos += separationOffset;
        }

        rb.MovePosition(targetPos);
    }
    // NOTE: Removed physics AddForce separation (ineffective for kinematic bodies) in favor of manual positional separation above.

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
            Destroy(other.gameObject);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        GameManager.Instance.decreaseFriendCount();
        Destroy(gameObject);
    }
}

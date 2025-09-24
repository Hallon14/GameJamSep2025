using Unity.VisualScripting;
using UnityEngine;

public class ArcherAllyBehavior : MonoBehaviour
{
    public float separationForce = 5f;
    [Tooltip("Minimum spacing to keep from other archers")] public float separationDistance = 0.6f;
    [Tooltip("How strongly to apply separation positional offset (0-1 typical)")] public float separationWeight = 0.5f;

    public float maxHealth = 4f;
    private float currentHealth;
    public float startSpeed = 8f; // Slightly faster than brute
    private float rotationSpeed = 90f; // Faster orbit (degrees per second)
    [Header("Dynamic Radius Settings")] 
    [Tooltip("Base orbit radius when only one archer exists")] public float radius = 1.5f; 
    [Tooltip("Additional radius added per extra archer")] public float radiusPerArcher = 0.05f; // reduced growth per added archer
    [Tooltip("Clamp the dynamic radius so it never exceeds this value")] public float maxRadius = 6f;
    [Tooltip("How quickly the orbit radius eases toward its new size when count changes")] public float radiusSmoothing = 6f;
    private float currentOrbitRadius;
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
        currentOrbitRadius = radius; // start at base radius
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null) return;
        Vector2 toArcher = rb.position - (Vector2)player.position;
        float currentDistance = toArcher.magnitude;

        // --- Dynamic radius calculation ---
        int count = AllArchers.Count; // number of active archers
        float desiredRadius = Mathf.Min(radius + (count - 1) * radiusPerArcher, maxRadius);
        // Smooth toward desired to avoid popping when an archer is added/removed
        float rsLerp = 1f - Mathf.Exp(-radiusSmoothing * Time.fixedDeltaTime);
        currentOrbitRadius = Mathf.Lerp(currentOrbitRadius, desiredRadius, rsLerp);

        Vector2 targetPos;
        if (!reachedRadius)
        {
            if (Mathf.Abs(currentDistance - currentOrbitRadius) > 0.05f)
            {
                Vector2 desiredPos = (Vector2)player.position + toArcher.normalized * currentOrbitRadius;
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
            Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * currentOrbitRadius;
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

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.decreaseFriendCount();
        }

        Destroy(gameObject);
    }

}

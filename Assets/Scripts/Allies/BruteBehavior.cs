using UnityEngine;

public class BruteBehavior : MonoBehaviour
{
    // Old AddForce-based separation removed (ineffective for kinematic bodies)
    [Tooltip("Minimum spacing to keep from other brutes")] public float separationDistance = 0.9f;
    [Tooltip("How strongly to apply separation positional offset (0-1 typical)")] public float separationWeight = 0.5f;

    public int maxHealth = 12;
    private int currentHealth;
    public float startSpeed = 6f; // Increased start speed
    public float chargeSpeed = 18f; // Much faster charge speed
    private float currentSpeed;
    private float rotationspeed = 60f; // Slower orbit (degrees per second)
    public float radius = 4f;
    Rigidbody2D rb;
    Transform player;
    float angle;
    bool reachedRadius = false;
    bool isCharging = false;
    Vector2 chargeDirection;

    // Static registry for manual separation (kinematic friendly)
    private static readonly System.Collections.Generic.List<BruteBehavior> AllBrutes = new System.Collections.Generic.List<BruteBehavior>();

    void OnEnable()
    {
        if (!AllBrutes.Contains(this)) AllBrutes.Add(this);
        InputHandler.onChargeStarted += Charge;
        InputHandler.onChargeEnded += EndCharge;
    }

    void OnDisable()
    {
        AllBrutes.Remove(this);
        InputHandler.onChargeStarted -= Charge;
        InputHandler.onChargeEnded -= EndCharge;
    }

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        currentSpeed = startSpeed;
        if (player != null)
        {
            Vector2 toBrute = rb.position - (Vector2)player.position;
            angle = Mathf.Atan2(toBrute.y, toBrute.x) * Mathf.Rad2Deg;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (isCharging)
        {
            // During charge we skip separation to keep a decisive motion.
            currentSpeed = chargeSpeed;
            rb.MovePosition(rb.position + chargeDirection * currentSpeed * Time.fixedDeltaTime);
            return;
        }

        Vector2 toBrute = rb.position - (Vector2)player.position;
        float currentDistance = toBrute.magnitude;
        Vector2 targetPos;
        if (!reachedRadius)
        {
            if (Mathf.Abs(currentDistance - radius) > 0.05f)
            {
                currentSpeed = startSpeed;
                Vector2 desiredPos = (Vector2)player.position + toBrute.normalized * radius;
                Vector2 newPos = Vector2.MoveTowards(rb.position, desiredPos, currentSpeed * Time.fixedDeltaTime);
                targetPos = newPos;
            }
            else
            {
                reachedRadius = true;
                angle = Mathf.Atan2(toBrute.y, toBrute.x) * Mathf.Rad2Deg;
                targetPos = rb.position; // snapped to orbit
            }
        }
        else
        {
            angle += rotationspeed * Time.fixedDeltaTime;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            targetPos = orbitPos;
        }

        // --- Separation positional adjustment (kinematic friendly) ---
        Vector2 separationOffset = Vector2.zero;
        foreach (var other in AllBrutes)
        {
            if (other == null || other == this) continue;
            Vector2 diff = targetPos - (Vector2)other.rb.position;
            float dist = diff.magnitude;
            if (dist > 0f && dist < separationDistance)
            {
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

    void Charge(Vector2 direction)
    {
        isCharging = true;
        chargeDirection = direction.normalized;
    }

    void EndCharge()
    {
        isCharging = false;
        reachedRadius = false; // Move back to radius and resume orbit
    }

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

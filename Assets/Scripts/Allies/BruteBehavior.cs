using UnityEngine;

public class BruteBehavior : MonoBehaviour
{
    // Old AddForce-based separation removed (ineffective for kinematic bodies)
    [Tooltip("Minimum spacing to keep from other brutes")] public float separationDistance = 0.9f;
    [Tooltip("How strongly to apply separation positional offset (0-1 typical)")] public float separationWeight = 0.5f;
    [Tooltip("Higher = snappier positional response; lower = smoother (for anti-jitter)")] public float movementSmoothing = 10f;
    [Tooltip("Smoothing factor for separation offset (higher = snappier, 0 = off)")] public float separationSmoothing = 12f;

    public float maxHealth = 12;
    private float currentHealth;
    public float startSpeed = 6f; // Increased start speed
    public float chargeSpeed = 18f; // Much faster charge speed
    private float currentSpeed;
    [Tooltip("Orbit angular speed in degrees per second")] public float orbitDegreesPerSecond = 110f; // was 60f
    [Header("Dynamic Radius Settings")]
    [Tooltip("Base orbit radius when only one brute exists")] public float radius = 2.5f; // reduced to sit a bit closer to player
    [Tooltip("Additional radius added per extra brute")] public float radiusPerBrute = 0.12f;
    [Tooltip("Clamp the dynamic radius so it never exceeds this value")] public float maxRadius = 10f;
    [Tooltip("How quickly the orbit radius eases toward its new size when count changes")] public float radiusSmoothing = 5f;
    private float currentOrbitRadius;
    Rigidbody2D rb;
    Transform player;
    float angle;
    bool reachedRadius = false;
    bool isCharging = false;
    Vector2 chargeDirection;
    // Smoothed separation accumulator to prevent frame-to-frame oscillation
    private Vector2 smoothedSeparation;

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
        currentOrbitRadius = radius;
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

        // --- Dynamic radius calculation ---
        int bruteCount = AllBrutes.Count;
        float desiredRadius = Mathf.Min(radius + (bruteCount - 1) * radiusPerBrute, maxRadius);
        float rsLerp = 1f - Mathf.Exp(-radiusSmoothing * Time.fixedDeltaTime);
        currentOrbitRadius = Mathf.Lerp(currentOrbitRadius, desiredRadius, rsLerp);
        Vector2 targetPos;
        Vector2 baseOrbitPos = rb.position; // will be overwritten once angle path known
        if (!reachedRadius)
        {
            if (Mathf.Abs(currentDistance - currentOrbitRadius) > 0.05f)
            {
                currentSpeed = startSpeed;
                Vector2 desiredPos = (Vector2)player.position + toBrute.normalized * currentOrbitRadius;
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
            angle += orbitDegreesPerSecond * Time.fixedDeltaTime; // integrate logical orbit angle (no resync to avoid jitter)
            float angleRad = angle * Mathf.Deg2Rad;
            baseOrbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * currentOrbitRadius;
            targetPos = baseOrbitPos; // separation applied after
        }
        // --- Separation positional adjustment (kinematic friendly) ---
        // For orbit phase we separate relative to the ideal circle position (baseOrbitPos) to reduce angular feedback jitter.
        Vector2 rawSeparation = Vector2.zero;
        foreach (var other in AllBrutes)
        {
            if (other == null || other == this) continue;
            Vector2 otherPos = (Vector2)other.rb.position;
            Vector2 referencePos = reachedRadius ? baseOrbitPos : targetPos; // during approach use current targetPos
            Vector2 diff = referencePos - otherPos;
            float dist = diff.magnitude;
            if (dist > 0f && dist < separationDistance)
            {
                float strength = (separationDistance - dist) / separationDistance; // 0..1
                rawSeparation += diff.normalized * strength;
            }
        }
        if (rawSeparation.sqrMagnitude > 0f)
        {
            rawSeparation = Vector2.ClampMagnitude(rawSeparation, separationDistance) * separationWeight;
        }
        // Smooth separation to avoid frame-to-frame oscillations (esp. with multiple brutes adjacent)
        if (separationSmoothing > 0f)
        {
            float sepLerp = 1f - Mathf.Exp(-separationSmoothing * Time.fixedDeltaTime);
            smoothedSeparation = Vector2.Lerp(smoothedSeparation, rawSeparation, sepLerp);
        }
        else
        {
            smoothedSeparation = rawSeparation;
        }
        targetPos += smoothedSeparation;
        if (!reachedRadius)
        {
            // While approaching the ring, move at full speed without smoothing to avoid sluggish feel.
            rb.MovePosition(targetPos);
        }
        else
        {
            // Direct orbit placement (keep angle integration independent of separation so no jitter feedback)
            rb.MovePosition(targetPos);
        }
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

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            TakeDamage(40f * Time.deltaTime);
        }
    }


    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
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

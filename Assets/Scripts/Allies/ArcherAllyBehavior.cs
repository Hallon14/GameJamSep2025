using Unity.VisualScripting;
using UnityEngine;

public partial class ArcherAllyBehavior : MonoBehaviour
{
    public float separationForce = 5f;
    [Tooltip("Minimum spacing to keep from other archers")] public float separationDistance = 0.6f;
    [Tooltip("How strongly to apply separation positional offset (0-1 typical)")] public float separationWeight = 0.5f;
    [Tooltip("Higher = snappier positional response after reaching orbit; lower = smoother (anti-jitter)")] public float movementSmoothing = 10f;
    [Header("Orbit Distribution")]
    [Tooltip("Maintain evenly spaced angles instead of free-running angles")] public bool useSlotDistribution = true;
    [Tooltip("(Deprecated) Linear speed along ring. Leave >0 only if you disable fixedAngularSpeed.")] public float tangentialSpeed = 6f;
    [Tooltip("Use a constant angular speed (deg/sec) for slot orbiting")] public float fixedAngularSpeed = 90f;

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
            if (useSlotDistribution)
            {
                int slotCount = AllArchers.Count;
                if (slotCount == 0) slotCount = 1;
                int index = AllArchers.IndexOf(this);
                if (index < 0) index = 0;

                // Determine angular speed: prefer fixedAngularSpeed; fallback to tangentialSpeed if fixed is <= 0
                float dynamicRotDegPerSec = fixedAngularSpeed > 0f ? fixedAngularSpeed :
                    ((currentOrbitRadius > 0.05f && tangentialSpeed > 0f) ? (tangentialSpeed / currentOrbitRadius) * Mathf.Rad2Deg : rotationSpeed);

                // Advance a shared phase using the first archer as the driver to avoid multi-increment
                AdvanceGlobalPhase(dynamicRotDegPerSec * Time.fixedDeltaTime);

                float spacing = 360f / slotCount;
                float targetAngle = globalOrbitPhase + index * spacing;
                // Directly lock to slot angle (constant speed determined solely by global phase advance)
                angle = targetAngle;
                float angleRad = angle * Mathf.Deg2Rad;
                Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * currentOrbitRadius;
                targetPos = orbitPos;
            }
            else
            {
                // Legacy free-spin mode
                angle -= rotationSpeed * Time.fixedDeltaTime;
                float angleRad = angle * Mathf.Deg2Rad;
                Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * currentOrbitRadius;
                targetPos = orbitPos;
            }
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
        if (!reachedRadius)
        {
            // Approach phase: no smoothing so archers settle quickly.
            rb.MovePosition(targetPos);
        }
        else
        {
            // Orbit phase: apply smoothing to damp separation/orbit interplay jitter.
            float lerpFactor = 1f - Mathf.Exp(-movementSmoothing * Time.fixedDeltaTime);
            Vector2 finalPos = Vector2.Lerp(rb.position, targetPos, lerpFactor);
            rb.MovePosition(finalPos);

            // Re-sync angle to real position so separation offsets don't cause snap-back to mathematical circle.
            if (!useSlotDistribution)
            {
                // Only resync in free mode; in slot mode angle is controlled.
                Vector2 dir = finalPos - (Vector2)player.position;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                }
            }
        }
    }
    
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

// Static helpers block (kept outside class scope intentionally if needed by future orbiting allies)
public partial class ArcherAllyBehavior
{
    // Shared orbit phase used only when slot distribution is enabled.
    private static float globalOrbitPhase = 0f;
    private static void AdvanceGlobalPhase(float deltaDegrees)
    {
        if (AllArchers.Count == 0) return;
        // Only the first active archer advances the shared phase to avoid accumulating multiple times per frame.
        var first = AllArchers[0];
        if (first == null) return;
        // We can detect driver by reference equality
        if (ReferenceEquals(first, AllArchers[0]))
        {
            // Clamp wrap
            globalOrbitPhase = (globalOrbitPhase + deltaDegrees) % 360f;
            if (globalOrbitPhase < 0f) globalOrbitPhase += 360f;
        }
    }
}

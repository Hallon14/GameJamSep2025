using UnityEngine;

public class BruteBehavior : MonoBehaviour
{
    public float separationForce = 5f;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject != this.gameObject && collision.gameObject.GetComponent<BruteBehavior>() != null)
        {
            Vector2 away = (rb.position - (Vector2)collision.transform.position).normalized;
            rb.AddForce(away * separationForce, ForceMode2D.Force);
        }
    }

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

    void OnEnable()
    {
        InputHandler.onChargeStarted += Charge;
        InputHandler.onChargeEnded += EndCharge;
    }

    void OnDisable()
    {
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
            currentSpeed = chargeSpeed;
            rb.MovePosition(rb.position + chargeDirection * currentSpeed * Time.fixedDeltaTime);
            return;
        }
        Vector2 toBrute = rb.position - (Vector2)player.position;
        float currentDistance = toBrute.magnitude;
        if (!reachedRadius)
        {
            if (Mathf.Abs(currentDistance - radius) > 0.05f)
            {
                currentSpeed = startSpeed;
                Vector2 desiredPos = (Vector2)player.position + toBrute.normalized * radius;
                Vector2 newPos = Vector2.MoveTowards(rb.position, desiredPos, currentSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }
            else
            {
                reachedRadius = true;
                angle = Mathf.Atan2(toBrute.y, toBrute.x) * Mathf.Rad2Deg;
            }
        }
        else
        {
            angle += rotationspeed * Time.fixedDeltaTime;
            float angleRad = angle * Mathf.Deg2Rad;
            Vector2 orbitPos = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            rb.MovePosition(orbitPos);
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

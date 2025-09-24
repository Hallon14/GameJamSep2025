using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public Transform attackTarget;
    public float attackRange;
    public float attackRate;
    public float movementSpeed = 2f;
    public float preferredDistance;
    public float preferredDistanceRange;

    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 2f;

    public Rigidbody2D rb2D;

    public bool hasTarget = true;

    void OnEnable()
    {
        PlayerHealth.onPlayerDeath += DisableEnemy;
    }
    void OnDisable()
    {
        PlayerHealth.onPlayerDeath -= DisableEnemy;
    }

    public virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        attackTarget = GameObject.Find("Player")?.transform;
        InvokeRepeating("TryAttack", 0, attackRate);
    }

    public virtual void TryAttack()
    {
        if (hasTarget)
        {
            if ((transform.position - attackTarget.position).sqrMagnitude < attackRange * attackRange)
            {
                Attack();
            }
        }

    }

    public virtual void Attack()
    {

    }

    public void FixedUpdate()
    {
        if (hasTarget)
        {
            Move();
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
        }


    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Move()
    {

        if ((attackTarget.position - transform.position).sqrMagnitude < Mathf.Pow(preferredDistance - preferredDistanceRange, 2))
        {
            rb2D.AddForce(-(attackTarget.position - transform.position).normalized * movementSpeed);
        }
        else if ((attackTarget.position - transform.position).sqrMagnitude > Mathf.Pow(preferredDistance + preferredDistanceRange, 2))
        {
            rb2D.AddForce((attackTarget.position - transform.position).normalized * movementSpeed);
        }
        else
        {

        }

    }

    public void DisableEnemy()
    {
        hasTarget = false;
    }
}

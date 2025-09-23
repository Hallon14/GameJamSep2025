using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public Transform attackTarget;
    public float attackRange;
    public float attackRate;
    public float movementSpeed = 2f;

    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 2f;

    public Rigidbody2D rb2D;

    public virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        attackTarget = GameObject.Find("Player")?.transform;
        InvokeRepeating("TryAttack", 0, attackRate);
    }

    public virtual void TryAttack()
    {
        if ((transform.position - attackTarget.position).sqrMagnitude < attackRange * attackRange)
        {
            Attack();
        }
    }

    public virtual void Attack()
    {

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
        Destroy(gameObject);
    }
}

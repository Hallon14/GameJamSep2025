using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject undeadVersion;
    public float maxHealth;
    private float currentHealth;
    public Transform attackTarget;
    public float attackRange;
    public float attackRate;
    public float movementSpeed = 2f;
    public float preferredDistance;
    public float preferredDistanceRange;

    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 2f;

    public float contactDPS = 20;

    public Rigidbody2D rb2D;
    public Transform allyParent;

    public bool hasTarget = true;
    public HitFlash hitFlash;

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
        allyParent = GameObject.Find("AllyParent")?.transform;
        InvokeRepeating("TryAttack", Random.Range(0f, 1f), attackRate);
        hitFlash = GetComponent<HitFlash>();
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
        GetComponent<SoundPlayer>().PlayAttackSound();
    }

    public void FixedUpdate()
    {
        if (hasTarget)
        {
            Move();
        }

    }

    public virtual void TakeDamage(float damage)
    {
        hitFlash.HitEffect();
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            TakeDamage(other.gameObject.GetComponent<Projectile>().damage);
            Destroy(other.gameObject);
        }


    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BruteAlly"))
        {

            TakeDamage(contactDPS * Time.deltaTime);
        }
    }

    public virtual void Die()
    {
        SpawnUndead();
        if (GameManager.Instance)
        {
            GameManager.Instance.increaseFriendCount();
        }
        Destroy(gameObject);
    }

    public void SpawnUndead()
    {
        //allyParent = GameObject.Find("AllyParent").transform;
        Instantiate(undeadVersion, transform.position, Quaternion.identity, allyParent);

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

using NUnit.Framework;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject undeadVersion;
    public float maxHealth;
    private float currentHealth;
    public Transform attackTarget;
    private GameObject player;
    public float attackRange;
    public float attackRate;
    public float movementSpeed = 2f;
    public float preferredDistance;
    public float preferredDistanceRange;

    private int allySpawnCap = 200;

    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 2f;

    public float contactDPS = 20;

    public Rigidbody2D rb2D;

    [HideInInspector]
    public Transform allyParent;

    public bool hasTarget = true;
    public HitFlash hitFlash;
    private bool playerIsAlive;

    [Header("Death Sequence")] public GameObject deathSequencePrefab; // assign same as archer/warrior death animation

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
        playerIsAlive = true;
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        attackTarget = GameObject.Find("Player")?.transform;
        player = GameObject.Find("Player");
        allyParent = GameObject.Find("AllyParent")?.transform;
        InvokeRepeating("TryAttack", Random.Range(0f, 1f), attackRate);
        hitFlash = GetComponent<HitFlash>();
    }

    public virtual void TryAttack()
    {
        if ((player.transform.position - gameObject.transform.position).magnitude <= attackRange)
        {
            attackTarget = player.transform;
            hasTarget = true;
        }
        else
        {
            attackTarget = GetTargetInRange()?.transform;
            if (attackTarget == null)
            {
                hasTarget = false;
            }
        }

        if (hasTarget)
        {
            if ((transform.position - attackTarget.position).sqrMagnitude < attackRange * attackRange)
            {
                Attack();
            }
        }

    }
    public GameObject GetTargetInRange()
    {
        foreach (Transform enemy in allyParent)
        {
            if ((transform.position - enemy.position).sqrMagnitude < attackRange * attackRange)
            {
                hasTarget = true;
                return enemy.gameObject;
            }
        }


        return null;
    }

    public virtual void Attack()
    {
        GetComponent<SoundPlayer>().PlayAttackSound();
    }

    public void FixedUpdate()
    {
        Move();
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
            //GetComponent<SoundPlayer>().PlayTakeDamageSound();
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
        if (deathSequencePrefab != null)
        {
            Instantiate(deathSequencePrefab, transform.position, transform.rotation);
        }
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
        if (allyParent.childCount < allySpawnCap)
        {
            Instantiate(undeadVersion, transform.position, Quaternion.identity, allyParent);
        }

    }

    public virtual void Move()
    {
        if (playerIsAlive)
        {
            if (!hasTarget || !attackTarget)
            {
                attackTarget = player.transform;
                //rb2D.AddForce((player.transform.position - transform.position).normalized * movementSpeed);
                //return;
            }

            if ((attackTarget.position - transform.position).sqrMagnitude < Mathf.Pow(preferredDistance - preferredDistanceRange, 2))
            {
                rb2D.AddForce(-(attackTarget.position - transform.position).normalized * movementSpeed);
                return;
            }
            else if ((attackTarget.position - transform.position).sqrMagnitude > Mathf.Pow(preferredDistance + preferredDistanceRange, 2))
            {
                rb2D.AddForce((attackTarget.position - transform.position).normalized * movementSpeed);
                return;
            }
            else
            {
            }
        }

    }

    public void DisableEnemy()
    {
        playerIsAlive = false;
        hasTarget = false;
        CancelInvoke();
    }
}

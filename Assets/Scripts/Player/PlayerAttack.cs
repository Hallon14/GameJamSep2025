using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private InputHandler inputHandler;
    private GameObject attackTarget;
    public Transform enemyParent;
    public Transform allyParent;
    public float attackRange;
    public float attackRate;
    public GameObject projectile;
    public float projectileSpeed;
    public float projectileLifetime;
    private bool playerIsDead;

    void OnEnable()
    {
        PlayerHealth.onPlayerDeath += setPlayerDead;
    }
    void OnDisable()
    {
        PlayerHealth.onPlayerDeath -= setPlayerDead;
    }

    public virtual void Start()
    {
        playerIsDead = false;
        inputHandler = GetComponent<InputHandler>();
        enemyParent = GameObject.Find("EnemyParent")?.transform;
        if (!enemyParent)
        {
            enemyParent = new GameObject("EnemyParent").transform;
        }

        allyParent = GameObject.Find("AllyParent")?.transform;
        if (!allyParent)
        {
            allyParent = new GameObject("AllyParent").transform;
        }

        InvokeRepeating("TryAttack", 0, attackRate);
    }

    public void setPlayerDead()
    {
        playerIsDead = true;
    }

    public void TryAttack()
    {
        if (enemyParent.childCount > 0 && !playerIsDead)
        {
            Attack(attackTarget);
        }
    }

    public void Attack(GameObject target)
    {
        GetComponent<SoundPlayer>().PlayAttackSound();
        Projectile fireball = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        Vector2 projectileDirection = inputHandler.GetAimDirection();
        fireball.Initialize(projectileDirection, projectileSpeed, projectileLifetime);
    }


}

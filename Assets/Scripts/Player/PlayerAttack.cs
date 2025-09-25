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

    public virtual void Start()
    {
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

    public void TryAttack()
    {

        Attack(attackTarget);


    }

    public void Attack(GameObject target)
    {
        Projectile fireball = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        Vector2 projectileDirection = inputHandler.GetAimDirection();
        fireball.Initialize(projectileDirection, projectileSpeed, projectileLifetime);
        GetComponent<SoundPlayer>().PlayAttackSound();
    }
    // public GameObject GetTarget()
    // {
    //     if (enemyParent)
    //     {
    //         foreach (Transform enemy in enemyParent)
    //         {
    //             if ((transform.position - enemy.position).sqrMagnitude < attackRange * attackRange)
    //             {
    //                 return enemy.gameObject;
    //             }
    //         }
    //     }

    //     return null;
    // }
}

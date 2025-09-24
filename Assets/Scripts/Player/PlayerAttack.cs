using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackTarget;
    public Transform enemyParent;
    public float attackRange;
    public float attackRate;
    public GameObject projectile;
    public float projectileSpeed;
    public float projectileLifetime;

    public virtual void Start()
    {
        InvokeRepeating("TryAttack", 0, attackRate);
    }

    public void TryAttack()
    {
        attackTarget = GetTarget();
        if (attackTarget)
        {
            Attack(attackTarget);
        }

    }

    public void Attack(GameObject target)
    {
        Projectile arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        Vector2 projectileDirection = (target.transform.position - transform.position).normalized;
        arrow.Initialize(projectileDirection, projectileSpeed, projectileLifetime);
    }

    public GameObject GetTarget()
    {
        foreach (Transform enemy in enemyParent)
        {
            if ((transform.position - enemy.position).sqrMagnitude < attackRange * attackRange)
            {
                return enemy.gameObject;
            }
        }

        return null;
    }
}

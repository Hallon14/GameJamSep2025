using UnityEngine;

public class RangedEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        base.Attack();
        Move();
        ShootArrow();


    }

    public void ShootArrow()
    {
        Projectile arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        Vector2 projectileDirection = (attackTarget.position - transform.position).normalized;
        arrow.Initialize(projectileDirection, projectileSpeed, projectileLifetime);
    }

    public void Move()
    {

        if ((attackTarget.position - transform.position).sqrMagnitude < (attackRange / 3) * (attackRange / 3))
        {
            rb2D.linearVelocity = -(attackTarget.position - transform.position).normalized * movementSpeed;
        }
        else if ((attackTarget.position - transform.position).sqrMagnitude > (attackRange / 2) * (attackRange / 2))
        {
            rb2D.linearVelocity = (attackTarget.position - transform.position).normalized * movementSpeed;
        }
        else
        {
            rb2D.linearVelocity = Vector2.zero;
        }

    }

}

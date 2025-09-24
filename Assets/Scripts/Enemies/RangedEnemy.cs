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

        ShootArrow();


    }

    public void ShootArrow()
    {
        Projectile arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        Vector2 projectileDirection = (attackTarget.position - transform.position).normalized;
        arrow.Initialize(projectileDirection, projectileSpeed, projectileLifetime);
    }



}

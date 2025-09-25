using UnityEngine;

public class MeleeEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        base.Attack();
        SwingSword();
    }

    public void SwingSword()
    {
        Vector2 projectileDirection = (attackTarget.position - transform.position).normalized;
        Projectile arrow = Instantiate(projectile, (Vector2)transform.position + projectileDirection, Quaternion.identity).GetComponent<Projectile>();
        arrow.Initialize(projectileDirection, projectileSpeed, projectileLifetime);

    }

}

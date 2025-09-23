using UnityEngine;

public class BowHandler : MonoBehaviour
{
    private Vector2 velocity;
    public Rigidbody2D rb2D;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 _direction, float _projectileSpeed, float _projectileLifetime)
    {
        velocity = _direction.normalized * _projectileSpeed;
        transform.right = _direction;
        Invoke("Die", _projectileLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.linearVelocity = velocity;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    /*public void ShootArrow()
    {
        Projectile arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        Vector2 projectileDirection = (attackTarget.position - transform.position).normalized;
        arrow.Initialize(projectileDirection, projectileSpeed, projectileLifetime);
    }*/
}

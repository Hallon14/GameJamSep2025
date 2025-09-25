using UnityEngine;

public class BowHandler : MonoBehaviour
{
    private bool canShoot = true;
    public float reloadTime = 3f;
    private Vector2 velocity;
    public Rigidbody2D rb2D;

    public GameObject projectile; // Assign your arrow prefab in the inspector
    public float projectileSpeed = 10f;
    public float projectileLifetime = 2f;
    public int volleyCount = 5; // Number of arrows in the volley
    public float volleySpread = 30f; // Total spread angle in degrees

    void OnEnable()
    {
        InputHandler.onVolley += ShootVolley;
    }

    void OnDisable()
    {
        InputHandler.onVolley -= ShootVolley;
    }

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

    void Update()
    {
        //rb2D.linearVelocity = velocity;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void ShootArrow(Vector2 direction)
    {
        if (!canShoot) return;
        canShoot = false;
        Projectile arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        arrow.Initialize(direction, projectileSpeed, projectileLifetime);
        Invoke(nameof(Reload), reloadTime);
    }


    private void Reload()
    {
        canShoot = true;
    }

    public void ShootVolley(Vector2 aimDirection)
    {
        if (aimDirection == Vector2.zero) aimDirection = Vector2.right;
        ShootArrow(aimDirection);
    }
}


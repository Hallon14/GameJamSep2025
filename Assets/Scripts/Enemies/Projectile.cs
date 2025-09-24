using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 velocity;
    public Rigidbody2D rb2D;
    public int damage;




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
}

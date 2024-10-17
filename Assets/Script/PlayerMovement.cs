using UnityEngine;

public class PlayerMovement : Entity
{
    // Fireball variables
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public KeyCode shootKey = KeyCode.Space;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    protected override void Start()
    {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    protected override void HandleInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        while (Input.GetKeyDown(shootKey) && Time.time >= nextFireTime)
        {
            ShootFireball();
            nextFireTime = Time.time + fireRate;
        }
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Player has died!");
        // You might want to add game over logic here
    }

    void ShootFireball()
    {
        if (fireballPrefab == null)
        {
            Debug.LogError("Fireball prefab is not assigned!");
            return;
        }

        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Vector2 shootDirection = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY")).normalized;

        if (shootDirection == Vector2.zero)
        {
            shootDirection = transform.right;  // Default to right if no movement
        }

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        fireball.GetComponent<Rigidbody2D>().velocity = shootDirection * fireballSpeed;
    }
}
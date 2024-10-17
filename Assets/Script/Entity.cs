using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float moveSpeed = 5f;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 movement;
    protected bool canMove = true;

    public int health = 100;
    public LayerMask solidObjectsLayer;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        HandleInput();
        UpdateAnimator();
    }

    protected virtual void FixedUpdate()
    {
        Move();
        CheckCollisions();
    }

    protected virtual void HandleInput()
    {
        // To be overridden in child classes
    }

    protected virtual void Move()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    protected virtual void UpdateAnimator()
    {
        if (movement != Vector2.zero)
        {
            animator.SetFloat("moveX", movement.x);
            animator.SetFloat("moveY", movement.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    protected virtual void CheckCollisions()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.2f, solidObjectsLayer);
        canMove = (hits.Length == 0);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        gameObject.SetActive(false);
    }
}
using UnityEngine;

public class Monster : Entity
{
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    public LayerMask playerLayer;

    private Transform player;

    protected override void Start()
    {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer("Monster");
        FindPlayer();
    }

    private void FindPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (playerCollider != null)
        {
            player = playerCollider.transform;
        }
        else
        {
            Debug.LogWarning("No player found within detection radius!");
        }
    }

    protected override void HandleInput()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;

            // Attack if in range
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
        else
        {
            // Stop moving if player is out of detection range
            movement = Vector2.zero;
            player = null; // Reset player reference when out of range
        }
    }

    private void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking the player!");
        lastAttackTime = Time.time;

        // Use layer-based collision detection for attack
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (playerCollider != null)
        {
            PlayerMovement playerMovement = playerCollider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.TakeDamage(10); // Assuming 10 damage per attack
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Monster has been defeated!");
        // Add any specific monster death logic here
    }

    // Visualization for detection and attack ranges in the Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
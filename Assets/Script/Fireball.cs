using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 20;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private GameObject shooter;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(GameObject shooter)
    {
        this.shooter = shooter;
        
        // Determine the target layer based on the shooter
        if (shooter.layer == LayerMask.NameToLayer("Player"))
        {
            targetLayer = LayerMask.GetMask("Monster");
        }
        else if (shooter.layer == LayerMask.NameToLayer("Monster"))
        {
            targetLayer = LayerMask.GetMask("Player");
        }
        else
        {
            Debug.LogError("Invalid shooter layer: " + LayerMask.LayerToName(shooter.layer));
            Destroy(gameObject);
        }

        // Set obstacle layer
        obstacleLayer = LayerMask.GetMask("SolidObject");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer.value) != 0)
        {
            // Hit target (player or monster)
            HandleHit(collision.gameObject);
            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & obstacleLayer.value) != 0)
        {
            // Hit obstacle (SolidObject)
            // You can add effects here if needed, like particle effects
            Destroy(gameObject);
        }
    }

    private void HandleHit(GameObject target)
    {
        Entity entity = target.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("Hit object does not have an Entity component: " + target.name);
        }
    }
}
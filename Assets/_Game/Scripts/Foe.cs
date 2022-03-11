using UnityEngine;

public abstract class Foe : MonoBehaviour
{
    [SerializeField] protected float currentHealth = 100;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected int collisionDamage = 25;

    private float m_Bound = 15.0f;
    protected FoeSpawner foeSpawner;

    public virtual void Init() { }

    protected virtual void Move()
    {
        if (transform.position.z > m_Bound || transform.position.z < -m_Bound)
        {
            Destroy();
        }
        transform.Translate(speed * Time.deltaTime * -Vector3.forward);
    }

    protected virtual void DoDamage(int damage) { }
    public virtual void TakeDamage(int damage)
    {
        if (currentHealth > 0) { currentHealth -= damage; }

        if (currentHealth <= 0) { currentHealth = 0; Destroy(); }
    }

    private void Destroy()
    {
        if (this is Enemy) { }
        else if (this is Obstacle) { }

        foeSpawner.DeSpawnFoe(this);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PhysicsLayers.PlayerLayer)
        {
            if (other.transform.parent.TryGetComponent(out Player player))
            {
                TakeDamage(player.CollisionDamage);
                player.TakeDamage(collisionDamage);
            }
        }
    }
}


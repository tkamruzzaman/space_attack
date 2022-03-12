using CodeMonkey.HealthSystemCM;
using System;
using UnityEngine;

public abstract class Foe : MonoBehaviour, IGetHealthSystem
{
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float speed = 5;
    [SerializeField] protected int collisionDamage = 25;

    private float m_Bound = 15.0f;
    protected FoeSpawner foeSpawner;

    protected HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnDead += HealthSystem_OnDead;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        if (this is Enemy) { ScoreManager.Instance.AddScore(10); }
        else if (this is Obstacle) { ScoreManager.Instance.AddScore(5); }

        Destroy();
    }

    public virtual void Init()
    {
        healthSystem.SetHealth(maxHealth);
    }

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
        healthSystem.Damage(damage);
    }

    private void Destroy()
    {
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

    public HealthSystem GetHealthSystem() => healthSystem;

    private void OnDestroy()
    {
        healthSystem.OnDead -= HealthSystem_OnDead;
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnHealed -= HealthSystem_OnHealed;
    }

}


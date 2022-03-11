using UnityEngine;

public class Projectile : MonoBehaviour
{
    /*[SerializeField]*/
    private float m_Bound = 10.0f;
    [SerializeField] private float m_Speed = 5.0f;

    private int m_Direction = 1;

    [SerializeField] private int m_DamageCapacity = 10;
    private ProjectileSpawner m_ProjectileSpawner;
    private ProjectileType m_ProjectileType;
    private TrailRenderer m_TrailRenderer;

    private void Awake()
    {
        m_TrailRenderer = transform.GetComponentInChildren<TrailRenderer>();
    }

    private void Start()
    {
        m_ProjectileSpawner = FindObjectOfType<ProjectileSpawner>();
        if (m_ProjectileSpawner == null)
        {
            Debug.LogError("Projectile Spawner is NULL");
        }
    }

    public void Init(ProjectileType projectileType)
    {
        m_TrailRenderer.Clear();
        m_ProjectileType = projectileType;

        switch (m_ProjectileType)
        {
            case ProjectileType.None: m_Direction = 0; break;
            case ProjectileType.PlayerProjectile: m_Direction = 1; break;
            case ProjectileType.FoeProjectile: m_Direction = -1; break;
        }
    }

    private void Update()
    {
        if (transform.position.z > m_Bound || transform.position.z < -m_Bound)
        {
            Destroy();
        }

        transform.Translate(m_Direction * m_Speed * Time.deltaTime * Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_ProjectileType == ProjectileType.PlayerProjectile)
        {
            if (other.gameObject.layer == PhysicsLayers.FoeLayer)
            {
                if (other.transform.parent.TryGetComponent(out Foe foe))
                {
                    foe.TakeDamage(m_DamageCapacity);
                }
                Destroy();
            }
            if (other.gameObject.layer == PhysicsLayers.ProjectileLayer)
            {
                if (other.transform.parent.TryGetComponent(out Projectile projectile))
                {
                    projectile.Destroy();
                }
                Destroy();
            }
        }
        if (m_ProjectileType == ProjectileType.FoeProjectile)
        {
            if (other.gameObject.layer == PhysicsLayers.PlayerLayer)
            {
                if (other.transform.parent.TryGetComponent(out Player player))
                {
                    player.TakeDamage(m_DamageCapacity);
                }
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        m_ProjectileSpawner.DeSpawnProjectile(this);
    }
}

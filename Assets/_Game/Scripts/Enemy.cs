using System.Collections;
using UnityEngine;

public class Enemy : Foe
{
    private ProjectileSpawner m_ProjectileSpawner;
    [SerializeField] private Transform m_ProjectileShoot;
    [Range(5.5f, 9.1f)] [SerializeField] private float m_FireRate = 6.0f;


    private void Start()
    {
        currentHealth = 100;
        speed = 10;

        foeSpawner = FindObjectOfType<FoeSpawner>();
        if (foeSpawner == null) { Debug.LogError("FoeSpawner is NULL"); }

        m_ProjectileSpawner = FindObjectOfType<ProjectileSpawner>();
        if (m_ProjectileSpawner == null) { Debug.LogError("Projectile Spawner is NULL"); }
    }

    public override void Init()
    {
        base.Init();
        StartCoroutine(IE_ShootProjectiles());
    }

    private void Update()
    {
        Move();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void DoDamage(int damage)
    {
        base.DoDamage(damage);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
    }

    public IEnumerator IE_ShootProjectiles()
    {
        yield return new WaitForSeconds(1);

        while (true)
        {
            Projectile projectile = m_ProjectileSpawner.SpawnProjectile();
            projectile.transform.SetPositionAndRotation(m_ProjectileShoot.position, Quaternion.identity);
            projectile.transform.SetParent(m_ProjectileSpawner.transform);
            projectile.Init(ProjectileType.FoeProjectile);
            float t = 1 - (m_FireRate * 0.1f);
            yield return new WaitForSeconds(t);
        }
    }
}


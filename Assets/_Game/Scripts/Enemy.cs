using System.Collections;
using UnityEngine;

public class Enemy : Foe // INHERITANCE
{
    private ProjectileSpawner m_ProjectileSpawner;
    [SerializeField] private Transform m_ProjectileShoot;
    [Range(1.0f, 5.0f)] [SerializeField] private float m_FireRate = 3.5f;

    private GameManager m_GameManager;
    private bool m_IsInGamePlay;

    [SerializeField] private Transform[] m_MeshTransforms;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        if (m_GameManager == null) { Debug.LogError("GameManager is NULL"); }
        m_GameManager.OnGameEnded += OnGameEnded;

        foeSpawner = FindObjectOfType<FoeSpawner>();
        if (foeSpawner == null) { Debug.LogError("FoeSpawner is NULL"); }

        m_ProjectileSpawner = FindObjectOfType<ProjectileSpawner>();
        if (m_ProjectileSpawner == null) { Debug.LogError("Projectile Spawner is NULL"); }
    }

    public override void Init()
    {
        base.Init();
        ActiveMeshRandomly();
        m_IsInGamePlay = true;
        StartCoroutine(IE_ShootProjectiles());
    }

    private void Update()
    {
        if (!m_IsInGamePlay) { return; }

        Move();
    }

    protected override void Move() // POLYMORPHISM
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
        base.OnTriggerEnter(other);
    }

    public IEnumerator IE_ShootProjectiles()
    {
        yield return new WaitForSeconds(1);

        while (m_IsInGamePlay)
        {
            Projectile projectile = m_ProjectileSpawner.SpawnProjectile();
            projectile.transform.SetPositionAndRotation(m_ProjectileShoot.position, Quaternion.identity);
            projectile.transform.SetParent(m_ProjectileSpawner.transform);
            projectile.Init(ProjectileType.FoeProjectile);
            float t = 1 - (m_FireRate * 0.1f);
            yield return new WaitForSeconds(t);
        }
    }

    private void OnGameEnded()
    {
        m_IsInGamePlay = false;
        StopAllCoroutines();
    }

    private void OnDestroy() => m_GameManager.OnGameEnded -= OnGameEnded;

    private void ActiveMeshRandomly()
    {
        foreach (Transform item in m_MeshTransforms)
        {
            item.gameObject.SetActive(false);
        }
        m_MeshTransforms[Random.Range(0, m_MeshTransforms.Length)].gameObject.SetActive(true);
    }

}


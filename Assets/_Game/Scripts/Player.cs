using CodeMonkey.HealthSystemCM;
using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IGetHealthSystem
{
    private float m_HorizontalInput;
    private float m_VerticalInput;

    [SerializeField] private Vector2 m_MovementRange;

    [Range(5, 15)] [SerializeField] private float m_TranslationSpeed = 10.0f;

    [SerializeField] private Transform m_ProjectileShoot;
    [SerializeField] private Transform m_MeshTranform;
    private GameManager m_GameManager;
    private ProjectileSpawner m_ProjectileSpawner;

    [SerializeField] private float m_MaxHealth = 100;
    [Range(7.5f, 9.1f)] [SerializeField] private float m_FireRate = 8.0f;

    [SerializeField] private int m_CollisionDamage = 50;
    [SerializeField] private ParticleSystem m_DeadParticle;
    public int CollisionDamage { get => m_CollisionDamage; private set => m_CollisionDamage = value; }

    private bool m_IsInGamePlay;

    private HealthSystem m_healthSystem;

    private void Awake()
    {
        m_healthSystem = new HealthSystem(m_MaxHealth);

        m_healthSystem.OnDead += HealthSystem_OnDead;
        m_healthSystem.OnDamaged += HealthSystem_OnDamaged;
        m_healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        if (m_GameManager == null) { Debug.LogError("GameManager is NULL"); }
        m_GameManager.OnGameEnded += OnGameEnded;

        m_ProjectileSpawner = FindObjectOfType<ProjectileSpawner>();
        if (m_ProjectileSpawner == null) { Debug.LogError("Projectile Spawner is NULL"); }
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        m_MeshTranform.gameObject.SetActive(false);
        m_DeadParticle.gameObject.SetActive(true);
        m_DeadParticle.Play();
        m_GameManager.DoGameOver();
    }

    private void Update()
    {
        if (!m_IsInGamePlay) { return; }

        Movement();
    }

    private void Movement()
    {
        m_HorizontalInput = Input.GetAxis("Horizontal");
        m_VerticalInput = Input.GetAxis("Vertical");

        m_HorizontalInput = m_HorizontalInput * m_TranslationSpeed * Time.deltaTime;
        m_VerticalInput = m_VerticalInput * m_TranslationSpeed * Time.deltaTime;

        if (transform.position.x < -m_MovementRange.x)
        {
            transform.position = new Vector3(-m_MovementRange.x, 0, transform.position.z);
        }
        if (transform.position.x > m_MovementRange.x)
        {
            transform.position = new Vector3(m_MovementRange.x, 0, transform.position.z);
        }
        if (transform.position.z < -m_MovementRange.y)
        {
            transform.position = new Vector3(transform.position.x, 0, -m_MovementRange.y);
        }
        if (transform.position.z > m_MovementRange.y)
        {
            transform.position = new Vector3(transform.position.x, 0, m_MovementRange.y);
        }

        transform.Translate(m_HorizontalInput, 0, m_VerticalInput);
    }

    public IEnumerator IE_ShootProjectiles()
    {
        m_IsInGamePlay = true;

        yield return new WaitForSeconds(1);

        while (m_IsInGamePlay)
        {
            Projectile projectile = m_ProjectileSpawner.SpawnProjectile();
            projectile.transform.SetPositionAndRotation(m_ProjectileShoot.position, Quaternion.identity);
            projectile.transform.SetParent(m_ProjectileSpawner.transform);
            projectile.Init(ProjectileType.PlayerProjectile);
            float t = 1 - (m_FireRate * 0.1f);
            yield return new WaitForSeconds(t);
        }
    }

    public void DoDamage(int damage) { }
    public void TakeDamage(int damage)
    {
        m_healthSystem.Damage(damage);
    }

    private void OnGameEnded()
    {
        m_IsInGamePlay = false;
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        m_GameManager.OnGameEnded -= OnGameEnded;

        m_healthSystem.OnDead -= HealthSystem_OnDead;
        m_healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        m_healthSystem.OnHealed -= HealthSystem_OnHealed;
    }

    public HealthSystem GetHealthSystem() => m_healthSystem;
}

using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float m_HorizontalInput;
    private float m_VerticalInput;

    [SerializeField] private Vector2 m_MovementRange;

    [Range(5, 15)] [SerializeField] private float m_TranslationSpeed = 10.0f;

    [SerializeField] private Transform m_ProjectileShoot;

    private GameManager m_GameManager;
    private ProjectileSpawner m_ProjectileSpawner;

    [SerializeField] private float m_CurrentHealth = 100;
    [Range(7.5f, 9.1f)] [SerializeField] private float m_FireRate = 8.0f;

    [SerializeField] private int m_CollisionDamage = 50;
    public int CollisionDamage { get => m_CollisionDamage; private set => m_CollisionDamage = value; }

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        if (m_GameManager == null) { Debug.LogError("GameManager is NULL"); }
        m_ProjectileSpawner = FindObjectOfType<ProjectileSpawner>();
        if (m_ProjectileSpawner == null) { Debug.LogError("Projectile Spawner is NULL"); }
    }

    private void Update()
    {
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
        yield return new WaitForSeconds(1);

        while (true)
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
        if (m_CurrentHealth > 0) { m_CurrentHealth -= damage; }

        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;
            m_GameManager.DoGameOver();
        }
    }
}

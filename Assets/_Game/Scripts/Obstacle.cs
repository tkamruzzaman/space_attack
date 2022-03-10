using UnityEngine;

public class Obstacle : Foe
{
    private GameManager m_GameManager;
    private bool m_IsInGamePlay;

    private void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        if (m_GameManager == null) { Debug.LogError("GameManager is NULL"); }
        m_GameManager.OnGameEnded += OnGameEnded;

        foeSpawner = FindObjectOfType<FoeSpawner>();
        if (foeSpawner == null) { Debug.LogError("FoeSpawner is NULL"); }
    }

    private void Update()
    {
        if (!m_IsInGamePlay) { return; }

        Move();
    }

    public override void Init()
    {
        base.Init();
        m_IsInGamePlay = true;
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
        base.OnTriggerEnter(other);
    }

    private void OnGameEnded()
    {
        m_IsInGamePlay = false;
    }

    private void OnDestroy() => m_GameManager.OnGameEnded -= OnGameEnded;

}

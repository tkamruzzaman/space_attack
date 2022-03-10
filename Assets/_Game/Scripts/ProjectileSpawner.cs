using UnityEngine;

public enum ProjectileType
{
    None = 0,
    PlayerProjectile = 1,
    FoeProjectile = 2,
}

public class ProjectileSpawner : PoolerBase<Projectile>
{
    [SerializeField] private Projectile m_ProjectilePrefab;

    private void Start() => InitPool(m_ProjectilePrefab); // Initialize the pool

    // Optionally override the setup components
    protected override void GetSetup(Projectile projectile)
    {
        base.GetSetup(projectile);
        //projectile.transform.position = Vector3.zero;
    }

    public Projectile SpawnProjectile() => Get(); // Pull from the pool

    public void DeSpawnProjectile(Projectile projectile) => Release(projectile); // Release back to the pool
}

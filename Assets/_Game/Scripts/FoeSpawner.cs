using System.Collections;
using UnityEngine;

public class FoeSpawner : MonoBehaviour
{
    [SerializeField] private Enemy m_EnemyPrefab;
    [SerializeField] private Obstacle m_ObstaclePrefab;
    [SerializeField] private float m_SpawnPosX = 15;
    [SerializeField] private float m_SpawnPosZ = 12;
    [Range(1, 9)]
    [SerializeField] private float m_SpawnRate = 5.0f;

    private Foe m_FoeToSpawn;

    private EnemySpawner m_EnemySpawner;
    private ObstacleSpawner m_ObstacleSpawner;

    private void Start()
    {
        m_EnemySpawner = new EnemySpawner(m_EnemyPrefab);
        m_ObstacleSpawner = new ObstacleSpawner(m_ObstaclePrefab);
    }

    public IEnumerator IE_SpawnFoes()
    {
        yield return new WaitForSeconds(1);

        while (true)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-m_SpawnPosX, m_SpawnPosX), 0, m_SpawnPosZ);
            m_FoeToSpawn = Random.Range(0, 2) == 0 ? m_EnemyPrefab : m_ObstaclePrefab;
            Foe foe = SpawnFoe(m_FoeToSpawn);
            foe.Init();
            foe.transform.SetPositionAndRotation(spawnPos, m_FoeToSpawn.transform.rotation);
            foe.transform.SetParent(transform);
            yield return new WaitForSeconds(1 - (m_SpawnRate * 0.1f));
        }
    }

    public Foe SpawnFoe(Foe foe)
    {
        if (foe is Enemy) { return m_EnemySpawner.GetEnemy(); }
        else if (foe is Obstacle) { return m_ObstacleSpawner.GetObstacle(); }
        Debug.LogError("Foe is NULL");
        return null;
    }

    public void DeSpawnFoe(Foe foe)
    {
        if (foe is Enemy) { m_EnemySpawner.ReleaseEnemy(foe as Enemy); }
        else if (foe is Obstacle) { m_ObstacleSpawner.ReleaseObstacle(foe as Obstacle); }
    }
}


public class EnemySpawner : PoolerBase<Enemy>
{
    public EnemySpawner(Enemy prefab) { InitPool(prefab); }
    public Enemy GetEnemy() => Get();
    public void ReleaseEnemy(Enemy enemy) => Release(enemy);
}

public class ObstacleSpawner : PoolerBase<Obstacle>
{
    public ObstacleSpawner(Obstacle prefab) { InitPool(prefab); }
    public Obstacle GetObstacle() => Get();
    public void ReleaseObstacle(Obstacle obstacle) => Release(obstacle);
}


using UnityEngine;

public class EnemySpawner : MonoBehaviour, IUpdatable
{
    [SerializeField] private WaveData waveData;
    [SerializeField] private float spawnDistance = 50f;
    [SerializeField] private float despawnDistance = 60f;

    private float elapsed;
    private float spawnTimer;
    private PlayerController player;
    private IEnemyFactory enemyFactory;
    private int currentWaveIndex = 0;
    private bool hasNextWave => currentWaveIndex + 1 < waveData.waves.Length;
    private bool hasStartedNextWave => elapsed >= waveData.waves[currentWaveIndex + 1].startTime;
    
    void Start()
    {
        GameplayManager.Instance.Register(this);
        player = GameplayManager.Instance.player;
        enemyFactory = new EnemyFactory(GameplayManager.Instance.gamePrefabs, transform, despawnDistance);
    }

    public void OnUpdate(float deltaTime)
    {
        elapsed += deltaTime;
        spawnTimer -= deltaTime;

        while (hasNextWave && hasStartedNextWave)
        {
            currentWaveIndex++;
        }
        if (spawnTimer <= 0f)
        {
            WaveData.Wave wave = waveData.waves[currentWaveIndex];
            SpawnEnemy(wave);
            spawnTimer = wave.spawnInterval;
        }
    }
    public void OnFixedUpdate(float deltaTime) {}
    void SpawnEnemy(WaveData.Wave wave)
    {
        EEnemyType type = waveData.PickEnemyType(wave);
        enemyFactory.Spawn(type, GetSpawnPosition());
    }

    Vector2 GetSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        
        return (Vector2)player.currentHero.transform.position + dir * spawnDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Camera.main.transform.position, spawnDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Camera.main.transform.position, despawnDistance);
    }
}
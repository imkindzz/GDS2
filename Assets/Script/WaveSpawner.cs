using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawn Points (choose 1+):")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Config")]
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private float timeBetweenSpawns = 0.15f;
    [SerializeField] private float timeBetweenWaves = 2f;
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool loopWaves = false;

    private int _aliveInWave;
    private bool _running;

    void Start()
    {
        if (autoStart) StartSpawning();
    }

    [ContextMenu("Start Spawning")]
    public void StartSpawning()
    {
        if (_running) return;
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[WaveSpawner] No spawn points assigned.");
            return;
        }
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        _running = true;

        do
        {
            for (int w = 0; w < waves.Count; w++)
            {
                var wave = waves[w];
                if (wave == null || wave.entries == null || wave.entries.Count == 0)
                    continue;

                // Spawn this wave
                yield return StartCoroutine(SpawnWave(wave));

                // Wait until all enemies in this wave are dead
                yield return new WaitUntil(() => _aliveInWave == 0);

                if (timeBetweenWaves > 0f)
                    yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        while (loopWaves);

        _running = false;
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        _aliveInWave = 0;

        foreach (var entry in wave.entries)
        {
            if (entry.prefab == null || entry.count <= 0) continue;

            for (int i = 0; i < entry.count; i++)
            {
                // Clamp index so it’s valid
                int chosenIndex = Mathf.Clamp(entry.spawnPointIndex, 0, spawnPoints.Length - 1);
                var point = spawnPoints[chosenIndex];

                GameObject go = Instantiate(entry.prefab, point.position, point.rotation);

                var status = go.GetComponent<EnemyStatus>();
                if (status != null)
                {
                    _aliveInWave++;
                    status.Died += HandleEnemyDied;
                }
                else
                {
                    Debug.LogWarning($"[WaveSpawner] Spawned enemy '{go.name}' has no EnemyStatus; it won't be tracked.");
                }

                if (timeBetweenSpawns > 0f)
                    yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }

    private void HandleEnemyDied(EnemyStatus status)
    {
        status.Died -= HandleEnemyDied;
        _aliveInWave = Mathf.Max(0, _aliveInWave - 1);
    }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public List<WaveEntry> entries = new List<WaveEntry>();
    }

    [System.Serializable]
    public class WaveEntry
    {
        public GameObject prefab;    // enemy prefab with EnemyStatus
        public int count = 5;
        public int spawnPointIndex = 0; // <— choose spawn point in Inspector
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Spawn Points (choose 1+):")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Wave Config")]
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private float timeBetweenSpawns = 0.15f; // delay between units in a wave
    [SerializeField] private float timeBetweenWaves = 2f;     // delay after a wave is cleared
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool loopWaves = false;          // optional looping

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
                Wave wave = waves[w];
                if (wave == null || wave.entries == null || wave.entries.Count == 0)
                    continue;

                // Spawn this wave
                yield return StartCoroutine(SpawnWave(wave));

                // Wait until all enemies in this wave are dead
                yield return new WaitUntil(() => _aliveInWave == 0);

                // Optional small breather
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

        // Round-robin through spawn points
        int spIndex = 0;

        foreach (var entry in wave.entries)
        {
            if (entry.prefab == null || entry.count <= 0) continue;

            for (int i = 0; i < entry.count; i++)
            {
                var point = spawnPoints[spIndex % spawnPoints.Length];
                spIndex++;

                GameObject go = Instantiate(entry.prefab, point.position, point.rotation);
                var hp = go.GetComponent<EnemyStatus>();
                if (hp != null)
                {
                    _aliveInWave++;
                    hp.OnDied += HandleEnemyDied;
                }
                else
                {
                    Debug.LogWarning($"[WaveSpawner] Spawned enemy '{go.name}' has no EnemyHealth; it won't be tracked.");
                }

                if (timeBetweenSpawns > 0f)
                    yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }

    private void HandleEnemyDied(EnemyStatus hp)
    {
        // Unsubscribe to be tidy (in case of pooled reuse later)
        hp.OnDied -= HandleEnemyDied;
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
        public GameObject prefab; // your enemy prefab
        public int count = 5;     // how many to spawn
    }
}

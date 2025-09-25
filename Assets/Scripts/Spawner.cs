using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyTypes;
    [Header("Spawn Timing")]
    [Tooltip("Seconds between spawns (initial)")] [SerializeField] private float spawnInterval = 5f;
    [Tooltip("Apply a single difficulty ramp after this many seconds (set <=0 to disable)")] [SerializeField] private float firstRampTime = 20f;
    [Tooltip("How much to subtract once at ramp time")] [SerializeField] private float rampIntervalDelta = 0.5f;
    [Tooltip("Never let spawn interval go below this value")] [SerializeField] private float minSpawnInterval = 2.5f;
    private bool rampApplied = false;
    private GameObject enemyParent;
    private GameObject allyParent;
    [Header("Optional Start Delay")] [Tooltip("If > 0, wait this many seconds before starting spawns.")]
    [SerializeField] private float startDelay = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        PlayerHealth.onPlayerDeath += DisableSpawner;
    }
    void OnDisable()
    {
        PlayerHealth.onPlayerDeath -= DisableSpawner;
    }

    void Start()
    {
        enemyParent = GameObject.Find("EnemyParent");
        if (!enemyParent)
        {
            enemyParent = new GameObject("EnemyParent");
        }

        allyParent = GameObject.Find("AllyParent");
        if (!allyParent)
        {
            allyParent = new GameObject("AllyParent");
        }

        if (startDelay > 0f && gameObject.CompareTag("LateSpawner"))
        {
            StartCoroutine(BeginAfterDelay());
        }
        else
        {
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
        }
    }

    public void DisableSpawner()
    {
        CancelInvoke();
    }

    public void TrySpawning()
    {

    }

    public void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemyTypes.Length);
        Instantiate(enemyTypes[enemyIndex], transform.position, Quaternion.identity, enemyParent.transform);
    }


    // Update is called once per frame
    void Update()
    {
        if (!rampApplied && firstRampTime > 0f && Time.timeSinceLevelLoad >= firstRampTime)
        {
            float newInterval = Mathf.Max(minSpawnInterval, spawnInterval - rampIntervalDelta);
            if (!Mathf.Approximately(newInterval, spawnInterval))
            {
                spawnInterval = newInterval;
                CancelInvoke();
                InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
            }
            rampApplied = true; // ensure this runs only once
        }
    }

    private System.Collections.IEnumerator BeginAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }
}

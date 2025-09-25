using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyTypes;
    private float spawnInterval =2f;
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
        //every 10 seconds gameobject exists, increase spawn rate by 0.5 seconds
        if (Time.timeSinceLevelLoad > 10f && spawnInterval > 0.5f)
        {
            spawnInterval -= 0.5f;
            CancelInvoke();
            InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
        }
    }

    private System.Collections.IEnumerator BeginAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }
}

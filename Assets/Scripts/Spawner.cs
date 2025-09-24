using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyTypes;
    public float spawnRate;
    private GameObject enemyParent;
    private GameObject allyParent;
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

        InvokeRepeating("SpawnEnemy", 0f, spawnRate);
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

    }
}

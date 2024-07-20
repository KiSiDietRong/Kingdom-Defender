using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;


    [Header("Attributes")]
    [SerializeField] private int baseEnemy = 8;
    [SerializeField] private float enemyPerSec = 0.5f;
    [SerializeField] private float timeBetweenWave = 5f;
    [SerializeField] private float difficultyScallingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemyAlive;
    private int enemyLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroy);
    }
    private void Start()
    {
        StartCoroutine(StartWave());
    }
    private void Update()
    {
        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= (1f / enemyPerSec) && enemyLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemyLeftToSpawn--;
            enemyAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemyAlive == 0 && enemyLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
    private void EnemyDestroy()
    {
        enemyAlive--;
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWave);
        isSpawning = true;
        enemyLeftToSpawn = baseEnemy;
    }
    
    private void SpawnEnemy()
    {
        GameObject preFabsSpawn = enemyPrefabs[0];
        Instantiate(preFabsSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemy * Mathf.Pow(currentWave, difficultyScallingFactor));
    }
}

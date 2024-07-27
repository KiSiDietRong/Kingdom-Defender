using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Button startButton;
    [SerializeField] private Text countdownText;
    [SerializeField] private Text waveCounterText;

    [Header("Attributes")]
    [SerializeField] private float enemyPerSec = 0.5f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Wave Settings")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private int totalWaves = 7;

    private int currentWaveIndex = 0;
    private int enemyAlive;
    private int enemyLeftToSpawn;
    private bool isSpawning = false;
    private bool waveCountdownRunning = false;

    private float timeSinceLastSpawn;
    private float timeRemaining;

    private void Awake()
    {
        if (waves.Length > 0)
        {
            timeRemaining = waves[currentWaveIndex].timeUntilNextWave;
        }
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartWaveSequence);
        ShowStartButton();
        UpdateWaveCounterText();
        UpdateCountdownText(timeRemaining);
    }

    private void Update()
    {
        if (isSpawning)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= (1f / enemyPerSec) && enemyLeftToSpawn > 0)
            {
                SpawnEnemy();
                enemyLeftToSpawn--;
                enemyAlive++;
                timeSinceLastSpawn = 0f;
            }

            if (enemyAlive == 0 && enemyLeftToSpawn == 0 && !waveCountdownRunning)
            {
                EndWave();
            }
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        if (currentWaveIndex < waves.Length - 1)
        {
            // Move to the next wave directly after finishing current wave
            currentWaveIndex++;
            StartWaveCountdown(); // Start the countdown for the next wave
        }
        else
        {
            Debug.Log("All waves completed!");
            // Optionally, you can handle the end of all waves here
        }
    }

    private void StartNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            enemyLeftToSpawn = currentWave.enemyCounts[0];
            timeRemaining = currentWave.timeUntilNextWave;
            UpdateWaveCounterText();
            UpdateCountdownText(timeRemaining);
            isSpawning = true;

            // Reset countdown for the next wave
            StopCoroutine("WaveDurationCountdown");
            waveCountdownRunning = false;

            // Start spawning enemies immediately
            StartCoroutine(WaveDurationCountdown());
        }
    }

    private void StartWaveCountdown()
    {
        if (!waveCountdownRunning)
        {
            waveCountdownRunning = true;
            StartCoroutine(CountdownCoroutine());
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timeRemaining > 0)
        {
            UpdateCountdownText(timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timeRemaining = 0;
        UpdateCountdownText(timeRemaining);
        waveCountdownRunning = false;

        // Move to the next wave
        StartNextWave();
    }

    private IEnumerator WaveDurationCountdown()
    {
        // Start countdown for the wave's duration
        while (timeRemaining > 0)
        {
            UpdateCountdownText(timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timeRemaining = 0;
        UpdateCountdownText(timeRemaining);

        // Move to the next wave
        EndWave();
    }

    public void StartWaveSequence()
    {
        startButton.gameObject.SetActive(false);
        StartNextWave(); // Start the first wave immediately
    }

    private void EnemyDestroy()
    {
        enemyAlive--;
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private void UpdateCountdownText(float timeRemaining)
    {
        countdownText.text = $"Time left: {Mathf.CeilToInt(timeRemaining)}s";
    }

    private void UpdateWaveCounterText()
    {
        waveCounterText.text = $"Wave: {currentWaveIndex + 1}/{totalWaves}";
    }

    [System.Serializable]
    public class Wave
    {
        public int[] enemyCounts;
        public float timeUntilNextWave;
    }

    private void ShowStartButton()
    {
        startButton.gameObject.SetActive(true);
    }
}

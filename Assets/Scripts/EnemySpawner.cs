using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Button startButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Text countdownText;

    [Header("Attributes")]
    [SerializeField] private float enemyPerSec = 0.5f;
    [SerializeField] private float timeBetweenWave = 5f;
    [SerializeField] private float startDelay = 3f;
    [SerializeField] private float skipButtonDelay = 5f;
    [SerializeField] private float skipDelay = 2f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Wave Settings")]
    [SerializeField] private Wave[] waves;

    private int currentWaveIndex = 0;
    private int enemyAlive;
    private int enemyLeftToSpawn;
    private bool isSpawning = false;

    private float timeUntilNextWave;
    private float timeSinceLastSpawn;

    private Coroutine startDelayCoroutine;
    private Coroutine skipButtonDelayCoroutine;
    private Coroutine skipDelayCoroutine;
    private Coroutine waveCountdownCoroutine;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroy);
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartWaveSequence);
        skipButton.onClick.AddListener(SkipWave);
        ShowStartButton();
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

            if (enemyAlive == 0 && enemyLeftToSpawn == 0)
            {
                EndWave();
            }
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWaveIndex++;
        if (currentWaveIndex < waves.Length)
        {
            StartCoroutine(StartSkipButtonDelay());
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    private IEnumerator StartSkipButtonDelay()
    {
        ShowSkipButton();
        yield return new WaitForSeconds(skipButtonDelay);
        HideSkipButton();
        StartNextWave();
    }

    public void StartWaveSequence()
    {
        startButton.gameObject.SetActive(false);
        startDelayCoroutine = StartCoroutine(StartDelayCoroutine());
    }

    private IEnumerator StartDelayCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        StartNextWave();
    }

    private void StartNextWave()
    {
        isSpawning = true;
        Wave currentWave = waves[currentWaveIndex];
        enemyLeftToSpawn = currentWave.numEnemies;
        timeSinceLastSpawn = 0f;
        waveCountdownCoroutine = StartCoroutine(StartWaveCountdown());
    }

    private IEnumerator StartWaveCountdown()
    {
        timeUntilNextWave = timeBetweenWave;
        UpdateCountdownText();
        while (timeUntilNextWave > 0)
        {
            yield return new WaitForSeconds(1f);
            timeUntilNextWave--;
            UpdateCountdownText();
        }
        StartNextWave();
    }

    public void SkipWave()
    {
        HideSkipButton();
        StopCoroutine(waveCountdownCoroutine);
        EndWave();
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

    private void UpdateCountdownText()
    {
        countdownText.text = "Next wave in: " + timeUntilNextWave.ToString() + "s";
    }

    [System.Serializable]
    public class Wave
    {
        public int numEnemies;
    }

    private void ShowStartButton()
    {
        startButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(false);
    }

    private void ShowSkipButton()
    {
        skipButton.gameObject.SetActive(true);
    }

    private void HideSkipButton()
    {
        skipButton.gameObject.SetActive(false);
    }
}

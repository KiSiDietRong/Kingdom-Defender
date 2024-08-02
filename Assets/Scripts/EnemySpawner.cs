<<<<<<< HEAD
﻿using System.Collections;
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> Toàn
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
<<<<<<< HEAD
    [SerializeField] private Button startButton;
    [SerializeField] private Text countdownText;
    [SerializeField] private Text waveCounterText;
=======

>>>>>>> Toàn

    [Header("Attributes")]
    [SerializeField] private int baseEnemy = 8;
    [SerializeField] private float enemyPerSec = 0.5f;
<<<<<<< HEAD
=======
    [SerializeField] private float timeBetweenWave = 5f;
    [SerializeField] private float difficultyScallingFactor = 0.75f;
>>>>>>> Toàn

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

<<<<<<< HEAD
    [Header("Wave Settings")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private int totalWaves = 7;

    [Header("Main House Settings")]
    [SerializeField] private BaseHealth baseHealth;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite grayStar; // Gray star image
    [SerializeField] private Sprite goldStar; // Gold star image

    [Header("Victory Screen")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private int currentWaveIndex = 0;
=======
    private int currentWave = 1;
    private float timeSinceLastSpawn;
>>>>>>> Toàn
    private int enemyAlive;
    private int enemyLeftToSpawn;
    private bool isSpawning = false;
    private bool waveCountdownRunning = false;

<<<<<<< HEAD
    private float timeSinceLastSpawn;
    private float timeRemaining;

=======
>>>>>>> Toàn
    private void Awake()
    {
        if (waves.Length > 0)
        {
            timeRemaining = waves[currentWaveIndex].timeUntilNextWave;
        }
    }
    private void Start()
    {
<<<<<<< HEAD
        startButton.onClick.AddListener(StartWaveSequence);
        ShowStartButton();
        UpdateWaveCounterText();
        UpdateCountdownText(timeRemaining);

        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        victoryPanel.SetActive(false);
=======
        StartCoroutine(StartWave());
>>>>>>> Toàn
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

<<<<<<< HEAD
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
=======
        if (enemyAlive == 0 && enemyLeftToSpawn == 0)
        {
            EndWave();
>>>>>>> Toàn
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
<<<<<<< HEAD

        if (currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            StartWaveCountdown();
        }
        else
        {
            StartCoroutine(ShowVictoryScreenAfterDelay(3f));
        }
    }

    private IEnumerator ShowVictoryScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0; // Pause the game
        ShowVictoryScreen();
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

            StopCoroutine("WaveDurationCountdown");
            waveCountdownRunning = false;

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

        StartNextWave();
    }

    private IEnumerator WaveDurationCountdown()
    {
        while (timeRemaining > 0)
        {
            UpdateCountdownText(timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timeRemaining = 0;
        UpdateCountdownText(timeRemaining);

        EndWave();
    }

    public void StartWaveSequence()
    {
        startButton.gameObject.SetActive(false);
        StartNextWave();
    }

=======
        currentWave++;
        StartCoroutine(StartWave());
    }
>>>>>>> Toàn
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
<<<<<<< HEAD

    private void UpdateCountdownText(float timeRemaining)
    {
        countdownText.text = $"Time left: {Mathf.CeilToInt(timeRemaining)}s";
    }

    private void UpdateWaveCounterText()
    {
        waveCounterText.text = $"Wave: {currentWaveIndex + 1}/{totalWaves}";
    }

    private void ShowVictoryScreen()
    {
        victoryPanel.SetActive(true);

        int stars = CalculateStars(baseHealth.currentHeart);
        UpdateStarsDisplay(stars);
    }

    private int CalculateStars(int remainingHealth)
    {
        if (remainingHealth >= 15)
        {
            return 3;
        }
        else if (remainingHealth >= 13)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private void UpdateStarsDisplay(int stars)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < stars)
            {
                starImages[i].sprite = goldStar; // Set to gold star
            }
            else
            {
                starImages[i].sprite = grayStar; // Set to gray star
            }
        }
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

    private void RestartGame()
    {
        Time.timeScale = 1; // Unpause the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1; // Unpause the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Menu");
=======
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemy * Mathf.Pow(currentWave, difficultyScallingFactor));
>>>>>>> Toàn
    }
}

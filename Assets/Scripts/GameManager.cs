using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject batteryPrefab;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] TextMeshProUGUI gameTitle;

    [SerializeField] Button startBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] Button restartBtn;
    [SerializeField] Button easyModeBtn;
    [SerializeField] Button hardModeBtn;

    SpawnManager spawnManager;
    PauseManager pauseManager;

    int coinCount = 0;
    int coinsNeededToWin;
    int timer;
    public int health;
    private int maxHealth;

    public bool isGameOver = false;
    public bool isGameStarted = false;

    public void OnEasyModeSelected()
    {
        timer = 60;
        health = 5;
        maxHealth = 5;
        coinsNeededToWin = 10;

        StartGame();
    }

    public void OnHardModeSelected()
    {
        timer = 60;
        health = 3;
        maxHealth = 3;
        coinsNeededToWin = 15;

        StartGame();
    }

    public IEnumerator ShowHelpText()
    {
        int helpTextTimer = 8; // Ennyi ideig fog megjelenni a help szöveg
        helpText.gameObject.SetActive(true);

        while (helpTextTimer > 0)
        {
            yield return new WaitForSeconds(1.0f); // Biztosítja, hogy 1 mp-et várjon mielõtt kivon 1-et a jelenlegi értékbõl
            helpTextTimer--;
        }

        helpText.gameObject.SetActive(false);
    }

    public void PickUpBattery()
    {
        health = maxHealth;

        healthText.SetText("Életerõ: " + health);

        SpawnNewBattery();
    }

    void SpawnNewBattery()
    {
        float randomX = Random.Range(-25.0f, 25.0f);
        float randomZ = Random.Range(-14.0f, 14.0f);

        Vector3 offset = new Vector3(randomX, 0, randomZ);

        Instantiate(batteryPrefab,
            coinPrefab.transform.position + offset,
            batteryPrefab.transform.rotation);
    }

    public void TakeDamage()
    {
        health--;

        healthText.SetText("Életerõ: " + health);

        if (health <= 0)
        {
            GameOver(false);
        }
    }

    IEnumerator Countdown()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timer--;

            if (timer > 9)
            {
                timerText.SetText("0:" + timer.ToString());
            }
            else 
            {
                timerText.SetText("0:0" + timer.ToString());
            }
        }

        GameOver(false);
    }

    public void SpawnNewCoin()
    {
        float randomX = Random.Range(-25.0f, 25.0f);
        float randomZ = Random.Range(-14.0f, 14.0f);

        Vector3 offset = new Vector3(randomX, 0, randomZ);

        Instantiate(coinPrefab,
            player.transform.position + offset,
            coinPrefab.transform.rotation);

        UpdateCoinCounter();
    }

    void UpdateCoinCounter()
    {
        coinCount++;
        scoreText.SetText("Pontszám: " + coinCount.ToString() + "/" + coinsNeededToWin.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        // Játék elemeinek elrejtése
        healthText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        helpText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        easyModeBtn.gameObject.SetActive(false);
        hardModeBtn.gameObject.SetActive(false);
        restartBtn.gameObject.SetActive(false);

        pauseManager = GameObject.Find("PauseManager").GetComponent<PauseManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        // Time freeze
        Time.timeScale = 0f;
    }

    public void OnStartBtnSelected()
    {
        // Menü elemeinek elrejtése
        gameTitle.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);

        // Menü elemek megjelenítése
        easyModeBtn.gameObject.SetActive(true);
        hardModeBtn.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        // Time unfreeze
        Time.timeScale = 1f;

        // Menü elemek elrejtése
        easyModeBtn.gameObject.SetActive(false);
        hardModeBtn.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartBtn.gameObject.SetActive(false);

        // Játék elemek megjelenítése
        healthText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        // Állapot resetelése
        isGameStarted = true;
        isGameOver = false;
        coinCount = 0;

        // Játékos resetelése
        player.GetComponent<PlayerController>().canMove = true;

        scoreText.SetText("Pontszám: " + coinCount.ToString() + "/" + coinsNeededToWin.ToString());

        StartCoroutine(Countdown());
        spawnManager.StartSpawning();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted || isGameOver) return;

        if (coinCount == coinsNeededToWin)
        {
            GameOver(true);
        }

        // Játék szüneteltetése
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseManager.TogglePause();
        }

        // Kilépés a Fõmenübe
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
        }
    }

    void GameOver(bool isWin)
    {
        if (isWin)
        {
            gameOverText.SetText("Gratulálok, nyertél! :)");
        }

        restartBtn.gameObject.SetActive(true);

        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        player.GetComponent<PlayerController>().canMove = false;
        //enemyPrefab.GetComponent<EnemyController>().canMove = false;

        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
        {
            enemy.canMove = false;
        }

        StopAllCoroutines();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

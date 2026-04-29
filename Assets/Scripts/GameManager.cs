using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    int coinCount = 0;
    int timer = 60;
    public int health = 5;
    private int maxHealth = 5;

    public bool isGameOver = false;

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
        scoreText.SetText("Pontszám: " + coinCount.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        helpText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        isGameOver = false;

        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        if (coinCount == 10)
        {
            GameOver(true);
        }
    }

    void GameOver(bool isWin)
    {
        if (isWin)
        {
            gameOverText.SetText("Gratulálok, nyertél! :)");
        }

        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        player.GetComponent<PlayerController>().canMove = false;
        enemyPrefab.GetComponent<EnemyController>().canMove = false;
        StopAllCoroutines();
    }
}

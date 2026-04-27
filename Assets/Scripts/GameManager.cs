using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject coinPrefab;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI gameOverText;

    int coinCount = 0;
    int timer = 60;

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
        gameOverText.gameObject.SetActive(false);

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

        gameOverText.gameObject.SetActive(true);
        player.GetComponent<PlayerController>().canMove = false;
        StopAllCoroutines();
    }
}

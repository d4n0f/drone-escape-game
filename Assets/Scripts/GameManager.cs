using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject coinPrefab;

    int coinCount = 0;

    public void SpawnNewCoin()
    {
        float randomX = Random.Range(-20.0f, 20.0f);
        float randomZ = Random.Range(-12.0f, 12.0f);

        Vector3 offset = new Vector3(randomX, 0, randomZ);

        Instantiate(coinPrefab,
            player.transform.position + offset,
            coinPrefab.transform.rotation);

        UpdateCoinCounter();
    }

    void UpdateCoinCounter()
    {
        coinCount++;
        //dogHP.SetText("HP: " + doghealthpoints.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

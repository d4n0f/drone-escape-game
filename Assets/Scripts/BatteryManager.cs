using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (gameManager.health == 5)
        {
            gameManager.StartCoroutine(gameManager.ShowHelpText());
            return;
        }

        gameManager.PickUpBattery();
        Destroy(this.gameObject);
    }
}

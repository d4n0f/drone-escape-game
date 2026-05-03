using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] AudioClip coinPickUp;

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

        AudioSource.PlayClipAtPoint(coinPickUp, transform.position, 5f);
        gameManager.SpawnNewCoin();
        Destroy(this.gameObject);
    }
}

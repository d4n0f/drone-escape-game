using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    GameManager gameManager;

    [SerializeField] float speed = 8.0f;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (!gameManager.isGameStarted) return;

        FollowPlayer();
    }

    // Update is called once per frame
    void FollowPlayer()
    {
        if (gameManager.isGameOver) return;

        Vector3 dir = (player.transform.position - transform.position).normalized;
        Vector3 newPos = rb.position + dir * speed * Time.fixedDeltaTime;

        newPos.y = 5;

        rb.MovePosition(newPos);
    }
}

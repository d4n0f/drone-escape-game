using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    CharacterController characterController;
    GameManager gameManager;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isGameStarted) return;

        ControllPlayer();
    }

    void ControllPlayer()
    {
        if (!canMove) { return; }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        float tilt = Input.GetAxis("Horizontal") * 20f;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, -tilt);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
        }

        characterController.Move(direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.TakeDamage();

            collision.gameObject.GetComponent<EnemyController>().Die();

            //Destroy(collision.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    Vector3 offset = new Vector3(0, 32.5f, 0);

    void Start()
    {

    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}

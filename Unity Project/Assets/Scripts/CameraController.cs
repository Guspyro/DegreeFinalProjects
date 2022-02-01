using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = PlayerController.instance.transform;
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}

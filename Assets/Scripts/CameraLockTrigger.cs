using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLockTrigger : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CinemachineVirtualCamera>();    
        target = cam.Follow;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            cam.Follow = transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            cam.Follow = target;
    }
    
}

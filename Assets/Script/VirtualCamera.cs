using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) SetCameraTarget(player.transform);
    }
    public void SetCameraTarget(Transform target)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = target;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

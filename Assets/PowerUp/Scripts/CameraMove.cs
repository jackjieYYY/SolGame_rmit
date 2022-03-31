using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform CameraTransform;
    public bool PlayerAlive = true;
    [SerializeField] Vector3 offSet;

    void Update()
    {
        if (PlayerAlive == true)
        {
            CameraTransform.position = PlayerTransform.position + offSet;
        }
    }
}
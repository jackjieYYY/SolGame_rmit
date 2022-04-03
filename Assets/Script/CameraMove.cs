using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform CameraTransform;
    public Transform BgTransform;
    public bool PlayerAlive = true;
    [SerializeField] Vector3 offSet;
    [SerializeField] Vector3 BgOffSet;

    void Update()
    {
        if (PlayerTransform)
        {
            CameraTransform.position = PlayerTransform.position + offSet;
            BgTransform.position = PlayerTransform.position + BgOffSet;
        }
    }
}
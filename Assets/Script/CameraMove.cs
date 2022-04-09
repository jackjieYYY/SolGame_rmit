using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform PlayerTransform;
    public Transform CameraTransform;
    public Transform BgTransform;
    public Transform StarField;
    [SerializeField] Vector3 offSet;
    [SerializeField] Vector3 BgOffSet;
    [SerializeField] Vector3 StarOffset;

    private void Start()
    {
        StarField.position = PlayerTransform.position + StarOffset;
        BgTransform.position = PlayerTransform.position + BgOffSet;
        
    }
    void Update()
    {
        if (PlayerTransform)
        {
            CameraTransform.position = PlayerTransform.position + offSet;
            //BgTransform.position = PlayerTransform.position + BgOffSet;
            //
            
        }
    }
}
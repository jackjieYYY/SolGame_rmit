using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //deviation = transform.position - player.position; // 初始物体与相机的偏移量=相机的位置 - 移动物体的偏移量
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1, 0);
    }
}

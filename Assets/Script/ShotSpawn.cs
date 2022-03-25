using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawn : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //Debug.Log(transform.rotation);
        //transform.rotation = Quaternion.Euler(parent.transform.rotation.x, parent.transform.rotation.y, parent.transform.rotation.z);
    }
}

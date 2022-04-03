using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    public bool reverse;
    Vector3 rotate;
    float mod;
    // Start is called before the first frame update
    void Start()
    {
        mod = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, mod, 0);
        if (reverse)
        {
            mod = mod - 0.03f;
        }
        else
        {
            mod = mod + 0.03f;
        }
        //Debug.Log(transform.rotation);
    }
}

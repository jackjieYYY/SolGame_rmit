using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltRemove : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        PlayerController ship = collision.GetComponent<PlayerController>();
        if (ship != null)
        {
                Destroy(gameObject);

        }
    }
}

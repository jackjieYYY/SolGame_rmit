using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastUpgrade : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        PlayerController ship = collision.GetComponent<PlayerController>();
        if(ship != null)
        {
                Debug.Log("Changed to weapon 1");
                ship.ChangeWeapon(1);
                Destroy(gameObject);
        }
    }
}

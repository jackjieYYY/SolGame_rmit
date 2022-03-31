using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltUpgrade : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        PlayerController ship = collision.GetComponent<PlayerController>();
        if (ship != null)
        {
            Debug.Log("Changed to weapon 2");
            ship.ChangeWeapon(2);
            Destroy(gameObject);
        }
    }
}

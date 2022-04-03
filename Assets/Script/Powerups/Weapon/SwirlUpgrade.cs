using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlUpgrade : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        PlayerController ship = collision.GetComponent<PlayerController>();
        if (ship != null)
        {
                Debug.Log("changed to weapon 3");
                ship.ChangeWeapon(3);
                Destroy(gameObject);
        }
    }
}

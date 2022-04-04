using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        PlayerController ship = collision.GetComponent<PlayerController>();
        if (ship != null)
        {
            if (!ship.Invincibility)
            {
                ship.ChangeHealth(-1);
            }
        }
    }
}

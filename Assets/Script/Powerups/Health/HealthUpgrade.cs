using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : MonoBehaviour
{
    public int health = 1;
    private void OnTriggerEnter(Collider collision)
    {
        PlayerController ship = collision.GetComponent<PlayerController>();
        if (ship != null)
        {
            if (ship.Health < ship.Maximumhealth)
            {
                
                ship.ChangeHealth(health);
                Debug.Log("Increased health by 1, new health: " + ship.Health);
                Destroy(gameObject);
            }
        }
    }
}

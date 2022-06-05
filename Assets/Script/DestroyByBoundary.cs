using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerController ship = other.GetComponent<PlayerController>();
        if (ship != null)
        {
            ship.ChangeHealth(-100);
            //if the ship is on zero health, destroy it as well
        }
        else
        {
            Destroy(other.gameObject);
            UnityEngine.Debug.Log("He hit da wall!!!!!!!!!!");
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



    }
}

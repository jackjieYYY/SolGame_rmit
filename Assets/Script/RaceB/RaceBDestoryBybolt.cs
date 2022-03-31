using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceBDestoryBybolt : MonoBehaviour
{

    public GameObject Explosion;
    int health; //The health of the raceB object
    public int maximumHealth = 5; // the maximum health of the RaceB object
    //int BoltCount = 0;


    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject); //Destroy the bullet fired from the ship
        if (health == 0)
        {
            var temp = Instantiate(Explosion, transform.position, transform.rotation); //Spawn in the broken version
            Destroy(temp, 2f);
            Destroy(gameObject);
            GameObject.Find("GameController").GetComponent<GameController>().RaceBDroid_Destory(this.gameObject);
        }
        else
        {
            health--;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maximumHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private class MonoStub : MonoBehaviour
    {

    }

}

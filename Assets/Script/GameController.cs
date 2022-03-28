using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject RaceBDroid;
    public Vector3 spawnValue;
    int maxSpawnWaitTime = 10;
    // Start is called before the first frame update
    void Start()
    {
        // open a new Thread
        StartCoroutine(spawnRaceBDroid());
    }

    /// <summary>
    /// IEnumerator and startCoroutine just like a Thread.
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnRaceBDroid()
    {
        while (true)
        {

            Vector3 spawnPosition = new Vector3(Random.Range(6, 9), 0, Random.Range(-3, -5));
            Instantiate(RaceBDroid, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5, maxSpawnWaitTime));
        }

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}

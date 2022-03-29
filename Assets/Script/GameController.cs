using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject RaceBDroid;
    List<GameObject> RaceBDroidList = new List<GameObject> ();
    public Vector3 spawnValue;
    int maxSpawnWaitTime = 10;
    int maxRaceBDroid = 3;
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
            if (RaceBDroidList.Count < maxRaceBDroid)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(6, 9), 4, Random.Range(-3, -5));
                var temp = Instantiate(RaceBDroid, spawnPosition, Quaternion.identity);
                RaceBDroidList.Add(temp.gameObject);
            }
            yield return new WaitForSeconds(Random.Range(5, maxSpawnWaitTime));
        }


    }


    public void RaceBDroid_Destory(GameObject RaceBDroid)
    {
        RaceBDroidList.Remove(RaceBDroid);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

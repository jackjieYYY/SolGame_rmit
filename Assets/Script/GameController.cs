using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject RaceBDroid;
    public GameObject RaceADroid;
    List<GameObject> RaceBDroidList = new List<GameObject>();
    List<GameObject> RaceADroidList = new List<GameObject>();
    int maxDroid = 3;
    float DroidSpeed = 0.01f;

    int level = 1;
    public Vector3 spawnValue;
    int maxSpawnWaitTime = 5;

    // Start is called before the first frame update

    //-------------------UIsetting start --------------------

    int score = 0;
    public Text scoreText;
    public Text gameTime;
    public Text gameLevel;
    int hour;
    int minute;
    int second;
    float timeSpend = 0f;
    //-------------------UIsetting end--------------------



    void Start()
    {
        // open a new Thread
        StartCoroutine(spawnDroid());
    }

    /// <summary>
    /// IEnumerator and startCoroutine just like a Thread.
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnDroid()
    {

        while (true)
        {
            // raceA spawn
            if (RaceADroidList.Count < maxDroid * 3)
            {
                for (int i = 0; i < maxDroid; i++)
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-9, -6), 4, Random.Range(-5, 5));
                    var raceA = Instantiate(RaceADroid, spawnPosition, Quaternion.identity);
                    RaceADroidList.Add(raceA.gameObject);
                }
            }
            // raceB spawn
            if (RaceBDroidList.Count < maxDroid)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(6, 9), 4, Random.Range(-3, -5));
                var raceB = Instantiate(RaceBDroid, spawnPosition, Quaternion.identity);
                RaceBDroidList.Add(raceB.gameObject);

            }
            yield return new WaitForSeconds(Random.Range(2, maxSpawnWaitTime));
        }


    }

    public void RaceADroid_Destory(GameObject RaceADroid)
    {
        RaceADroidList.Remove(RaceADroid);
    }
    public void RaceBDroid_Destory(GameObject RaceBDroid)
    {
        RaceBDroidList.Remove(RaceBDroid);
    }

    // Update is called once per frame
    void Update()
    {
        gameTimeUpdate();
        gameLevelUpdate();
        LevelUpdate();
    }

    public void updateScore()
    {
        scoreText.text = string.Format("Score: {0}", score.ToString());
    }



    public void addScore(int value)
    {
        score += value;
        updateScore();
    }

    public void gameTimeUpdate()
    {
        timeSpend += Time.deltaTime;
        hour = (int)timeSpend / 3600;
        minute = ((int)timeSpend - hour * 3600) / 60;
        second = (int)timeSpend - hour * 3600 - minute * 60;
        gameTime.text = string.Format("Time:{0:D2}:{1:D2}", minute, second);
    }

    public void gameLevelUpdate()
    {
        gameLevel.text = string.Format("Level: {0}", level.ToString());
    }

    public void LevelUpdate()
    {
        if (score > level * 5)
        {
            level++;
            maxDroid++;
            DroidSpeed = DroidSpeed;
        }
    }

    public float getDroidSpeed()
    {
        return DroidSpeed;
    }
}




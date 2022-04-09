using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject RaceBDroid;
    public GameObject RaceADroid;
    List<GameObject> RaceBDroidList = new List<GameObject>();
    List<GameObject> RaceADroidList = new List<GameObject>();
    GameObject ship;
    public PlayerController shipController;

    int maxDroid = 3;
    float DroidSpeed = 0.01f;

    int level = 1;
    public Vector3 spawnValue;

    int maxSpawnWaitTime = 5;

    public Text GameOverText;
    private bool isGameOver;
    public Text RestartText;
    private bool needRestart;

    // Start is called before the first frame update


    //-------------------UIsetting start --------------------

    int score = 0;
    public Text scoreText;
    public Text gameTime;
    public Text gameLevel;
    public Text healthText;
    public Text invincibilityText;
    int hour;
    int minute;
    int second;
    float timeSpend = 0f;
    
    // health of ship information
    int health;

    //-------------------UIsetting end--------------------



    void Start()
    {
        GameOverText.text = "";
        isGameOver = false;
        needRestart = false;
        // open a new Thread
        StartCoroutine(spawnDroid());

        ship = GameObject.Find("Player");
        if (ship != null)
        {
            shipController = ship.GetComponent<PlayerController>();
        }
            
    }

    /// <summary>
    /// IEnumerator and startCoroutine just like a Thread.
    /// </summary>
    /// <returns></returns>
    IEnumerator spawnDroid()
    {

        while (true)
        {
            if (isGameOver)
            {
                yield return null;
            }
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
        RestartCheck();
        if (isGameOver)
            return;
        gameTimeUpdate();
        gameLevelUpdate();
        LevelUpdate();
        HealthUpdate();
        InvincibilityUpdate();
    }

    public void updateScore()
    {
        if (isGameOver)
            return;
        scoreText.text = string.Format("Score: {0}", score.ToString());
    }

    public void HealthUpdate()
    {
        if (isGameOver)
            return;
        if (GameObject.Find("Player") != null)
        {
            healthText.text = string.Format("Health: {0}", shipController.Health);
        }
        //Debug.Log(shipController.Health);
    }
    public void InvincibilityUpdate()
    {
        if (isGameOver)
            return;
        if (GameObject.Find("Player") != null)
        {
            invincibilityText.text = string.Format("Invincible: {0}", shipController.Invincibility);
        }
    }

    public void RestartCheck()
    {
        if (needRestart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Application.LoadLevel(Application.loadedLevel);
                SceneManager.LoadScene(0);
            }
        }
    }

    public void addScore(int value)
    {
        score += value;
        updateScore();
    }

    public void gameTimeUpdate()
    {
        if (isGameOver)
            return;
        timeSpend += Time.deltaTime;
        hour = (int)timeSpend / 3600;
        minute = ((int)timeSpend - hour * 3600) / 60;
        second = (int)timeSpend - hour * 3600 - minute * 60;
        gameTime.text = string.Format("Time:{0:D2}:{1:D2}", minute, second);
    }

    public void gameLevelUpdate()
    {
        if (isGameOver)
            return;
        gameLevel.text = string.Format("Level: {0}", level.ToString());
    }

    public void LevelUpdate()
    {
        if (isGameOver)
            return;
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
    public bool IsGameOver()
    {
        return isGameOver;
    }
    public void GameOver()
    {
        isGameOver = true;
        needRestart = true;
        GameOverText.text = "Game Over";
        RestartText.text = "Press Space to Restart";
    }


}




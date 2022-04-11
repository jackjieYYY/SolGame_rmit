using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boidController : MonoBehaviour
{
    public int scoreForRescue = 1;
    public int scoreForLoss = -1;
    private GameController gameController;

    public GameObject deathExplosion;
    public AudioClip rescue;

    AudioSource Rescue;

    private void Start()
    {
        Rescue = AddAudio(false, false, 0.3f);
        Rescue.clip = rescue;
    }

    void OnTriggerEnter(Collider collision)
    {
        var _gameController = GameObject.Find("GameController");
        if (_gameController != null)
        {
            gameController = _gameController.GetComponent<GameController>();
        }
        if (collision.name == "RaceA_Droid(Clone)" || collision.name == "RaceB_Droid(Clone)")
        {
            //if we touch a droid, destroy and lose score
            gameController.addScore(scoreForLoss);
            //Instantiate(deathExplosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            Debug.Log("Destroyed by droid");
        }
        else if(collision.name == "Player")
        {
            //if we touch a droid, destroy but add score
            gameController.addScore(scoreForRescue);
            //AudioSource.PlayClipAtPoint(rescue, this.gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    public AudioSource AddAudio(bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;

        return newAudio;
    }
}

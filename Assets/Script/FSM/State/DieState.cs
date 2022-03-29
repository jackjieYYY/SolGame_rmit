using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DieState : IState
{
    FSM m_Fsm;
    GameObject gameObject;
    GameObject deathExplosion;
    GameObject dieAnimationObject;

    float dieAnimationTime;
    float dieAnimationDestoryTime = 2f;


    public DieState(FSM fsm, GameObject _gameObject, GameObject _deathExplosion)
    {
        m_Fsm = fsm; 
        gameObject = _gameObject;
        deathExplosion = _deathExplosion;
        

    }
    public void OnEnter()   //  The method that should be performed to enter this state
    {
        dieAnimationTime = Time.time;
        //Spawn in the broken version
        dieAnimationObject = MonoSub.Instantiate(deathExplosion, gameObject.transform.position, gameObject.transform.rotation);
    }
    public void OnUpdate() //The method that should be executed to maintain this state
    {
        MonoSub.Destroy(gameObject);
        MonoSub.Destroy(dieAnimationObject,2f);
        GameObject.Find("GameController").GetComponent<GameController>().RaceBDroid_Destory(this.gameObject);
    }
    public void OnExit() //The method that should be executed to exit this state
    {

    }
    private class MonoSub : MonoBehaviour
    {

    }
}


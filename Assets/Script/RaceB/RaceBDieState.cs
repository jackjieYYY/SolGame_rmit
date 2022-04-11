using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RaceBDieState : IState
{
    FSM m_Fsm;
    GameObject gameObject;
    GameObject deathExplosion;
    GameObject dieAnimationObject;

    float dieAnimationTime;
    float dieAnimationDestoryTime = 2f;


    public RaceBDieState(FSM fsm, GameObject _gameObject, GameObject _deathExplosion)
    {
        m_Fsm = fsm; 
        gameObject = _gameObject;
        deathExplosion = _deathExplosion;
        

    }
    public void OnEnter()   //  The method that should be performed to enter this state
    {

    }
    public void OnUpdate() //The method that should be executed to maintain this state
    {
        var dieAnimationObject = MonoSub.Instantiate(deathExplosion, gameObject.transform.position, gameObject.transform.rotation);
        MonoSub.Destroy(gameObject);
        MonoSub.Destroy(dieAnimationObject, 2f);
        GameObject.Find("GameController").GetComponent<GameController>().RaceBDroid_Destory(this.gameObject);
    }
    public void OnExit() //The method that should be executed to exit this state
    {

    }
    private class MonoSub : MonoBehaviour
    {

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnterState : IState
{
    private FSM m_FSM;
    private Transform m_transform;
    GameObject m_gameObject;
    public GameObject spawnExplosion;
    private GameController gameController;

    Vector3 localScale;

    public EnterState(FSM fsm, GameObject _gameObject)
    {
        m_FSM = fsm;
        m_gameObject = _gameObject;
        m_transform = m_gameObject.transform;

        var _gameController = GameObject.Find("GameController");
        if (_gameController != null)
        {
            gameController = _gameController.GetComponent<GameController>();
        }

    }
    public EnterState()
    {

    }

    public void setLocalScale(Vector3 _vector3)
    {
        localScale = _vector3;
    }

    public void OnEnter()   //  The method that should be performed to enter this state
    {

    }
    public void OnUpdate() //The method that should be executed to maintain this state
    {
        
        m_transform.position = Vector3.MoveTowards(m_transform.position, new Vector3(m_transform.position.x,0, m_transform.position.z), 0.05f);
        m_transform.localScale = Vector3.MoveTowards(m_transform.localScale, localScale, 0.01f);

        if (m_transform.position.y == 0 && m_transform.localScale == localScale)
        {
            m_FSM.TransitionState(StateType.SpawnAnimation);

        }

    }
    public void OnExit() //The method that should be executed to exit this state
    {
        // call OnExit() method by FSM
    }

    private class MonoSub : MonoBehaviour
    {

    }
}
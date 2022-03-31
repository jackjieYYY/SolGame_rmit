using Assets.Script.Astar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChaseState : IState
{
    private GameController gameController;
    GameObject player;
    GameObject m_GameObject;
    FSM m_FSM;
    List<Vector3> path;
    float speed = 0.01f;
    private float nextFindPathTime;
    private float FindPathTimeRate = 1f;

    public ChaseState(FSM fsm,GameObject _gameObject)
    {
        player = GameObject.Find("Player");
        m_GameObject = _gameObject;
        m_FSM = fsm;
        var _gameController = GameObject.Find("GameController");
        if (_gameController != null)
        {
            gameController = _gameController.GetComponent<GameController>();
        }
    }



    public void OnEnter()   //  The method that should be performed to enter this state
    {

        
    }
    public void OnUpdate() //The method that should be executed to maintain this state
    {

        if (Time.time > nextFindPathTime)
        {
            nextFindPathTime = Time.time + FindPathTimeRate;
            try
            {
                TryGetPath(player.GetComponent<Transform>().position);
            }
            catch (Exception)
            {
                Debug.Log("Error on Paht Finder");
            }
        }


        if (path != null)
        {
            speed = gameController.getDroidSpeed();
            FollowPath();
        }
    }
    public void OnExit() //The method that should be executed to exit this state
    {
        Debug.Log("I am ChaseState. OnExit()");
    }



    void TryGetPath(Vector3 end)
    {
        PathManager.Request(m_GameObject.transform.position, end, onPathFound);
    }

    void onPathFound(List<Vector3> _path, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            this.path = _path;
        }
    }
    void FollowPath()
    {
        if (path == null)
            return;
        if (path.Count == 0)
            return;
        Vector3 currentWayPoint = path[0];
        m_GameObject.transform.position = Vector3.MoveTowards(m_GameObject.transform.position, currentWayPoint, speed);
        if (m_GameObject.transform.position == currentWayPoint)
        {
            path.RemoveAt(0);
        }
    }











}



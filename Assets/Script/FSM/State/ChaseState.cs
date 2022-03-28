using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChaseState : IState
{
    FSM m_FSM;
    public ChaseState(FSM fsm)
    {
        m_FSM = fsm;
    }


    public void OnEnter()   //  The method that should be performed to enter this state
    {
        Debug.Log("I am ChaseState. OnEnter()");
    }
    public void OnUpdate() //The method that should be executed to maintain this state
    {
        Debug.Log("I am ChaseState. OnUpdate()");
    }
    public void OnExit() //The method that should be executed to exit this state
    {
        Debug.Log("I am ChaseState. OnExit()");
    }
}


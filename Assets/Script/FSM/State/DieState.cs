using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DieState : IState
{
    FSM m_Fsm;
    public DieState(FSM fsm)
    {
        m_Fsm = fsm;
    }
    public void OnEnter()   //  The method that should be performed to enter this state
    {
        Debug.Log("I am DieState. OnEnter()");
    }
    public void OnUpdate() //The method that should be executed to maintain this state
    {
        Debug.Log("I am DieState. OnUpdate()");
    }
    public void OnExit() //The method that should be executed to exit this state
    {
        Debug.Log("I am DieState. OnExit()");
    }
}


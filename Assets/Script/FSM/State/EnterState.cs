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
    private Rigidbody m_Rigidbody;

    public EnterState(FSM fsm,Transform transform, Rigidbody rigidbody)
    {
        m_FSM = fsm;
        m_Rigidbody = rigidbody;
        m_transform = transform;
    }
    public EnterState()
    {

    }

    public void OnEnter()   //  The method that should be performed to enter this state
    {
        Debug.Log("I am EnterState. OnEnter()");
    }
    public void OnUpdate() //The method that should be executed to maintain this state
    {
        Debug.Log("I am EnterState. OnUpdate()");
        m_FSM.TransitionState(StateType.Chase);

    }
    public void OnExit() //The method that should be executed to exit this state
    {
        Debug.Log("I am EnterState. OnExit()");
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController : MonoBehaviour
{
    private FSM m_Fsm;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;
    private void Awake()
    {
        m_Fsm = new FSM();
        m_Fsm.AddState(StateType.Enter, new EnterState(m_Fsm,m_Transform, m_Rigidbody));
        m_Fsm.AddState(StateType.Chase,new ChaseState(m_Fsm));
        m_Fsm.AddState(StateType.Die, new DieState(m_Fsm));
        m_Fsm.TransitionState(StateType.Enter);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Fsm.onTick();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController : MonoBehaviour
{
    private FSM m_Fsm;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;

    public GameObject deathExplosion;
    public GameObject spawnAnimation;

    int HP = 2;
    int hitByBoltCount = 0;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();


        m_Fsm = new FSM();
        m_Fsm.AddState(StateType.Enter, new EnterState(m_Fsm,gameObject));
        m_Fsm.AddState(StateType.SpawnAnimation, new SpawnAnimationState(m_Fsm, gameObject, spawnAnimation));
        m_Fsm.AddState(StateType.Chase,new ChaseState(m_Fsm,gameObject));
        m_Fsm.AddState(StateType.Die, new DieState(m_Fsm,gameObject,deathExplosion));
        m_Fsm.TransitionState(StateType.Enter);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Fsm.OnUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        hitByBoltCount++;
        if (this.hitByBoltCount > this.HP)
        {
            m_Fsm.TransitionState(StateType.Die);
        }
    }

}

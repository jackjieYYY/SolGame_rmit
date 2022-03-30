using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceB_FSMController : MonoBehaviour
{
    private FSM m_Fsm;

    public GameObject deathExplosion;
    public GameObject spawnAnimation;

    int HP = 2;
    int hitByBoltCount = 0;
    int score = 2;
    private GameController gameController;

    RandomRotator randomRotator;

    private void Awake()
    {

        m_Fsm = new FSM();
        randomRotator = new RandomRotator(gameObject.GetComponent<Rigidbody>());
        randomRotator.setRotation(new Vector3(0,0, 1), 1f);
        m_Fsm.AddState(StateType.Enter, new EnterState(m_Fsm,gameObject));
        m_Fsm.AddState(StateType.SpawnAnimation, new SpawnAnimationState(m_Fsm, gameObject, spawnAnimation));
        m_Fsm.AddState(StateType.Chase,new ChaseState(m_Fsm,gameObject));
        m_Fsm.AddState(StateType.Die, new DieState(m_Fsm,gameObject,deathExplosion));
        m_Fsm.TransitionState(StateType.Enter);


        var _gameController = GameObject.Find("GameController");
        if (_gameController != null)
        {
            gameController = _gameController.GetComponent<GameController>();
        }
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
            gameController.addScore(score);
            m_Fsm.TransitionState(StateType.Die);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceA_FSMController : MonoBehaviour
{
    private FSM m_Fsm;

    public GameObject deathExplosion;
    public GameObject spawnAnimation;

    int HP = 1;
    int hitByBoltCount = 0;
    int score = 1;
    private GameController gameController;
    RandomRotator randomRotator;

    private void Awake()
    {

        m_Fsm = new FSM();
        randomRotator = new RandomRotator(gameObject.GetComponent<Rigidbody>());
        randomRotator.setRotation(new Vector3(1, 1, 1), 1f);

        var enterState = new EnterState(m_Fsm, gameObject);
        enterState.setLocalScale(new Vector3(0.2f, 0.2f, 0.2f));
        m_Fsm.AddState(StateType.Enter,enterState);

        m_Fsm.AddState(StateType.SpawnAnimation, new SpawnAnimationState(m_Fsm, gameObject, spawnAnimation));
        m_Fsm.AddState(StateType.Chase,new ChaseState(m_Fsm,gameObject));
        m_Fsm.AddState(StateType.Die, new RaceADieState(m_Fsm,gameObject,deathExplosion));
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

    void OnTriggerEnter(Collider collision)
    {

        //Create a controller to check if the collision object is the spaceship
        PlayerController ship = collision.GetComponent<PlayerController>();

        // Assign the proper amount of damage for each weapon
        int damage = checkWeaponDamage(collision);
        //int damage
        //Check which weapon was used on the drone

        Debug.Log("Triggered: " + collision);

        // If ship is null, then the object is not the ship
        if (ship != null)
        {
            ship.ChangeHealth(-1);
            //if the ship is on zero health, destroy it as well
            if (ship.Health <= 0)
            {
                Destroy(collision.gameObject);
            }
        }
        else
        {
            //if the collision object is not the ship, it should be destroyed on contact
            Destroy(collision.gameObject);
        }

        //Regardless of what hits the drone, it should take damage
        HP = HP - damage;
        if (HP <= 0)
        {
            gameController.addScore(score);
            m_Fsm.TransitionState(StateType.Die);
        }

    }

    int checkWeaponDamage(Collider collision)
    {
        //check the associated components
        BlastMover blast = collision.GetComponent<BlastMover>();
        BoltMover bolt = collision.GetComponent<BoltMover>();
        SwirlMover swirl = collision.GetComponent<SwirlMover>();

        int damage;

        // Check which weapon was used
        if (blast != null)
        {
            damage = 1;
        }
        else if (bolt != null)
        {
            damage = 2;
        }
        else if (swirl != null)
        {
            damage = 3;
        }
        else
        {
            damage = 1;
        }

        return damage;
    }

}

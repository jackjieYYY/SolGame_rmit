using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceB_FSMController : MonoBehaviour
{
    private FSM m_Fsm;
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;

    public GameObject deathExplosion;
    public GameObject spawnAnimation;

    public int HP = 3;
    private GameController gameController;
    RandomRotator randomRotator;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();


        m_Fsm = new FSM();
        randomRotator = new RandomRotator(gameObject.GetComponent<Rigidbody>());
        randomRotator.setRotation(new Vector3(0, 0, 1), 1f);

        var enterState = new EnterState(m_Fsm, gameObject);
        enterState.setLocalScale(new Vector3(0.1f, 0.1f, 0.1f));
        m_Fsm.AddState(StateType.Enter, enterState);

        m_Fsm.AddState(StateType.SpawnAnimation, new SpawnAnimationState(m_Fsm, gameObject, spawnAnimation));
        m_Fsm.AddState(StateType.Chase, new ChaseState(m_Fsm, gameObject));
        m_Fsm.AddState(StateType.Die, new RaceBDieState(m_Fsm, gameObject, deathExplosion));
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

    // Upon trigger a hit, the drone shoud lose health. If the object that hit the drone is 
    // is the player ship, the player should take damage, and explode if his health is zero
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

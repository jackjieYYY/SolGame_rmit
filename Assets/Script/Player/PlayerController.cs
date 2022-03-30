using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    Rigidbody m_Rigidbody;

    private float nextFile;
    public float fileRate;

    public GameObject shot;
    public Transform shotSpawn;
    public Boundary boundary;


    float thrust = 0.001f; // How long we have to hold to get to max speed (higher quicker)
    float maxThrust = 0.01f; // Max speed
    float spaceDrag = 0.95f; // How we slow down naturally
    float acceleratorCooloff = 0.93f; // How our accelerator cools off
    float rotationSpeed = 0.1f; // How quick we turn

    // Internal calc vars
    float angle = 0.0f;
    float acceleration;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFile)
        {
            nextFile = Time.time + fileRate;
            Debug.Log("father shotSpawn.transform.position " + shotSpawn.transform.position);
            GameObject temp = Instantiate(shot, shotSpawn.transform.position, Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90, Vector3.down));
            Destroy(temp, 5f);
        }
    }

    void FixedUpdate()
    {


        // Get our controller input and translate it to thrust / rotation           
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && acceleration < maxThrust)
            acceleration += thrust;

        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && acceleration > -maxThrust)
            acceleration -= thrust;

        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
            angle -= rotationSpeed;

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
            angle += rotationSpeed;


        // Physics calculation
        velocity.x += acceleration * Mathf.Cos(angle);
        velocity.y += acceleration * Mathf.Sin(angle);
        velocity *= spaceDrag;
        acceleration *= acceleratorCooloff;


        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(m_Rigidbody.position.x + velocity.x, boundary.xMin, boundary.xMax),
            0f,
            Mathf.Clamp(m_Rigidbody.position.z + velocity.y, boundary.zMin, boundary.zMax)
            );
        GetComponent<Rigidbody>().rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg + 90, Vector3.down);

    }

}

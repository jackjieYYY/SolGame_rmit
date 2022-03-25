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
    public float acceleration;
    float maxSpeed = 0.05f;
    float speed = 0;
    public float tilt;

    private float nextFile;
    public float fileRate;

    public GameObject shot;
    public Transform shotSpawn;
    public Boundary boundary;

    private float mouseAngle;
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void getMouseAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.z = 0;
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        this.mouseAngle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg;
    }


    // Update is called once per frame
    void Update()
    {
        getMouseAngle();
        if (Input.GetButton("Fire1") && Time.time > nextFile)
        {
            nextFile = Time.time + fileRate;
            Debug.Log("father shotSpawn.transform.position " + shotSpawn.transform.position);
            //GameObject temp = Instantiate(shot, shotSpawn.transform.position, Quaternion.Euler(90f, mouseAngle + 90f, 0f));
            
            GameObject temp = Instantiate(shot, shotSpawn.transform.position, Quaternion.Euler(0f, mouseAngle, transform.rotation.z));
            Destroy(temp, 5f);
        }
    }

    void FixedUpdate()
    {
        m_Rigidbody.rotation = Quaternion.Euler(0f, this.mouseAngle, 0f);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (speed < maxSpeed)
            {
                speed += acceleration;
            }
            //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.velocity += transform.forward  * speed;
        }
        else{
            if (Mathf.Abs(speed) > 0)
            {

                speed += (-acceleration) * Mathf.Sign(speed);
                m_Rigidbody.velocity += transform.forward * (-speed);
            }
            if(Mathf.Abs(speed) < 0.01f)
            {
                speed = 0;
                m_Rigidbody.velocity = new Vector3(0, 0, 0);
            }

        }



        //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)



        /*
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            m_Rigidbody.velocity = new Vector3(0, 0, 0);

        }
        */


        GetComponent<Rigidbody>().position = new Vector3(
            Mathf.Clamp(m_Rigidbody.position.x, boundary.xMin, boundary.xMax),
            0f,
            Mathf.Clamp(m_Rigidbody.position.z, boundary.zMin, boundary.zMax)
            );

    }

}

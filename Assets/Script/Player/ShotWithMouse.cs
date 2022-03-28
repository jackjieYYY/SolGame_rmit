using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotWithMouse : MonoBehaviour
{

    private float nextFile;
    private float fileRate = 0.5f;

    public GameObject shot;
    public Transform shotSpawn;

    private float mouseAngle;
    // Start is called before the first frame update

    // Start is called before the first frame update
    void Start()
    {
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
            GameObject temp = Instantiate(shot, shotSpawn.transform.position, Quaternion.Euler(0f, mouseAngle, transform.rotation.z));
            Destroy(temp, 10f);
        }
    }

    void FixedUpdate()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rigidbody;
    private Vector3 initialVelocity;
    private float drag = 0f, mass = 10f, angularDrag = 0.1f;
    private float rotateX, rotateY, rotateZ, velocityX, velocityZ;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.drag = this.drag;
        _rigidbody.mass = this.mass;
        _rigidbody.angularDrag = this.angularDrag;

        velocityX = Random.Range(-3f, 3f);
        velocityZ = Random.Range(-3f, 3f);
        initialVelocity = new Vector3(velocityX, 0.0f, velocityZ);
        _rigidbody.velocity = initialVelocity;

        rotateX = Random.Range(-1f, 1f);
        rotateY = Random.Range(-1f, 1f);
        rotateZ = Random.Range(-1f, 1f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotateX, rotateY , rotateZ);
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }
}

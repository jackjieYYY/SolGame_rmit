using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BoidManager : MonoBehaviour
{
    Collider m_collider;
    public Collider AgentCollider { get { return m_collider; } }
    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
}

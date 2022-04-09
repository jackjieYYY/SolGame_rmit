using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator
{
    float rotation;
    Rigidbody m_rigidbody;
    Vector3 vector3;
    public RandomRotator(Rigidbody _rigidbody)
    {
        m_rigidbody = _rigidbody;
    }

    public void setRotation(Vector3 _vector3,float _rotation)
    {
        rotation = _rotation;
        vector3 = _vector3;
        m_rigidbody.angularVelocity = vector3 * rotation;
    }

}

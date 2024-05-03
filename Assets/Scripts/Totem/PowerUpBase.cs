using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    //[SerializeField] float _speedRotation = 30.0f;
    [SerializeField] Vector3 _angleRotation;

    void Update()
    {
        transform.Rotate(_angleRotation.x * Time.deltaTime, _angleRotation.y * Time.deltaTime, _angleRotation.z * Time.deltaTime);
    }
}

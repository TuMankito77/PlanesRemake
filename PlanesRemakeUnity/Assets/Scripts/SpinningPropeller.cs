using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPropeller : MonoBehaviour
{
    public float spinningSpeed=100; // speed at which the propeller spins 

    void Update()
    {
        // makes rotate the propeller 
        transform.Rotate(Vector3.forward, Time.deltaTime * -spinningSpeed);
    }
}

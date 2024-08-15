using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBoster : MonoBehaviour
{
    private bool rotate = false;
    private float speed=500;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
            transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        rotate = true;
    }
}

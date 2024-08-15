using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private Rigidbody cubeRb;
    private float impulse=10;
    private float speed=10;
    private float horizontal;
    // Start is called before the first frame update
    void Start()
    {
        cubeRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        Debug.Log(horizontal);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontal);
    }
    private void OnTriggerEnter(Collider other)
    {
        float timer = 2;
        while (timer > 0)
            cubeRb.AddForce(Vector3.left * impulse, ForceMode.Impulse);
            timer -= Time.deltaTime;
        cubeRb.AddForce(Vector3.left * -impulse, ForceMode.Impulse);
    }
}

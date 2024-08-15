using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMovement : MonoBehaviour
{
    private Player player;

    public float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.getGameOver())
            transform.Rotate(Vector3.up, Time.deltaTime * speed);
    }
}

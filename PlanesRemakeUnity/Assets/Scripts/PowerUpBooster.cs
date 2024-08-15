using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBooster : MonoBehaviour
{
    private Player player;

    public AudioClip powerUpClip;
    private float speed = 10f;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        if(!player.getGameOver())
            transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x < -150)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Player controller = other.GetComponent<Player>();
        if (controller != null)
        {
            controller.changeBoost(true, 5);
            controller.PlaySound(powerUpClip);
            Destroy(gameObject);
        }
    }
}

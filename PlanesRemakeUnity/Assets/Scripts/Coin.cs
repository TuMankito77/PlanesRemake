using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Player player;

    public AudioClip coinClip;
    public float speed = 10;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        if(!player.getGameOver())
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (transform.position.x < -150)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Player controller = other.GetComponent<Player>();
        if(controller != null)
        {
            controller.PlaySound(coinClip);
            CoinTextScript.coinAmount += 1;
            Destroy(gameObject);
        }
    }
}

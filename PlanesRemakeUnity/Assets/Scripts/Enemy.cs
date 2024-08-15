using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private Player player;
    public float speed = 15;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!player.getGameOver())
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (transform.position.x < -150)
            Destroy(gameObject);
    }
    /*private void OnTriggerEnter(Collider other)
    {
        Player controller = other.GetComponent<Player>();
        if (controller != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }*/
}

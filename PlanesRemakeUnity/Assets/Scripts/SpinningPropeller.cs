using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningPropeller : MonoBehaviour
{
    private Player player;
    private MeshRenderer propellerMeshRend;
    public float spinningSpeed=100; // speed at which the propeller spins 

    void Start()
    {
        player = GameObject.Find("Player(Clone)").GetComponent<Player>();
        propellerMeshRend = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        // makes rotate the propeller 
        transform.Rotate(Vector3.forward, Time.deltaTime * -spinningSpeed);
        //destroys the propeller when the game finishes
        if (player != null && player.getGameOver())
            Destroy(gameObject);

    }
}

using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    public float forwardSpeed = 10f;  // speed at which the wall will go forward 
    public float waggingSpeed = 10f;  // speed at which the wall will go up and down
    private bool up = true; // variable to control if the wall goes up or down 

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        waggingSpeed = Random.Range(10,20); // generates a random speed for the wall to go up and down 

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.getGameOver())
        {
            // moves the wall forward
            transform.Translate(Vector3.left * forwardSpeed * Time.deltaTime);
            // boundaries for the wall to go up and down 
            if (transform.position.y > -13)
                up = false;
            if (transform.position.y < -27)
                up = true;
            // moves the wall up or down depending on the boundary
            if (up)
                transform.Translate(Vector3.up * waggingSpeed * Time.deltaTime);
            if(!up)
                transform.Translate(Vector3.up * -waggingSpeed * Time.deltaTime);
            // destroys the wall if it's out of the player's view 
            if (transform.position.x < -150)
                Destroy(gameObject);
        }
    }
    // increases the wallsPassed variable on the player's script 
    private void OnTriggerEnter(Collider other)
    {
        Player controller = other.GetComponent<Player>();
        if (controller != null)
        {
            WallTextScript.wallAmount += 1;
        }
    }
}

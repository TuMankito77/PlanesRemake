using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public AudioClip crashClip;
    public ParticleSystem crashParticle;
    public ParticleSystem coinParticle;
    private AudioSource audioSource;
    private MeshRenderer playerMeshRend;

    //variables for the touch controls
    private Vector2 centerPos;
    private Vector2 movePos;
    private Vector2 direction;
    public RectTransform circle;
    public RectTransform outerCircle;


    public float speed = 10;
    private float horizontal;
    private float vertical;
    private float boostTimer = 0;
    private bool boost = false;
    private bool gameOver = false;
    private float menuTimer = 5;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMeshRend = this.gameObject.GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // gets the horizontal and the vertical inputs from the player 
        TouchControls();
        // controls the movement boundary in the x and y axis 
        constraints();
        // activates the boost for 5 seconds
        if (boost)
        {
            speed = 10 * 3;
            if (boostTimer > 0)
                boostTimer -= Time.deltaTime;
            else
                boost = false;
        }
        else
            speed = 10;

        if (gameOver)
            if (menuTimer > 0)
                menuTimer -= Time.deltaTime;
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // controls that the player don't go out of the player's view 
    public void constraints()
    {
        // constraint for the y axis on the positive direction 
        if (transform.position.y > 10)
        {
            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }
        // constraint for the y axis on the nagative direction
        if (transform.position.y < -13)
        {
            transform.position = new Vector3(transform.position.x, -13, transform.position.z);
        }
        // constraint for the x axis on the positive direction
        if (transform.position.x > 28)
        {
            transform.position = new Vector3(28, transform.position.y, transform.position.z);
        }
        // constraint for the x axis on the negative direction
        if (transform.position.x < -28)
        {
            transform.position = new Vector3(-28, transform.position.y, transform.position.z);
        }
    }
    //activates the booster 
    public void changeBoost(bool activated, float timer)
    {
        boost = activated;
        boostTimer = timer;//sets the boost timer
    }
    //plays the audios 
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    //detects the if we passed through a collider 
    private void OnTriggerEnter(Collider other)
    {
        //detects if the player hits an enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            //gives the sign that the game is over 
            gameOver = true;
            //disables the player's mesh renderer, like an explotion, boom!
            playerMeshRend.enabled = !playerMeshRend.enabled;
            //activates the crash explotion 
            crashParticle.Play();
            //plays the crash sound 
            PlaySound(crashClip);
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            //activates the coin particle effect
            coinParticle.Play();
        }
    }

    private void MovePlayer(Vector2 position)
    {
        //moves the player on the x axis
        transform.Translate(position.x * speed * Time.deltaTime * Vector3.forward);
        //moves the player on the y axis
        transform.Translate(position.y * speed * Time.deltaTime * Vector3.up);
    }

    private void TouchControls()
    {
        //detects if there's a touch
        if (Input.touchCount > 0)
        {
            //gets the first touch on the screen
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //saves the first position detected on the screen by the first touch
                centerPos = touch.position;
                //moves the joystick to the place where the touch was detected
                outerCircle.position = touch.position;
                circle.position = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                //saves the second position when you move your finger along the screen 
                movePos = touch.position;
                //moves the inner circle of the joystick to the position of your finger 
                circle.position = touch.position;
                //gets the normalized vector in order to have an scale up to one ot move the player 
                direction = (movePos - centerPos).normalized;
                MovePlayer(direction);
            }
            if (touch.phase == TouchPhase.Stationary)
            {
                //we save all the data from above to keep moving the player 
                movePos = touch.position;
                circle.position = touch.position;
                direction = (movePos - centerPos).normalized;
                MovePlayer(direction);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                //returns the inner circle from the joystick to the center when your finger is not touching the screen
                circle.position = centerPos;
            }
        }
    }

    // functions to have access to the private variables 
    public bool getGameOver()
    {
        return gameOver;
    }

}

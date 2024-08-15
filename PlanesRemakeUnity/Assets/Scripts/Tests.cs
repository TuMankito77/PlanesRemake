using UnityEngine;
using System.Collections;


// Input.GetTouch example.
//
// Attach to an origin based cube.
// A screen touch moves the cube on an iPhone or iPad.
// A second screen touch reduces the cube size.

public class Tests : MonoBehaviour
{
    private Vector3 initialPosition;//stores the initial position when a touch is detected 
    private Vector3 position;
    private Vector2 pos;
    private float horizontal;
    private float vertical=0;
    private float width;
    private float height;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void OnGUI()
    {
        // Compute a fontSize based on the size of the screen width.
        GUI.skin.label.fontSize = (int)(Screen.width / 25.0f);

        GUI.Label(new Rect(20, 20, width, height * 0.25f),
            "x = " + position.x.ToString("f2") +
            ", y = " + position.y.ToString("f2"));
    }

    void Update()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
                initialPosition.x = (initialPosition.x - width) / width;
                initialPosition.y = (initialPosition.y - height) / height;
                if (initialPosition.x > 0)
                {
                    horizontal = 1;
                }
                else if (initialPosition.x < 0)
                {
                    horizontal = -1;
                }
                Debug.Log("position x" + initialPosition.x + " position y " + initialPosition.y);
            }
            if(touch.phase == TouchPhase.Stationary)
            {
                initialPosition = touch.position;
                initialPosition.x = (initialPosition.x - width) / width;
                initialPosition.y = (initialPosition.y - height) / height;
                //Debug.Log("position x" + initialPosition.x + " position y " + initialPosition.y);
            }
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                if (pos.y < initialPosition.y)
                    vertical = -1;
                if (pos.y > initialPosition.y)
                    vertical = 1;

                
                //Debug.Log("position x" + pos.x + " position y " + pos.y);

                // Position the cube.
                //transform.position = new Vector3(position.x, transform.position.y, position.y);
            }
            if(touch.phase == TouchPhase.Ended)
            {
                horizontal = 0;
                vertical = 0;
            }
        }
        transform.Translate(Vector3.right * Time.deltaTime * 5f * horizontal);
        transform.Translate(Vector3.forward * Time.deltaTime * 5f * vertical);
    }
}
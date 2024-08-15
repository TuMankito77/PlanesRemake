using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashingWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player controller = other.GetComponent<Player>();
        if(controller != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}

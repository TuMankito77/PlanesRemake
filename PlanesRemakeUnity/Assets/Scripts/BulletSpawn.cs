using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    private AudioSource bulletAudio;
    public AudioClip shotSound;

    public GameObject bulletPrefap;
    private float startDelay = 1;
    private float repeatDelay = 2;
    void Start()
    {
        bulletAudio = GetComponent<AudioSource>(); 
        InvokeRepeating("SpawnBullet", startDelay, repeatDelay);
    }
    private void SpawnBullet()
    {
        Instantiate(bulletPrefap, transform.position, bulletPrefap.transform.rotation);
        bulletAudio.PlayOneShot(shotSound);
    }
}

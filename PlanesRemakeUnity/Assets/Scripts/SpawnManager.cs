using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public GameObject boosterPrefab;
    public GameObject enemyPrefab;
    public GameObject[] skins;
    private Player player;

    public float boundaryAxisY = 5;
    private float boundaryAxisX = 150;
    private float startTime = 2;
    private float interval = 8;
    private float randomCoinInterval;
    private float randomBoosterInterval;
    private float randomEnemyInterval;
    //private float random
    Vector3 spawnPositionWall;

    // Start is called before the first frame update
    void Start()
    {
        enablePlayer();

        player = GameObject.Find("Player").GetComponent<Player>();

        randomCoinInterval = Random.Range(2, 6);
        randomBoosterInterval = Random.Range(10, 15);
        randomEnemyInterval = Random.Range(15,20);
        InvokeRepeating("SpawnWall", startTime, interval);
        Invoke("SpawnBooster", randomBoosterInterval);
        Invoke("SpawnCoin", randomCoinInterval);
        Invoke("SpawnEnemy", randomEnemyInterval);
    }
    void Update()
    {
        if (player.getGameOver())
            Destroy(gameObject);
    }
    private void SpawnCoin()
    {
        Vector3 spawnPositionCoin = new Vector3(boundaryAxisX, Random.Range(-12, 12), 24.4f);
        Instantiate(coinPrefab, spawnPositionCoin, gameObject.transform.rotation);
        randomCoinInterval = Random.Range(2, 6);
        Invoke("SpawnCoin", randomCoinInterval);
    }
    private void SpawnWall()
    {
        spawnPositionWall = new Vector3(boundaryAxisX, Random.Range(-26, -14), 18);
        Instantiate(obstaclePrefab, spawnPositionWall, gameObject.transform.rotation);
    }
    private void SpawnBooster()
    {
        Vector3 spawnPositionBooster = new Vector3(boundaryAxisX, Random.Range(-12, 12), 24.4f);
        Instantiate(boosterPrefab, spawnPositionBooster, gameObject.transform.rotation);
        randomBoosterInterval = Random.Range(10, 15);
        Invoke("SpawnBooster", randomBoosterInterval);
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPositionEnemy = new Vector3(boundaryAxisX, Random.Range(-12,10), 24.4f);
        Instantiate(enemyPrefab, spawnPositionEnemy, enemyPrefab.transform.rotation);
        randomBoosterInterval = Random.Range(10, 15);
        Invoke("SpawnEnemy", randomBoosterInterval);
    }
    private void enablePlayer()
    {
        skins[0].SetActive(false);
        skins[1].SetActive(true);
        skins[2].SetActive(false);
    }
}

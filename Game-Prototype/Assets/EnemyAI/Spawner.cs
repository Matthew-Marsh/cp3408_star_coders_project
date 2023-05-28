using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int levelNumber = 1;
    public int spawnTimer = 30;
    public int numberEnemiesToSpawn = 4;
    public int detectionRange = 10;
    public int numberExistingEnemies = 0;
    public GameObject enemyBasic;
    public GameObject enemyAdvanced;
    public bool playerInRange = false;
    private SphereCollider detectionCollider;
    private bool spawnCooldown = false;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelNumber = gameManager.GetLevelNumber();
    }

    void Start()
    {
        // Sphere Collider around Spawn
        detectionCollider = this.gameObject.AddComponent<SphereCollider>();
        detectionCollider.center = Vector3.zero;
        detectionCollider.radius = detectionRange;
        detectionCollider.isTrigger = true;

    }

    void Update()
    {

        numberExistingEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (playerInRange && !spawnCooldown && numberExistingEnemies < 20)
        {
            SpawnEnemy();
            spawnCooldown = true;
            Invoke("SpawnCooldownReset", spawnTimer);
        }
    }

    void SpawnEnemy()
    {
        float levelMultiplier = levelNumber + (levelNumber / 2);
        float numberBasicEnemiesToSpawn;
        float numberAdvEnemiesToSpawn;
        
        if (levelNumber > 1)
        {
            numberBasicEnemiesToSpawn = Mathf.Round(numberEnemiesToSpawn * levelMultiplier);
            numberAdvEnemiesToSpawn = levelNumber - 1;
        } 
        else
        {
            numberBasicEnemiesToSpawn = numberEnemiesToSpawn;
            numberAdvEnemiesToSpawn = 0;
        }
        
        for (int i = 0; i < numberBasicEnemiesToSpawn; i++)
        {
            Instantiate(enemyBasic, this.transform.position, Quaternion.identity);
        }

        for (int i = 0; i < numberAdvEnemiesToSpawn; i++)
        {
            Instantiate(enemyAdvanced, this.transform.position, Quaternion.identity);
        }
    }

    void SpawnCooldownReset()
    {
        spawnCooldown = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}

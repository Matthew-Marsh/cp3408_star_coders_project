using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disabler : MonoBehaviour
{
    void Awake()
    {
        GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject spawner in enemySpawners)
        {
            spawner.SetActive(false);
        }
        foreach (GameObject enemy in enemys)
        {
            enemy.SetActive(false);
        }
    }
}

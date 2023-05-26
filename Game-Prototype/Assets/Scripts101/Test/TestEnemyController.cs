using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyController : MonoBehaviour
{
    EnemyMusicPlayer enemyMusicPlayer;

    private void Awake()
    {
        enemyMusicPlayer = FindObjectOfType<EnemyMusicPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyMusicPlayer.SetEnemyState(EnemyMusicPlayer.EnemyState.Attacking);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

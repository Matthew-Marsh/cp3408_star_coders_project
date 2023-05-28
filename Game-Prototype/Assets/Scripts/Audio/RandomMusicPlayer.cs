using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusicPlayer : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Attacking,
        InMenu
    }

    public CharacterState characterState;

    public AudioClip[] idleClips;
    public AudioClip[] attackClips;
    public AudioClip[] inMenuClips;

    public AudioClip GetRandomClip()
    {
        switch (characterState)
        {
            case CharacterState.Idle:
                return idleClips[Random.Range(0, idleClips.Length)];
            case CharacterState.Attacking:
                return attackClips[Random.Range(0, attackClips.Length)];
            case CharacterState.InMenu:
                return inMenuClips[Random.Range(0, inMenuClips.Length)];
            default:
                return null;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

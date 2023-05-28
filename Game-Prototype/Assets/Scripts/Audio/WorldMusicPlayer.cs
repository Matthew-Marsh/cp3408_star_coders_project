using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Random World Music Player based on states
/** Code to add to trigger states where required:
 * WorldMusicPlayer worldMusicPlayer;  -- Inside the class --
 * worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();  -- Add this to awake --
 * worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.Idle);  -- Add this to start and update --
 * worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.Attacking);  -- update on change --
 * worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.InMenu);  -- update on change --
 */
public class WorldMusicPlayer : MonoBehaviour
{
    private AudioSource worldMusicPlayerAudioSource;
    private bool isPlaying = false;

    private void Awake()
    {
        worldMusicPlayerAudioSource = GetComponent<AudioSource>();
    }

    public enum WorldState
    {
        Idle,
        Attacking,
        InMenu
    }

    public WorldState characterState;

    public AudioClip[] idleClips;
    public AudioClip[] attackClips;
    public AudioClip[] inMenuClips;

    public AudioClip GetRandomClip()
    {
        switch (characterState)
        {
            case WorldState.Idle:
                return idleClips[Random.Range(0, idleClips.Length)];
            case WorldState.Attacking:
                return attackClips[Random.Range(0, attackClips.Length)];
            case WorldState.InMenu:
                return inMenuClips[Random.Range(0, inMenuClips.Length)];
            default:
                return null;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayRandomAudioClip();
    }

    // Update is called once per frame
    void Update()
    {
        if (!worldMusicPlayerAudioSource.isPlaying)
        {
            isPlaying = false;
            PlayRandomAudioClip();
        }
    }

    public void PlayRandomAudioClip()
    {
        worldMusicPlayerAudioSource.clip = GetRandomClip();

        AudioClip clip = GetRandomClip();

        if (clip != null)
        {
            worldMusicPlayerAudioSource.clip = clip;
            worldMusicPlayerAudioSource.Play();
        }
    }

    public void SetWorldState(WorldState newWorldState)
    {
        characterState = newWorldState;

        if (isPlaying)
        {
            worldMusicPlayerAudioSource.Stop();
            isPlaying = false;
        }
        PlayRandomAudioClip();
    }
}

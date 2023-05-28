using System;
using UnityEngine;

// Add this to an empty game object with a box collider at the start of the level, where it will spawn the player spawns
public class StartCheckpoint : MonoBehaviour
{
    public GameObject playerPrefab;
    public AudioClip onSpawnAudio;

    private AudioSource audioSource;
    void Start()
    {

        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        audioSource = GetComponent<AudioSource>();
        PlayAudioClip(onSpawnAudio);

        //GameSystem.Instance.ResetTimer();
        //GameSystem.Instance.StartTimer();
        Destroy(gameObject);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
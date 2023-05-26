using UnityEngine;

// Simple footstep controller if dynamic one does not work
public class FootstepController : MonoBehaviour
{
    AudioSource footStepAudioSource;
    public AudioClip[] footStepClips;

    void Start()
    {
        footStepAudioSource = GetComponent<AudioSource>();
    }

    public void PlayStep()
    {
        if (footStepClips == null)
            return;

        footStepAudioSource.loop = false;
        AudioClip clip = footStepClips[Random.Range(0, footStepClips.Length)];
        footStepAudioSource.PlayOneShot(clip);
    }
}

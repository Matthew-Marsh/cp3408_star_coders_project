using UnityEngine;

// Simple footstep controller if dynamic one does not work
public class FootstepController : MonoBehaviour
{
    AudioSource footStepAudioSource;
    public AudioClip[] footStepClips;
    PlayerController controller;
    private float stepTimer = 0f;
    public float stepInterval = 0.2f;

    void Start()
    {
        footStepAudioSource = GetComponent<AudioSource>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (controller.isMoving)
        {
            if (stepTimer <= 0f)
            {
                PlayStep();
                stepTimer = stepInterval;
            }
            else
            {
                stepTimer -= Time.deltaTime;
            }
        }
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

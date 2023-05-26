using System.Collections;
using UnityEngine;

// Random Ambient Music Player based on states, the closer to the end checkpoint the ambient noises change
/** Code to add to trigger states where required:
 * Add audio source object and add this script - make audio source 2D
 * Add audio clips for each state
 */
public class AmbientMusicPlayer : MonoBehaviour
{
    private AudioSource ambientMusicAudioSource;
    private bool isPlaying = false;
    private bool isOnCoolDown = false;
    private float ambientAudioCoolDown = 2f;

    [SerializeField] public float distanceThreshold = 20f;
    private bool hasReachedEnd = false;
    private GameObject player;
    private GameObject endCheckpoint;
    private float distance = 0f;
    private float startDistance = 0f;

    private void Awake()
    {
        ambientMusicAudioSource = GetComponent<AudioSource>();
        endCheckpoint = GameObject.FindGameObjectWithTag("End");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public enum LocationState
    {
        Adventure,
        Danger,
        Safe
    }

    public LocationState locationState;

    public AudioClip[] adventureClips;
    public AudioClip[] dangerClips;
    public AudioClip[] safeClips;
    public AudioClip defaultClip;

    void Update()
    {
        CheckAndUpdateState();
        if (!ambientMusicAudioSource.isPlaying && !isOnCoolDown)
        {
            isPlaying = false;
            StartCoroutine(PlayRandomAudioClipwithCoolDown());
        }
        if (startDistance == 0)
        {
            startDistance = Vector3.Distance(player.transform.position, endCheckpoint.transform.position);
        }
        //Debug.Log("Ambient State is: " + locationState);
    }

    public AudioClip GetRandomClip()
    {
        switch (locationState)
        {
            case LocationState.Safe:
                if (safeClips.Length > 0)
                    return safeClips[Random.Range(0, safeClips.Length)];
                else
                    return defaultClip;
            case LocationState.Adventure:
                if (adventureClips.Length > 0)
                    return adventureClips[Random.Range(0, adventureClips.Length)];
                else
                    return defaultClip;
            case LocationState.Danger:
                if (dangerClips.Length > 0)
                    return dangerClips[Random.Range(0, dangerClips.Length)];
                else
                    return defaultClip;
            default:
                return null;

        }
    }

    IEnumerator PlayRandomAudioClipwithCoolDown()
    {
        isOnCoolDown = true;

        PlayRandomAudioClip();
        float coolDownDuration = GetRandomCoolDownDuration();
        yield return new WaitForSeconds(coolDownDuration);
        isOnCoolDown = false;
    }


    public void PlayRandomAudioClip()
    {
        ambientMusicAudioSource.clip = GetRandomClip();

        AudioClip clip = GetRandomClip();

        if (clip != null)
        {
            ambientMusicAudioSource.clip = clip;
            ambientMusicAudioSource.Play();
        }
    }

    public void SetAmbientState(LocationState newLocationState)
    {
        locationState = newLocationState;

        if (isPlaying)
        {
            ambientMusicAudioSource.Stop();
            isPlaying = false;
        }
        PlayRandomAudioClip();
    }

    private float GetRandomCoolDownDuration()
    {
        float maxCoolDown = ambientAudioCoolDown;
        float minCoolDown = 0f;
        float maxCoolDownBias = maxCoolDown * 0.7f;

        float randomValue = Random.Range(minCoolDown, maxCoolDown + maxCoolDownBias);
        float biasedValue = Mathf.Pow(randomValue, maxCoolDownBias);

        return biasedValue;
    }

    private void CheckAndUpdateState()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log("Player: " + player.ToString());
        endCheckpoint = GameObject.FindGameObjectWithTag("End");
        //Debug.Log("End point: " + endCheckpoint.ToString());
        distance = Vector3.Distance(player.transform.position, endCheckpoint.transform.position);
        //Debug.Log("Distance: " + distance.ToString());
        //Debug.Log("Start Distance: " + startDistance.ToString());

        if (endCheckpoint == null || player == null || distance == 0)
        {
            return;
        }

        if (!hasReachedEnd && endCheckpoint != null)
        {

            if (distance <= distanceThreshold)
            {
                hasReachedEnd = true;
                if (locationState != LocationState.Safe)
                    locationState = LocationState.Safe;

            }
            else if (distance < (startDistance / 2) && distance > distanceThreshold)
            {
                if (locationState != LocationState.Danger)
                    locationState = LocationState.Danger;
            }
            else
            {
                if (locationState != LocationState.Adventure)
                    locationState = LocationState.Adventure;
            }
        }
    }
}

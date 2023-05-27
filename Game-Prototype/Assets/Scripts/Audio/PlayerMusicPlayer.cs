using System.Collections;
using UnityEngine;

// Random player Music Player based on states
/** Code to add to trigger states where required:
 * Add audio source onto player - make 3D spatial, lower distance to 5 or 10
 * PlayerMusicPlayer playerMusicPlayer;  -- Inside the class --
 * playerMusicPlayer = FindObjectOfType<PlayerMusicPlayer>();  -- Add this to awake --
 * playerMusicPlayer.SetPlayerState(PlayerMusicPlayer.PlayerState.Idle);  -- Add this to start and update --
 * playerMusicPlayer.SetPlayerState(PlayerMusicPlayer.PlayerState.Attacking);  -- update on change --
 * playerMusicPlayer.SetPlayerState(PlayerMusicPlayer.PlayerState.Hurt);  -- update on change --
 */
public class PlayerMusicPlayer : MonoBehaviour
{
    public float playerIdleAudioCoolDown = 30f;
    public float playerAttackAudioCoolDown = 3f;
    public float playerHurtAudioCoolDown = 10f;

    private AudioSource playerMusicAudioSource;
    private bool isPlaying = false;
    private bool isOnCoolDown = false;
    private float playerAudioCoolDown = 2f;

    private void Awake()
    {
        playerMusicAudioSource = GetComponent<AudioSource>();
    }

    public enum PlayerState
    {
        Idle,
        Attacking,
        Hurt
    }

    public PlayerState playerState;

    public AudioClip[] idleClips;
    public AudioClip[] attackClips;
    public AudioClip[] HurtClips;

    public AudioClip GetRandomClip()
    {
        switch (playerState)
        {
            case PlayerState.Idle:
                return idleClips[Random.Range(0, idleClips.Length)];
            case PlayerState.Attacking:
                return attackClips[Random.Range(0, attackClips.Length)];
            case PlayerState.Hurt:
                return HurtClips[Random.Range(0, HurtClips.Length)];
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
        if (!playerMusicAudioSource.isPlaying && !isOnCoolDown)
        {
            isPlaying = false;
            StartCoroutine(PlayRandomAudioClipwithCoolDown());
        }
        // Stop the audio if the player is moving
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController.isMoving && playerMusicAudioSource.isPlaying)
        {
            playerMusicAudioSource.Stop();
            isPlaying = false;
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
        playerMusicAudioSource.clip = GetRandomClip();

        AudioClip clip = GetRandomClip();

        if (clip != null)
        {
            playerMusicAudioSource.clip = clip;
            playerMusicAudioSource.Play();
        }
    }

    public void SetPlayerState(PlayerState newPlayerState)
    {
        if (playerState != newPlayerState)
        {
            playerState = newPlayerState;

            switch (playerState)
            {
                case PlayerState.Idle:
                    playerAudioCoolDown = playerIdleAudioCoolDown;
                    break;
                case PlayerState.Attacking:
                    playerAudioCoolDown = playerAttackAudioCoolDown;
                    break;
                case PlayerState.Hurt:
                    playerAudioCoolDown = playerHurtAudioCoolDown;
                    break;
                default:
                    playerAudioCoolDown = 2f;
                    break;

            }

            if (isPlaying)
            {
                playerMusicAudioSource.Stop();
                isPlaying = false;
            }
            PlayRandomAudioClip();
        }
    }

    float GetRandomCoolDownDuration()
    {
        float maxCoolDown = playerAudioCoolDown;
        float minCoolDown = 0f;
        float maxCoolDownBias = maxCoolDown * 0.7f;

        float randomValue = Random.Range(minCoolDown, maxCoolDown + maxCoolDownBias);
        float biasedValue = Mathf.Pow(randomValue, maxCoolDownBias);

        return biasedValue;
    }
}

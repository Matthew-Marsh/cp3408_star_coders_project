using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Random enemy Music Player based on states
/** Code to add to trigger states where required:
 * Add audio source onto enemy - make 3D spatial, lower distance to 5 or 10
 * EnemyMusicPlayer enemyMusicPlayer;  -- Inside the class --
 * enemyMusicPlayer = FindObjectOfType<EnemyMusicPlayer>();  -- Add this to awake --
 * enemyMusicPlayer.SetEnemyState(EnemyMusicPlayer.EnemyState.Idle);  -- Add this to start and update --
 * enemyMusicPlayer.SetEnemyState(EnemyMusicPlayer.EnemyState.Attacking);  -- update on change --
 * enemyMusicPlayer.SetEnemyState(EnemyMusicPlayer.EnemyState.Alert);  -- update on change --
 */
public class EnemyMusicPlayer : MonoBehaviour
{
    public float enemyIdleAudioCoolDown = 30f;
    public float enemyAttackAudioCoolDown = 3f;
    public float enemyAlertAudioCoolDown = 10f;

    private AudioSource enemyMusicAudioSource;
    private bool isPlaying = false;
    private bool isOnCoolDown = false;
    private float enemyAudioCoolDown = 2f;

    private void Awake()
    {
        enemyMusicAudioSource = GetComponent<AudioSource>();
    }

    public enum EnemyState
    {
        Idle,
        Attacking,
        Alert
    }

    public EnemyState enemyState;

    public AudioClip[] idleClips;
    public AudioClip[] attackClips;
    public AudioClip[] AlertClips;

    public AudioClip GetRandomClip()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                return idleClips[Random.Range(0, idleClips.Length)];
            case EnemyState.Attacking:
                return attackClips[Random.Range(0, attackClips.Length)];
            case EnemyState.Alert:
                return AlertClips[Random.Range(0, AlertClips.Length)];
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
        if (!enemyMusicAudioSource.isPlaying && !isOnCoolDown)
        {
            isPlaying = false;
            StartCoroutine(PlayRandomAudioClipwithCoolDown());
        }
    }

    IEnumerator PlayRandomAudioClipwithCoolDown()
    {
        isOnCoolDown = true;

        PlayRandomAudioClip();
        float coolDownDuration = GetRandomCoolDownDuration();
        yield return new WaitForSeconds(coolDownDuration);
        isOnCoolDown= false;
    }

    public void PlayRandomAudioClip()
    {
        enemyMusicAudioSource.clip = GetRandomClip();

        AudioClip clip = GetRandomClip();

        if (clip != null)
        {
            enemyMusicAudioSource.clip = clip;
            enemyMusicAudioSource.Play();
        }
    }

    public void SetEnemyState(EnemyState newEnemyState)
    {
        enemyState = newEnemyState;

        switch (enemyState)
        {
            case EnemyState.Idle:
                enemyAudioCoolDown = enemyIdleAudioCoolDown;
                break;
            case EnemyState.Attacking:
                enemyAudioCoolDown = enemyAttackAudioCoolDown;
                break;
            case EnemyState.Alert:
                enemyAudioCoolDown = enemyAlertAudioCoolDown;
                break;
            default:
                enemyAudioCoolDown = 2f;
                break;

        }

        if (isPlaying)
        {
            enemyMusicAudioSource.Stop();
            isPlaying = false;
        }
        PlayRandomAudioClip();
    }

    float GetRandomCoolDownDuration()
    {
        float maxCoolDown = enemyAudioCoolDown;
        float minCoolDown = 0f;
        float maxCoolDownBias = maxCoolDown * 0.7f;

        float randomValue = Random.Range(minCoolDown, maxCoolDown + maxCoolDownBias);
        float biasedValue = Mathf.Pow(randomValue, maxCoolDownBias);

        return biasedValue;
    }
}

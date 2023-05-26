using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public int maxNumberOfEnemies = 2;
    public float spawnDuration = 20.0f;
    public GameObject enemyPrefab;
    public AudioClip openChestAudio;
    public AudioClip closeChestAudio;
    public AudioClip trapAudio;

    private GameObject loot;
    private float spawnTimer;
    private int numberOfEnemies;
    private bool isLootClaimed;
    private AudioSource audioSource;
    private bool isTrapAudioPlaying;

    [SerializeField] public List<GameObject> lootPrefabs;
    [SerializeField] public Animator chestAnimator;

    private void Start()
    {
        spawnTimer = spawnDuration;
        audioSource = GetComponent<AudioSource>();
        //chestAnimator = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnTimer = spawnDuration;

            if (!isLootClaimed && numberOfEnemies < maxNumberOfEnemies)
            {
                spawnEnemy(enemyPrefab);
                numberOfEnemies++;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && other.CompareTag("Player"))
        {
            if (!chestAnimator.GetBool("activateChest"))
            {
                PlayAudioClip(openChestAudio);
            }

            chestAnimator.SetBool("activateChest", true);

            if (loot == null && !isLootClaimed)
            {
                SpawnLoot();
            }
        }
        else
        {
            if (other.CompareTag("Player") && !isTrapAudioPlaying)
            {
                PlayLoopingAudioClip(trapAudio);
                isTrapAudioPlaying = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (chestAnimator.GetBool("activateChest"))
            {
                PlayAudioClip(closeChestAudio);
            }

            chestAnimator.SetBool("activateChest", false);

            if (loot != null && !isLootClaimed)
            {
                loot.SetActive(false);
                //GameObject lootToDestroy = loot;
                //loot = null;
                //DestroyGameObject(lootToDestroy);

            }
            spawnTimer = spawnDuration;
            isTrapAudioPlaying = false;
        }
    }

    private void SpawnLoot()
    {
        if (lootPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, lootPrefabs.Count);
            GameObject randomLoot = lootPrefabs[randomIndex];
            loot = Instantiate(randomLoot, transform.position, Quaternion.identity);
            LootItem lootItem = loot.GetComponent<LootItem>();
            if (lootItem != null)
            {
                lootItem.setClaimedCallback(OnLootClaimed);
            }
        }
    }

    private void DestroyGameObject(GameObject objectToDestroy)
    {
        Destroy(objectToDestroy);
    }

    private void spawnEnemy(GameObject newEnemy)
    {
        float spawnDistance = 2.0f;
        float spawnAngle = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0f, spawnAngle, 0f);

        Vector3 randomOffset = Quaternion.AngleAxis(spawnAngle, Vector3.up) * Vector3.forward * spawnDistance;
        Vector3 spawnPosition = transform.position + randomOffset;

        Instantiate(newEnemy, spawnPosition, randomRotation);
    }

    private void OnLootClaimed(GameObject claimedLoot)
    {
        isLootClaimed = true;
        claimedLoot.SetActive(false);
        //DestroyGameObject(claimedLoot);
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

    private void PlayLoopingAudioClip(AudioClip clip)
    {
        if (clip != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

}

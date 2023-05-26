using TMPro;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float interactRange = 5f;

    public AudioClip inventoryPickUpAudio;
    public AudioClip consumableAudioClip;
    public AudioClip weaponAudioClip;
    private AudioSource playerAudioSource;

    private InventorySystem inventory;
    WorldMusicPlayer worldMusicPlayer;

    private void Awake()
    {
        worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();

        worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.Idle);
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement.Normalize(); // Normalize diagonal movement

        transform.Translate(movement * movementSpeed * Time.deltaTime);

        // Pick up loot
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUpLoot();
        }

        // Go back an inventory item
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.ChangeItem(false);
            Debug.Log("Change Item Back.");
        }

        // Use inventory item
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InventoryItem currentItem = inventory.GetCurrentItem();
            if (currentItem != null)
            {
                if (currentItem.CompareTag("Weapon"))
                {
                    PlayAudioClip(weaponAudioClip, playerAudioSource);
                }
                else if (currentItem.CompareTag("Loot"))
                {
                    PlayAudioClip(consumableAudioClip, playerAudioSource);
                }
            }
            inventory.UseItem();
            Debug.Log("Use Item.");

        }

        // Go forward an inventory item
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.ChangeItem(true);
            Debug.Log("Change Item Forward.");
        }
    }

    private void PlayAudioClip(AudioClip clip, AudioSource audioSource)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void TryPickUpLoot()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Loot") || collider.CompareTag("Weapon"))
            {
                LootItem lootItem = collider.GetComponent<LootItem>();
                if (lootItem != null && !lootItem.IsClaimed())
                {
                    lootItem.SetClaimed();
                    PlayAudioClip(inventoryPickUpAudio, playerAudioSource);
                    Debug.Log(lootItem.ToString() + " claimed.");

                    inventory.AddToInventory(collider.gameObject);
                    Debug.Log(collider.gameObject.ToString() + " picked up.");
                    break;
                }
            }
        }
    }
}
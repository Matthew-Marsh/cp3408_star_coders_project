using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public bool isWeaponAvailable = true;
    public float coolDownDuration = 2.0f;
    public float floorAdjustmentYAxis = 0f;
    public float turnSpeed = 0.9f;
    float speed;

    [Header("Audio")]
    public AudioClip inventoryPickUpAudio;
    public AudioClip consumableAudioClip;
    public AudioClip weaponAudioClip;
    private AudioSource playerAudioSource;
    WorldMusicPlayer worldMusicPlayer;

    [Header("Invetory")]
    public float interactRange = 5f;
    private InventorySystem inventory;

    private Rigidbody rb;
    private Camera mainCamera;
    Animator anim;
    bool isSprinting;
    bool isAlive = true;
   
    PlayerHealthController health;
    GameObject weapon;

    private void Awake()
    {
        worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();
        playerAudioSource = GetComponent<AudioSource>();
        Debug.Log("Audio: " + worldMusicPlayer.ToString());
        Debug.Log("Audio: " + playerAudioSource.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventorySystem>();
        worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.Idle);
        Debug.Log("Inventory: " + inventory.ToString());

        weapon = GameObject.FindGameObjectWithTag("Weapon");

        //equipHandObject = GameObject.FindGameObjectWithTag("EquipHand");
        //Debug.Log("Equip hand object: " + equipHandObject.ToString());
        //if (equipHandObject != null)
            //weapon = equipHandObject.GetComponentInChildren<GameObject>();

        Debug.Log("Weapon object: " + weapon.ToString());

        if (weapon != null)
            weapon.GetComponent<BoxCollider>().enabled = false;

        rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        anim = this.GetComponent<Animator>();
        health = this.GetComponent<PlayerHealthController>();
    }

    void MovementControl()
    {
        bool isMoving = false;
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            movement -= mainCamera.transform.right;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement += mainCamera.transform.right;
            isMoving = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            movement += mainCamera.transform.forward;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement -= mainCamera.transform.forward;
            isMoving = true;
        }

        if (isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = sprintSpeed;
                isSprinting = true;
            }
            else
            {
                speed = walkSpeed;
                isSprinting = false;
            }

            //if (CanMove(movement))  // Stops going through objects/walls
            //{
                movement.y = 0f;
                movement.Normalize();
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

                if (isSprinting)
                    anim.SetTrigger("isRunFwd");
                else
                    anim.SetTrigger("isWalkFwd");

                transform.position += movement * speed * Time.deltaTime;
            }
            else
            {
                anim.SetTrigger("isIdle");
            }
        //}
    }

    // Disables the box collider on the players weapon
    void DisableWeaponBoxCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }
    // Enables the box collider on the players weapon
    void EnableWeaponBoxCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = true;
    }

    // Starts attack animation, enables the weapons box collider then disables it again, puts attacking
    // on a cooldown
    void AttackControl()
    {
        if (isWeaponAvailable == false)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            anim.Play("MeeleeAttack_TwoHanded");
            Invoke("EnableWeaponBoxCollider", 1);
            Invoke("DisableWeaponBoxCollider", 2); // Disables cooldown to prevent players walking into enemys to cause damage
            StartCoroutine(StartCooldown());
        }
    }

    public IEnumerator StartCooldown()
    {
        isWeaponAvailable = false;
        yield return new WaitForSeconds(coolDownDuration);
        isWeaponAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive == true)
        {
            MovementControl();
            AttackControl();

            // this code controls the player character following the mouse position
            
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log(cameraRay.ToString());
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, floorAdjustmentYAxis, 0));
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                Vector3 lookDirection = pointToLook - transform.position;
                lookDirection.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 360f * Time.deltaTime);
            }

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

        if (health.health <= 0)
        {
            anim.SetTrigger("isDead");
            isAlive = false;
        }
    }

    // Check if the raycast hits a collider e.g. objects, walls, enemies
    bool CanMove(Vector3 movement)
    {
        float rayDistance = speed * Time.deltaTime;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, movement, out hit, rayDistance))
        {
            return !hit.collider;
        }

        return true;
    }

    // Play audio clip
    private void PlayAudioClip(AudioClip clip, AudioSource audioSource)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Try to pick up loot
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

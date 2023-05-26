using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    float speed;
    private Rigidbody rb;
    private Camera mainCamera;
    Animator anim;
    bool isSprinting;
    public bool isWeaponAvailable = true;
    public float coolDownDuration = 2.0f;
    PlayerHealthController health;
    bool isAlive = true;
    public float floorAdjustmentYAxis = 0f;
    public float turnSpeed = 0.9f;
    GameObject equipHandObject;
    WeaponItem weapon;


    // Start is called before the first frame update
    void Start()
    {
        equipHandObject = GameObject.FindWithTag("EquipHand");
        if (equipHandObject != null)
            weapon = equipHandObject.GetComponentInChildren<WeaponItem>();

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

            if (CanMove(movement))  // Stops going through objects/walls
            {
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
        }
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
}

using System.Collections;
using System.Collections.Generic;
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
    GameObject weapon;
    public bool isWeaponAvailable = true;
    public float coolDownDuration = 2.0f;
    PlayerHealthController health;
    bool isAlive = true;
    public float floorAdjustmentYAxis = 0f;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        weapon.GetComponent<BoxCollider>().enabled = false;
        rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        anim = this.GetComponent<Animator>();
        health = this.GetComponent<PlayerHealthController>();
    }

    void MovementControl()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (Input.GetKey("left shift"))
        {
            speed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            speed = walkSpeed;
            isSprinting = false;
        }
        if (movement.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f * Time.deltaTime);

            if (isSprinting)
                anim.SetTrigger("isRunFwd");
            else
                anim.SetTrigger("isWalkFwd");

            transform.position += movement * speed * Time.deltaTime;
        }
        //if (Input.GetKey("w") && Input.GetKey("a"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunLeft");
        //    else
        //        anim.SetTrigger("isWalkLeft");
        //    transform.position += (Vector3.forward * speed * Time.deltaTime) + (Vector3.left * speed * Time.deltaTime);
        //}
        //else if (Input.GetKey("w") && Input.GetKey("d"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunRight");
        //    else
        //        anim.SetTrigger("isWalkRight");
        //    transform.position += (Vector3.forward * speed * Time.deltaTime) + (Vector3.right * speed * Time.deltaTime);
        //}
        //else if (Input.GetKey("s") && Input.GetKey("a"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunBck");
        //    else
        //        anim.SetTrigger("isWalkBck");
        //    transform.position += (Vector3.back * speed * Time.deltaTime) + (Vector3.left * speed * Time.deltaTime);
        //}
        //else if (Input.GetKey("s") && Input.GetKey("d"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunBck");
        //    else
        //        anim.SetTrigger("isWalkBck");
        //    transform.position += (Vector3.back * speed * Time.deltaTime) + (Vector3.right * speed * Time.deltaTime);
        //}
        //else if (Input.GetKey("d"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunFwd");
        //    else
        //        anim.SetTrigger("isWalkFwd");
        //    transform.position += Vector3.right * speed * Time.deltaTime;
        //}
        //else if (Input.GetKey("a"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunFwd");
        //    else
        //        anim.SetTrigger("isWalkFwd");
        //    transform.position += Vector3.left * speed * Time.deltaTime;
        //}
        //else if (Input.GetKey("w"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunFwd");
        //    else
        //        anim.SetTrigger("isWalkFwd");
        //    transform.position += Vector3.forward * speed * Time.deltaTime;
        //}
        //else if (Input.GetKey("s"))
        //{
        //    if (isSprinting)
        //        anim.SetTrigger("isRunBck");
        //    else
        //        anim.SetTrigger("isWalkBck");
        //    transform.position += Vector3.back * speed * Time.deltaTime;
        //}
        else
        {
            anim.SetTrigger("isIdle");
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
        if(isAlive == true)
        {
            MovementControl();
            AttackControl();

            // this code controls the player character following the mouse position
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, floorAdjustmentYAxis, 0));
            float rayLength;

            if(groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                Vector3 lookDirection = pointToLook - transform.position;
                lookDirection.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 360f * Time.deltaTime);
                //transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }

        if (health.health <= 0)
        {
            anim.SetTrigger("isDead");
            isAlive = false;
        }
    }
}

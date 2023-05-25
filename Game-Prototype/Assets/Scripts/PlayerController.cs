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
    public float coolDownDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        weapon.GetComponent<BoxCollider>().enabled = false;
        rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        anim = this.GetComponent<Animator>();
    }

    void MovementControl()
    {
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

        if (Input.GetKey("w") && Input.GetKey("a"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunLeft");
            else
                anim.SetTrigger("isWalkLeft");
            transform.position += (Vector3.forward * speed * Time.deltaTime) + (Vector3.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey("w") && Input.GetKey("d"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunRight");
            else
                anim.SetTrigger("isWalkRight");
            transform.position += (Vector3.forward * speed * Time.deltaTime) + (Vector3.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey("s") && Input.GetKey("a"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunBck");
            else
                anim.SetTrigger("isWalkBck");
            transform.position += (Vector3.back * speed * Time.deltaTime) + (Vector3.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey("s") && Input.GetKey("d"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunBck");
            else
                anim.SetTrigger("isWalkBck");
            transform.position += (Vector3.back * speed * Time.deltaTime) + (Vector3.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey("d"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunFwd");
            else
                anim.SetTrigger("isWalkFwd");
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetKey("a"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunFwd");
            else
                anim.SetTrigger("isWalkFwd");
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey("w"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunFwd");
            else
                anim.SetTrigger("isWalkFwd");
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey("s"))
        {
            if (isSprinting)
                anim.SetTrigger("isRunBck");
            else
                anim.SetTrigger("isWalkBck");
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
        else
        {
            anim.SetTrigger("isIdle");
        }
    }

    void DisableWeaponBoxCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }

    void EnableWeaponBoxCollider()
    {
        weapon.GetComponent<BoxCollider>().enabled = true;
    }

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
            Invoke("DisableWeaponBoxCollider", 2);
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
        MovementControl();
        AttackControl();

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
}

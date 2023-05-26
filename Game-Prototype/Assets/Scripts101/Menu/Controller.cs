using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;



public class Controller : MonoBehaviour
{
    //public Weapon[] startingWeapons;

    //[Header("Audio")]
    //public RandomPlayer FootstepPlayer;
    //public AudioClip JumpingAudioCLip;
    //public AudioClip LandingAudioClip;

    //bool m_IsPaused = false;
    //int m_CurrentWeapon;
    //public bool CanPause { get; set; } = true;

    //List<Weapon> m_Weapons = new List<Weapon>();

    //bool wasGrounded = m_Grounded;

    //bool m_Grounded;

    //void Awake()
    //{
    //    Instance = this;
    //}


    //// Start is called before the first frame update
    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;

    //    m_IsPaused = false;

    //    for (int i = 0; i < startingWeapons.Length; ++i)
    //    {
    //        PickupWeapon(startingWeapons[i]);
    //    }

    //    m_CurrentWeapon = -1;
    //    ChangeWeapon(0);



    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (CanPause && Input.GetButtonDown("Menu"))
    //    {
    //        PauseMenu.Instance.Display();
    //    }


    //    if (Input.GetAxis("Mouse ScrollWheel") < 0)
    //    {
    //        ChangeWeapon(m_CurrentWeapon - 1);
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
    //    {
    //        ChangeWeapon(m_CurrentWeapon + 1);
    //    }

    //    // Play landing audio clip after jumping
    //    //if (!wasGrounded && m_Grounded)
    //    //{
    //    //    FootstepPlayer.PlayClip(LandingAudioClip, 0.8f, 1.1f);
    //    //}


    //    public void DisplayCursor(bool display)
    //    {
    //        m_IsPaused = display;
    //        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
    //        Cursor.visible = display;
    //    }

    //    void PickupWeapon(Weapon prefab)
    //    {
    //        // Check if weapon already exists and add if not
    //        if (!m_Weapons.Exists(weapon => weapon.name == prefab.name))
    //        {
    //            var w = Instantiate(prefab, WeaponPosition, false);
    //            w.name = prefab.name;
    //            w.transform.localPosition = Vector3.zero;
    //            w.transform.localRotation = Quaternion.identity;
    //            w.gameObject.SetActive(false);

    //            w.PickedUp(this);

    //            m_Weapons.Add(w);
    //        }
    //    }

    //    void ChangeWeapon(int number)
    //    {
    //        if (m_CurrentWeapon != -1)
    //        {
    //            m_Weapons[m_CurrentWeapon].PutAway();
    //            m_Weapons[m_CurrentWeapon].gameObject.SetActive(false);
    //        }

    //        m_CurrentWeapon = number;

    //        if (m_CurrentWeapon < 0)
    //            m_CurrentWeapon = m_Weapons.Count - 1;
    //        else if (m_CurrentWeapon >= m_Weapons.Count)
    //            m_CurrentWeapon = 0;

    //        m_Weapons[m_CurrentWeapon].gameObject.SetActive(true);
    //        m_Weapons[m_CurrentWeapon].Selected();
    //    }

    //    public void PlayFootstep()
    //    {
    //        FootstepPlayer.PlayRandom();
    //    }



    //}
}

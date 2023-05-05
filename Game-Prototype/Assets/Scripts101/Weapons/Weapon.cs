using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Add this script to every weapon
public class Weapon : MonoBehaviour
{
//    static RaycastHit[] s_HitInfoBuffer = new RaycastHit[8];

//    public enum TriggerType
//    {
//        Auto,
//        Manual
//    }

//    public enum WeaponType
//    {
//        Raycast,
//        Projectile,
//        Melee
//    }

//    public enum WeaponState
//    {
//        Idle,
//        Firing,
//        Reloading,
//        Swinging
//    }

//    [System.Serializable]
//    public class AdvancedSettings
//    {
//        public float spreadAngle = 0.0f;
//        public int projectilePerShot = 1;
//        //public float screenShakeMultiplier = 1.0f;
//    }

//    public TriggerType triggerType = TriggerType.Manual;
//    public WeaponType weaponType = WeaponType.Raycast;
//    public float attackRate = 0.5f;
//    public float castDrawTime = 2.0f;
//    public float damage = 1.0f;

//    public Projectile projectilePrefab;
//    public float projectileLaunchForce = 200.0f;

//    public Transform EndPoint;

//    public AdvancedSettings advancedSettings;

//    [Header("Animation Clips")]
//    public AnimationClip fireSpellAnimationClip;
//    public AnimationClip fireArrowAnimationClip;
//    public AnimationClip castAnimationClip;
//    public AnimationClip drawBowAnimationClip;
//    public AnimationClip swingAnimationClip;


//    [Header("Audio Clips")]
//    public AudioClip fireSpellAudioClip;
//    public AudioClip fireArrowAudioClip;
//    public AudioClip castSpellAudioClip;
//    public AudioClip drawBowAudioClip;
//    public AudioClip swingAudioAudioClip;

//    public bool triggerDown
//    {
//        get { return m_TriggerDown; }
//        set
//        {
//            m_TriggerDown = value;
//            if (!m_TriggerDown) m_ShotDone = false;
//        }
//    }

//    public WeaponState CurrentState => m_CurrentState;
//    public Controller Owner => m_Owner;

//    Controller m_Owner;

//    Animator m_Animator;
//    WeaponState m_CurrentState;
//    bool m_ShotDone;
//    float m_ShotTimer = -1.0f;
//    bool m_TriggerDown;

//    AudioSource m_Source;

//    class ActiveTrail
//    {
//        //public LineRenderer renderer;
//        public Vector3 direction;
//        public float remainingTime;
//    }

//    //List<ActiveTrail> m_ActiveTrails = new List<ActiveTrail>();

//    Queue<Projectile> m_ProjectilePool = new Queue<Projectile>();

//    int fireNameHash = Animator.StringToHash("fire");
//    int reloadNameHash = Animator.StringToHash("reload");

//    void Awake()
//    {
//        m_Animator = GetComponentInChildren<Animator>();
//        m_Source = GetComponentInChildren<AudioSource>();

//        //if (PrefabRayTrail != null)
//        //{
//        //    const int trailPoolSize = 16;
//        //    PoolSystem.Instance.InitPool(PrefabRayTrail, trailPoolSize);
//        //}

//        //if (projectilePrefab != null)
//        //{
//        //    //a minimum of 4 is useful for weapon that have a clip size of 1 and where you can throw a second
//        //    //or more before the previous one was recycled/exploded.
//        //    int size = Mathf.Max(4, clipSize) * advancedSettings.projectilePerShot;
//        //    for (int i = 0; i < size; ++i)
//        //    {
//        //        Projectile p = Instantiate(projectilePrefab);
//        //        p.gameObject.SetActive(false);
//        //        m_ProjectilePool.Enqueue(p);
//        //    }
//        //}
//    }

//    public void PickedUp(Controller c)
//    {
//        m_Owner = c;
//    }

//    //public void PutAway()
//    //{
//    //    m_Animator.WriteDefaultValues();

//    //    for (int i = 0; i < m_ActiveTrails.Count; ++i)
//    //    {
//    //        var activeTrail = m_ActiveTrails[i];
//    //        m_ActiveTrails[i].renderer.gameObject.SetActive(false);
//    //    }

//    //    m_ActiveTrails.Clear();
//    //}

//    public void Selected()
//    {
//        if (fireSpellAnimationClip != null)
//            m_Animator.SetFloat("fireSpeed", fireSpellAnimationClip.length / attackRate);

//        if (fireArrowAnimationClip != null)
//            m_Animator.SetFloat("fireSpeed", fireArrowAnimationClip.length / attackRate);

//        if (swingAnimationClip != null)
//            m_Animator.SetFloat("fireSpeed", swingAnimationClip.length / attackRate);

//        if (castAnimationClip != null)
//            m_Animator.SetFloat("castSpeed", castAnimationClip.length / castDrawTime);

//        if (drawBowAnimationClip != null)
//            m_Animator.SetFloat("castSpeed", drawBowAnimationClip.length / castDrawTime);

//        m_CurrentState = WeaponState.Idle;

//        triggerDown = false;
//        m_ShotDone = false;

//        WeaponInfoUI.Instance.UpdateWeaponName(this);

//        m_Animator.SetTrigger("selected");
//    }

//    public void Fire(AudioClip audioClip)
//    {
//        if (m_CurrentState != WeaponState.Idle || m_ShotTimer > 0)
//            return;

//        m_ShotTimer = attackRate;

//        //the state will only change next frame, so we set it right now.
//        m_CurrentState = WeaponState.Firing;

//        m_Animator.SetTrigger("fire");

//        m_Source.pitch = Random.Range(0.7f, 1.0f);
//        m_Source.PlayOneShot(audioClip);

//        if (weaponType == WeaponType.Raycast)
//        {
//            for (int i = 0; i < advancedSettings.projectilePerShot; ++i)
//            {
//                RaycastShot();
//            }
//        }
//        else
//        {
//            ProjectileShot();
//        }
//    }


//    void RaycastShot()
//    {

//        //compute the ratio of our spread angle over the fov to know in viewport space what is the possible offset from center
//        float spreadRatio = advancedSettings.spreadAngle / Controller.Instance.MainCamera.fieldOfView;

//        Vector2 spread = spreadRatio * Random.insideUnitCircle;

//        RaycastHit hit;
//        Ray r = Controller.Instance.MainCamera.ViewportPointToRay(Vector3.one * 0.5f + (Vector3)spread);
//        Vector3 hitPosition = r.origin + r.direction * 200.0f;

//        //    if (Physics.Raycast(r, out hit, 1000.0f, ~(1 << 9), QueryTriggerInteraction.Ignore))
//        //    {
//        //        Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
//        //        ImpactManager.Instance.PlayImpact(hit.point, hit.normal, renderer == null ? null : renderer.sharedMaterial);

//        //        //if too close, the trail effect would look weird if it arced to hit the wall, so only correct it if far
//        //        if (hit.distance > 5.0f)
//        //            hitPosition = hit.point;

//        //        //this is a target
//        //        if (hit.collider.gameObject.layer == 10)
//        //        {
//        //            Target target = hit.collider.gameObject.GetComponent<Target>();
//        //            target.Got(damage);
//        //        }
//        //    }


//        //    if (PrefabRayTrail != null)
//        //    {
//        //        var pos = new Vector3[] { GetCorrectedMuzzlePlace(), hitPosition };
//        //        var trail = PoolSystem.Instance.GetInstance<LineRenderer>(PrefabRayTrail);
//        //        trail.gameObject.SetActive(true);
//        //        trail.SetPositions(pos);
//        //        m_ActiveTrails.Add(new ActiveTrail()
//        //        {
//        //            remainingTime = 0.3f,
//        //            direction = (pos[1] - pos[0]).normalized,
//        //            renderer = trail
//        //        });
//        //    }
//        //}

//        void ProjectileShot()
//        {
//            for (int i = 0; i < advancedSettings.projectilePerShot; ++i)
//            {
//                float angle = Random.Range(0.0f, advancedSettings.spreadAngle * 0.5f);
//                Vector2 angleDir = Random.insideUnitCircle * Mathf.Tan(angle * Mathf.Deg2Rad);

//                Vector3 dir = EndPoint.transform.forward + (Vector3)angleDir;
//                dir.Normalize();

//                var p = m_ProjectilePool.Dequeue();

//                p.gameObject.SetActive(true);
//                p.Launch(this, dir, projectileLaunchForce);
//            }
//        }

//        //For optimization, when a projectile is "destroyed" it is instead disabled and return to the weapon for reuse.
//        public void ReturnProjecticle(Projectile p)
//        {
//            m_ProjectilePool.Enqueue(p);
//        }

//        void Update()
//        {
//            UpdateControllerState();

//            if (m_ShotTimer > 0)
//                m_ShotTimer -= Time.deltaTime;

//            Vector3[] pos = new Vector3[2];
//            for (int i = 0; i < m_ActiveTrails.Count; ++i)
//            {
//                var activeTrail = m_ActiveTrails[i];

//                activeTrail.renderer.GetPositions(pos);
//                activeTrail.remainingTime -= Time.deltaTime;

//                pos[0] += activeTrail.direction * 50.0f * Time.deltaTime;
//                pos[1] += activeTrail.direction * 50.0f * Time.deltaTime;

//                m_ActiveTrails[i].renderer.SetPositions(pos);

//                if (m_ActiveTrails[i].remainingTime <= 0.0f)
//                {
//                    m_ActiveTrails[i].renderer.gameObject.SetActive(false);
//                    m_ActiveTrails.RemoveAt(i);
//                    i--;
//                }
//            }
//        }

//        void UpdateControllerState()
//        {
//            m_Animator.SetFloat("speed", m_Owner.Speed);
//            m_Animator.SetBool("grounded", m_Owner.Grounded);

//            var info = m_Animator.GetCurrentAnimatorStateInfo(0);

//            WeaponState newState;
//            if (info.shortNameHash == fireNameHash)
//                newState = WeaponState.Firing;
//            else if (info.shortNameHash == reloadNameHash)
//                newState = WeaponState.Reloading;
//            else
//                newState = WeaponState.Idle;

//            if (triggerDown)
//            {
//                if (triggerType == TriggerType.Manual)
//                {
//                    if (!m_ShotDone)
//                    {
//                        m_ShotDone = true;
//                        // TODO: Decision structure to use correct audio source
//                        Fire(fireSpellAudioClip);
//                    }
//                }
//                else
//                    // TODO: Decision structure to use correct audio source
//                    Fire(fireSpellAudioClip);
//            }
//        }
//    }

//#if UNITY_EDITOR

//    [CustomEditor(typeof(Weapon))]
//    public class WeaponEditor : Editor
//    {
//        SerializedProperty m_TriggerTypeProp;
//        SerializedProperty m_WeaponTypeProp;
//        SerializedProperty m_AttackRateProp;
//        SerializedProperty m_CastDrawTimeProp;
//        SerializedProperty m_DamageProp;
//        SerializedProperty m_ProjectilePrefabProp;
//        SerializedProperty m_ProjectileLaunchForceProp;
//        SerializedProperty m_EndPointProp;
//        SerializedProperty m_AdvancedSettingsProp;
//        SerializedProperty m_FireSpellAnimationClipProp;
//        SerializedProperty m_FireArrowAnimationClipProp;
//        SerializedProperty m_CastSpellAnimationClipProp;
//        SerializedProperty m_DrawBowAnimationClipProp;
//        SerializedProperty m_FireSpellAudioClipProp;
//        SerializedProperty m_FireArrowAudioClipProp;
//        SerializedProperty m_CastSpellAudioClipProp;
//        SerializedProperty m_DrawBowAudioClipProp;
//        // SerializedProperty m_PrefabRayTrailProp;
//        //SerializedProperty m_DisabledOnEmpty;

//        void OnEnable()
//        {
//            m_TriggerTypeProp = serializedObject.FindProperty("triggerType");
//            m_WeaponTypeProp = serializedObject.FindProperty("weaponType");
//            m_AttackRateProp = serializedObject.FindProperty("attackRate");
//            m_CastDrawTimeProp = serializedObject.FindProperty("castDrawTime");
//            m_DamageProp = serializedObject.FindProperty("damage");
//            m_ProjectilePrefabProp = serializedObject.FindProperty("projectilePrefab");
//            m_ProjectileLaunchForceProp = serializedObject.FindProperty("projectileLaunchForce");
//            m_EndPointProp = serializedObject.FindProperty("EndPoint");
//            m_AdvancedSettingsProp = serializedObject.FindProperty("advancedSettings");
//            m_FireSpellAnimationClipProp = serializedObject.FindProperty("FireSpellAnimationClip");
//            m_FireArrowAnimationClipProp = serializedObject.FindProperty("FireArrowAnimationClip");
//            m_CastSpellAnimationClipProp = serializedObject.FindProperty("CastSpellAnimationClip");
//            m_DrawBowAnimationClipProp = serializedObject.FindProperty("DrawBowAnimationClip");
//            m_FireSpellAudioClipProp = serializedObject.FindProperty("FireSpellAudioClip");
//            m_FireArrowAudioClipProp = serializedObject.FindProperty("FireArrowAudioClip");
//            m_DrawBowAudioClipProp = serializedObject.FindProperty("DrawBowAudioClip");
//            m_CastSpellAudioClipProp = serializedObject.FindProperty("CastSpellAudioClip");
//            //m_PrefabRayTrailProp = serializedObject.FindProperty("PrefabRayTrail");
//            //m_DisabledOnEmpty = serializedObject.FindProperty("DisabledOnEmpty");
//        }

//        public override void OnInspectorGUI()
//        {
//            serializedObject.Update();

//            EditorGUILayout.PropertyField(m_TriggerTypeProp);
//            EditorGUILayout.PropertyField(m_WeaponTypeProp);
//            EditorGUILayout.PropertyField(m_AttackRateProp);
//            EditorGUILayout.PropertyField(m_CastDrawTimeProp);
//            EditorGUILayout.PropertyField(m_DamageProp);

//            if (m_WeaponTypeProp.intValue == (int)Weapon.WeaponType.Projectile)
//            {
//                EditorGUILayout.PropertyField(m_ProjectilePrefabProp);
//                EditorGUILayout.PropertyField(m_ProjectileLaunchForceProp);
//            }

//            EditorGUILayout.PropertyField(m_EndPointProp);
//            EditorGUILayout.PropertyField(m_AdvancedSettingsProp, new GUIContent("Advance Settings"), true);
//            EditorGUILayout.PropertyField(m_FireSpellAnimationClipProp);
//            EditorGUILayout.PropertyField(m_FireArrowAnimationClipProp);
//            EditorGUILayout.PropertyField(m_CastSpellAnimationClipProp);
//            EditorGUILayout.PropertyField(m_DrawBowAnimationClipProp);
//            EditorGUILayout.PropertyField(m_FireSpellAudioClipProp);
//            EditorGUILayout.PropertyField(m_FireArrowAudioClipProp);
//            EditorGUILayout.PropertyField(m_CastSpellAudioClipProp);
//            EditorGUILayout.PropertyField(m_DrawBowAudioClipProp);

//            //if (m_WeaponTypeProp.intValue == (int)Weapon.WeaponType.Raycast)
//            //{
//            //    //EditorGUILayout.PropertyField(m_PrefabRayTrailProp);
//            //}

//            //EditorGUILayout.PropertyField(m_AmmoDisplayProp);
//            //EditorGUILayout.PropertyField(m_DisabledOnEmpty);

//            serializedObject.ApplyModifiedProperties();
//        }
//    }
}
//#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    static int s_CurrentLevel = -1;

    public GameObject[] StartPrefabs;
   
   // public AudioSource BGMPlayer;
   // public AudioClip EndGameSound;

    public float RunTime => m_Timer;
    public int TargetCount => m_TargetCount;
    public int DestroyedTarget => m_TargetDestroyed;
    public int Score => m_Score;

    float m_Timer;
    bool m_TimerRunning = false;

    int m_TargetCount;
    int m_TargetDestroyed;

    int m_Score = 1;

    void Awake()
    {
        Instance = this;
        foreach (var prefab in StartPrefabs)
        {
            Instantiate(prefab);
        }

        PoolSystem.Create();
    }

    void Start()
    {
        //WorldAudioPool.Init();

        RetrieveTargetsCount();

        GameSystemInfo.Instance.UpdateTimer(0);
    }

    public void ResetTimer()
    {
        m_Timer = 0.0f;
    }

    public void StartTimer()
    {
        m_TimerRunning = true;
    }

    public void StopTimer()
    {
        m_TimerRunning = false;
    }

    public void FinishRun()
    {
        //BGMPlayer.clip = EndGameSound;
        //BGMPlayer.loop = false;
        //BGMPlayer.Play();

        //Controller.Instance.DisplayCursor(true);
        //Controller.Instance.CanPause = false;
        FinalScoreUI.Instance.Display();
    }

    public void NextLevel()
    {
#if UNITY_EDITOR
        // Initial level generation
        if (s_CurrentLevel < 0)
        {
            var asyncOp = EditorSceneManager.LoadSceneAsyncInPlayMode(EditorSceneManager.GetActiveScene().path, new LoadSceneParameters(LoadSceneMode.Single));
            return;
        }
#endif
        s_CurrentLevel += 1;
        m_Score += 1;
        GameSystemInfo.Instance.UpdateScore(m_Score);

        // Handle the next level
        // Could possibly just use a multiplier to make harder
        //var op = EditorSceneManager.LoadSceneAsyncInPlayMode(GameDatabase.Instance.scenes[s_CurrentLevel], new LoadSceneParameters(LoadSceneMode.Single));
        //SceneManager.LoadScene(GameDatabase.Instance.scenes[s_CurrentLevel]);
    }

    void RetrieveTargetsCount()
    {
        //var targets = Resources.FindObjectsOfTypeAll<Target>();
        // OR var targets = get targets by tags

        //m_TargetCount = length of targets;
        m_TargetDestroyed = 0;
        m_Score = 0;

        GameSystemInfo.Instance.UpdateScore(0);
        //LevelSelectionUI.Instance.Init();
    }

    void Update()
    {
        if (m_TimerRunning)
        {
            m_Timer += Time.deltaTime;

            GameSystemInfo.Instance.UpdateTimer(m_Timer);
        }

        //Transform playerTransform = Controller.Instance.transform;

        //UI Update
        //MinimapUI.Instance.UpdateForPlayerTransform(playerTransform);
    }

    public void TargetDestroyed()
    {
        m_TargetDestroyed += 1;
    }
}
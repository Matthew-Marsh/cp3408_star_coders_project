using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // World Values to track
    private int levelNumber = 1;
    private int numberOfKeys = 0;

    // UI Menus
    private Canvas startMenuUI;
    private Canvas endLevelUI;
    private Canvas pauseMenuUI;
    private Canvas deathMenuUI;
    private Canvas gamePlayUI;

    [Header("Scene")]
    public string sceneToLoad;

    // Set up audio for UI Menus
    [Header("Audio")]
    private WorldMusicPlayer worldMusicPlayer;
    private AudioSource gameManagerAudioSource;
    public AudioClip[] startMenuAudioClips;
    public AudioClip selectButtonAudioClip;
    public AudioClip onDeathAudioClip;

    private Scene currentScene;
    private string currentSceneName;
    private string logFilePath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Get Game Manager Audio source
        gameManagerAudioSource = GetComponent<AudioSource>();

        // Find active scene and its name
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;

        // Determine intial actions based on scene name
        if (currentSceneName == "StartMenu")
        {
            Debug.Log("Start Menu Awake");
            PlayAudioClip(GetRandomAudioClip(startMenuAudioClips), true);
        }
        // Start scene sets all relevant GUI
        if (currentSceneName == "StartScene")
        {
            Debug.Log("Start Scene Awake");
            InitialiseUI();
            activateUI("gamePlayUI");
        }
    }

    private void Start()
    {
        Debug.Log("Create Debug Log File.");
        LogMessage("Start of Debug File.");
        LogMessage("Checking Start Menu Camera...");
        // Find and log active camera - Build testing
        Camera camera = FindObjectOfType<Camera>();
        if (camera != null)
        {
            LogMessage("Checking Components...");
            Component[] scripts = camera.GetComponents<MonoBehaviour>();
            LogMessage("Active Camera: " + camera.name);
            foreach (Component script in scripts)
            {
                LogMessage("Camera script attached: " + script.GetType().Name);
            }
        }
        else
        {
            Debug.Log("No camera found.");
        }
    }

    // On start menu if Play is selected continue saved from save
    public void ContinueGame()
    {
        levelNumber = PlayerPrefs.HasKey("LevelNumber") ? PlayerPrefs.GetInt("LevelNumber") : 1;
        StartGame();
    }

    // On start menu if New Game is selected
    public void StartNewGame()
    {
        ResetLevelNumber();
        StartGame();
    }

    // Start game
    public void StartGame()
    {
        Debug.Log("Start Game Called.");

        // Click menu item and load scene
        if (gameManagerAudioSource == null)
        {
            Debug.Log("One or more audio source/manager were null.");
            gameManagerAudioSource = GetComponent<AudioSource>();
        }
        gameManagerAudioSource.Stop();
        PlayAudioClip(selectButtonAudioClip, false);

        // Load and wait for scene load
        SceneManager.LoadScene(sceneToLoad);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // After scene has loaded execute remaining code
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded.");
        InitialiseUI();
        activateUI("gamePlayUI");

        // Find audio
        worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();
        gameManagerAudioSource = GetComponent<AudioSource>();
    }

    // Increment level
    public void incrementLevel()
    {
        levelNumber++;
    }

    // Display level complete UI
    public void DisplayLevelComplete()
    {
        activateUI("endLevelUI");
        incrementLevel();
        Debug.Log("Level Successfully Complete! Level Number: " + levelNumber);
    }

    // Select next level button from End Level UI
    public void LoadNextLevel()
    {
        Debug.Log("Load Next Level.");
        StartGame();
    }

    // Select try again button from Death Menu UI
    public void RestartLevel()
    {
        StartGame();
    }

    // Display Death Menu UI on player death
    public void GameOver()
    {
        if (worldMusicPlayer == null)
        {
            Debug.Log("One or more audio source/manager were null.");
            worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();
        }
        // Mute world music
        AudioSource worldMusicAudioSource = worldMusicPlayer.GetComponent<AudioSource>();
        worldMusicAudioSource.mute = true;

        PlayAudioClip(onDeathAudioClip, false);
        levelNumber = 0;
        numberOfKeys = 0;
        activateUI("deathMenuUI");
    }

    // Save level and keys, then exit game
    public void ExitGame()
    {
        Debug.Log("Exit Game");
        PlayAudioClip(selectButtonAudioClip, false);
        PlayerPrefs.SetInt("LevelNumber", levelNumber);
        Application.Quit();
    }

    // Open pause menu and pause game
    public void PauseGame()
    {
        LogMessage("Game Paused");
        activateUI("pauseMenuUI");

        Time.timeScale = 0f;  // Freeze time

        // Play click audio and set audio state to in menu
        if (worldMusicPlayer == null || gameManagerAudioSource == null)
        {
            Debug.Log("One or more audio source/manager were null.");
            worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();
            Debug.Log("World Audio Source: " + worldMusicPlayer.ToString());
            gameManagerAudioSource = GetComponent<AudioSource>();
            Debug.Log("Game Audio Source: " + gameManagerAudioSource.ToString());
        }
        AmbientMusicPlayer ambientMusicPlayer = FindObjectOfType<AmbientMusicPlayer>();
        ambientMusicPlayer.GetComponent<AudioSource>().mute = true;
        
        //PlayAudioClip(selectButtonAudioClip, false);
        worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.InMenu);

        // Find and log active camera - Build testing
        LogMessage("Checking Start Scene Camera...");
        Camera camera = FindObjectOfType<Camera>();
        if (camera != null)
        {
            LogMessage("Checking Components...");
            Component[] scripts = camera.GetComponents<MonoBehaviour>();
            LogMessage("Active Camera: " + camera.name);
            foreach (Component script in scripts)
            {
                LogMessage("Camera script attached: " + script.GetType().Name);
            }
        }
        else
        {
            LogMessage("No camera found.");
        }
    }

    // Resume game from pause menu
    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        if (worldMusicPlayer == null || gameManagerAudioSource == null)
        {
            Debug.Log("One or more audio source/manager were null.");
            worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();
            gameManagerAudioSource = GetComponent<AudioSource>();
        }

        //PlayAudioClip(selectButtonAudioClip, false);
        worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.Idle);

        AmbientMusicPlayer ambientMusicPlayer = FindObjectOfType<AmbientMusicPlayer>();
        ambientMusicPlayer.GetComponent<AudioSource>().mute = false;

        activateUI("gamePlayUI");
        Time.timeScale = 1f;

    }

    // Reset Level Counter
    public void ResetLevelNumber()
    {
        levelNumber = 1;
    }

    // Get the level number
    public int GetLevelNumber()
    {
        return levelNumber;
    }

    // Get number of keys
    public int GetNumberOfKeys()
    {
        return numberOfKeys;
    }

    // Increment keys
    public void IncrementKeys()
    {
        numberOfKeys++;
    }

    // Use key
    public bool UseKey()
    {
        if (numberOfKeys >= 1)
        {
            numberOfKeys--;
            return true;
        }
        else
        {
            return false;
        }
    }


    // Get a random audio clip from an array
    private AudioClip GetRandomAudioClip(AudioClip[] audioClips)
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    // Play audio clip, nominate looping or not
    private void PlayAudioClip(AudioClip clip, bool loopCondition)
    {
        if (gameManagerAudioSource == null)
        {
            Debug.Log("Audio source was missing.");
            gameManagerAudioSource = GetComponent<AudioSource>();
        }

        if (clip == null)
            return;

        gameManagerAudioSource.enabled = true;
        gameManagerAudioSource.mute = false;
        gameManagerAudioSource.loop = loopCondition;

        Debug.Log("Audio check.");
        Debug.Log("Clip: " + clip.ToString());
        Debug.Log("Game manager audio source: " + gameManagerAudioSource.ToString());
        Debug.Log("Game manager audio source enabled: " + gameManagerAudioSource.enabled);

        gameManagerAudioSource.PlayOneShot(clip);
    }

    private void InitialiseUI()
    {
        // Update scene name to active scene
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        Debug.Log("Current Scene Name: " + currentSceneName);

        if (currentSceneName == "StartScene")
        {
            gamePlayUI = Resources.FindObjectsOfTypeAll<Canvas>()
                .FirstOrDefault(canvas => canvas.name == "UIGamePlay");
            endLevelUI = Resources.FindObjectsOfTypeAll<Canvas>()
                .FirstOrDefault(canvas => canvas.name == "UIEndLevelMenu");
            pauseMenuUI = Resources.FindObjectsOfTypeAll<Canvas>()
                .FirstOrDefault(canvas => canvas.name == "UIPauseMenu");
            deathMenuUI = Resources.FindObjectsOfTypeAll<Canvas>()
                .FirstOrDefault(canvas => canvas.name == "UIDeathMenu");

            // Debug for each instance
            Debug.Log("GameManager looking for UI...");
            Debug.Log("GamePlayUI found: " + (gamePlayUI != null));
            Debug.Log("EndLevelUI found: " + (endLevelUI != null));
            Debug.Log("PauseMenuUI found: " + (pauseMenuUI != null));
            Debug.Log("DeathMenuUI found: " + (deathMenuUI != null));
        }
    }

    private void activateUI(string selectedUI)
    {
        if (gamePlayUI == null || endLevelUI == null || pauseMenuUI == null || deathMenuUI == null)
        {
            Debug.Log("One or more UI were null.");
            InitialiseUI();
        }

        // Debug which is UI is found
        Debug.Log("Current UI Components that are found.");
        Debug.Log("GamePlayUI found: " + (gamePlayUI != null));
        Debug.Log("EndLevelUI found: " + (endLevelUI != null));
        Debug.Log("PauseMenuUI found: " + (pauseMenuUI != null));
        Debug.Log("DeathMenuUI found: " + (deathMenuUI != null));

        // Reset UI
        gamePlayUI.gameObject.SetActive(false);
        endLevelUI.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(false);
        deathMenuUI.gameObject.SetActive(false);

        // Determine which UI to activate
        switch (selectedUI)
        {
            case "gamePlayUI":
                gamePlayUI.gameObject.SetActive(true);
                break;
            case "endLevelUI":
                endLevelUI.gameObject.SetActive(true);
                break;
            case "pauseMenuUI":
                pauseMenuUI.gameObject.SetActive(true);
                break;
            case "deathMenuUI":
                deathMenuUI.gameObject.SetActive(true);
                break;
            case "deactivateAll":
                break;
            default:
                gamePlayUI.gameObject.SetActive(true);
                break;
        }
    }

    private void LogMessage(string message)
    {
        if (string.IsNullOrEmpty(logFilePath))
        {
            logFilePath = Path.Combine(Application.dataPath, "FallenStarDebugLog.txt");

            //// Clear the log file if it already exists
            //if (File.Exists(logFilePath))
            //    File.Delete(logFilePath);
        }
        else
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(message);
            }
            Debug.Log("Logged: " + message);
        }
    }
}

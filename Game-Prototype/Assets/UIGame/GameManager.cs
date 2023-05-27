using System.Diagnostics.CodeAnalysis;
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
        gameManagerAudioSource = this.GetComponent<AudioSource>();

        // Find active scene and its name
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;

        // Determine intial actions based on scene name
        if (currentSceneName == "StartMenu")
        {
            Debug.Log("Start Menu");
            PlayAudioClip(GetRandomAudioClip(startMenuAudioClips), true);
        }
        // Start scene sets all relevant GUI
        if (currentSceneName == "StartScene")
        {
            Debug.Log("Start Scene");
            InitialiseUI();
            activateUI("gamePlayUI");
        }

    }

    // On start menu if Play is selected continue saved from save
    public void ContinueGame()
    {
        levelNumber = PlayerPrefs.HasKey("LevelNumber") ? PlayerPrefs.GetInt("LevelNumber") : 1;
        numberOfKeys = PlayerPrefs.HasKey("NumberOfKeys") ? PlayerPrefs.GetInt("NumberOfKeys") : 0;
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
        // Click menu item and load scene
        PlayAudioClip(selectButtonAudioClip, false);

        // Load and wait for scene load
        SceneManager.LoadScene(sceneToLoad);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // After scene has loaded execute remaining code
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitialiseUI();
        activateUI("gamePlayUI");

        // Find audio
        worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();
    }

        // Increment level
        public void incrementLevel()
    {
        levelNumber++;
    }

    // Display level complete UI
    public void DisplayLevelComplete()
    {
        Debug.Log("Handle Level Success");
        incrementLevel();
        activateUI("endLevelUI");
    }

    // Select next level button from End Level UI
    public void LoadNextLevel()
    {
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
        PlayAudioClip(onDeathAudioClip, false);
        levelNumber = 0;
        activateUI("deathMenuUI");
    }

    // Save level and keys, then exit game
    public void ExitGame()
    {
        PlayAudioClip(selectButtonAudioClip, false);
        PlayerPrefs.SetInt("LevelNumber", levelNumber);
        PlayerPrefs.SetInt("NumberOfKeys", numberOfKeys);
        Application.Quit();
    }

    // Open pause menu and pause game
    public void PauseGame()
    {
        PlayAudioClip(selectButtonAudioClip, false);
        Time.timeScale = 0f;
        activateUI("pauseMenuUI");
        worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.InMenu);
    }

    // Resume game from pause menu
    public void ResumeGame()
    {
        PlayAudioClip(selectButtonAudioClip, false);
        Time.timeScale = 1f;
        activateUI("gamePlayUI");
        worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.Idle);
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
        gameManagerAudioSource.loop = loopCondition;

        if (clip == null)
            return;

        gameManagerAudioSource.PlayOneShot(clip);
    }

    private void InitialiseUI()
    {
        // Update scene name to active scene
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        Debug.Log("Curren Scene Name: " + currentSceneName);

        if (currentSceneName == "StartScene")
        {
            gamePlayUI = GameObject.Find("UIGamePlay").GetComponent<Canvas>();
            endLevelUI = GameObject.Find("UIEndLevelMenu").GetComponent<Canvas>();
            pauseMenuUI = GameObject.Find("UIPauseMenu").GetComponent<Canvas>();
            deathMenuUI = GameObject.Find("UIDeathMenu").GetComponent<Canvas>();

            // Debug for each instance
            Debug.Log("GameManager instance created.");
            Debug.Log("GamePlayUI found: " + (gamePlayUI != null));
            Debug.Log("EndLevelUI found: " + (endLevelUI != null));
            Debug.Log("PauseMenuUI found: " + (pauseMenuUI != null));
            Debug.Log("DeathMenuUI found: " + (deathMenuUI != null));
        }
    }

    private void activateUI(string selectedUI)
    {
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
}

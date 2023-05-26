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
    private string sceneName;

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

        // Start playing Start Menu Audio
        gameManagerAudioSource = this.GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (currentScene.name == "StartMenu")
        {
            PlayAudioClip(GetRandomAudioClip(startMenuAudioClips), true);
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
        SceneManager.LoadScene(sceneToLoad);

        // Find audio
        worldMusicPlayer = FindObjectOfType<WorldMusicPlayer>();

        // Assign UI instances to Game Manager
        gamePlayUI = GameObject.Find("UIGamePlay").GetComponent<Canvas>();
        Debug.Log(gamePlayUI.ToString());
        endLevelUI = GameObject.Find("UIEndLevelMenu").GetComponent<Canvas>();
        Debug.Log(endLevelUI.ToString());
        pauseMenuUI = GameObject.Find("UIPauseMenu").GetComponent<Canvas>();
        Debug.Log(pauseMenuUI.ToString());
        deathMenuUI = GameObject.Find("UIDeathMenu").GetComponent<Canvas>();
        Debug.Log(deathMenuUI.ToString());

        // Activate only the game play UI
        gamePlayUI.gameObject.SetActive(true);
        endLevelUI.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(false);
        deathMenuUI.gameObject.SetActive(false);
    }

    // Increment level
    public void incrementLevel()
    {
        levelNumber++;
    }

    // Display level complete UI
    public void HandleLevelSuccess()
    {
        Debug.Log("Handle Level Success");
        gamePlayUI.gameObject.SetActive(false);
        endLevelUI.gameObject.SetActive(true);
        incrementLevel();
    }

    // Select next level button from End Level UI
    public void LoadNextLevel()
    {
        PlayAudioClip(selectButtonAudioClip, false);
        endLevelUI.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneToLoad);
    }

    // Select try again button from Death Menu UI
    public void RestartLevel()
    {
        PlayAudioClip(selectButtonAudioClip, false);
        deathMenuUI.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneToLoad);
    }

    // Display Death Menu UI on player death
    public void GameOver()
    {
        PlayAudioClip(onDeathAudioClip, false);
        levelNumber = 0;
        gamePlayUI.gameObject.SetActive(false);
        deathMenuUI.gameObject.SetActive(true);
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
        gamePlayUI.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(true);
        worldMusicPlayer.SetWorldState(WorldMusicPlayer.WorldState.InMenu);
    }

    // Resume game from pause menu
    public void ResumeGame()
    {
        PlayAudioClip(selectButtonAudioClip, false);
        Time.timeScale = 1f;
        pauseMenuUI.gameObject.SetActive(false);
        gamePlayUI.gameObject.SetActive(true);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start scene sets all relevant GUI
        if (currentScene.name == "StartScene")
        {
            gamePlayUI = GameObject.Find("UIGamePlay").GetComponent<Canvas>();
            endLevelUI = GameObject.Find("UIEndLevelMenu").GetComponent<Canvas>();
            pauseMenuUI = GameObject.Find("UIPauseMenu").GetComponent<Canvas>();
            deathMenuUI = GameObject.Find("UIDeathMenu").GetComponent<Canvas>();

            // Activate only the game play UI
            gamePlayUI.gameObject.SetActive(true);
            endLevelUI.gameObject.SetActive(false);
            pauseMenuUI.gameObject.SetActive(false);
            deathMenuUI.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

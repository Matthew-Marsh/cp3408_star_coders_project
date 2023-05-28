using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [Header("Start Menu")]
    public string sceneToLoad;
    private AudioSource startMenuAudioSource;
    public AudioClip[] startMenuAudioClips;
    public AudioClip selectButtonAudioClip;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        startMenuAudioSource = GetComponent<AudioSource>();
        
    }

    public void StartGame()
    {
        startMenuAudioSource.loop = false;
        PlayStartMenuMusic(selectButtonAudioClip);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void StartNewGame()
    {
        gameManager.ResetLevelNumber();
        startMenuAudioSource.loop = false;
        PlayStartMenuMusic(selectButtonAudioClip);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ExitGame()
    {
        startMenuAudioSource.loop = false;
        PlayStartMenuMusic(selectButtonAudioClip);
        Application.Quit();
    }

    private AudioClip GetRandomAudioClip()
    {
        return startMenuAudioClips[Random.Range(0, startMenuAudioClips.Length)];
    }

    public void PlayStartMenuMusic(AudioClip clip)
    {
        if (startMenuAudioClips == null)
            return;
        
        startMenuAudioSource.PlayOneShot(clip);
    }


}

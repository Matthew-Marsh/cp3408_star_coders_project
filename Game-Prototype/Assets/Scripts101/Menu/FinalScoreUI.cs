using UnityEngine;
using UnityEngine.UI;

public class FinalScoreUI : MonoBehaviour
{
    public static FinalScoreUI Instance { get; private set; }
    
    public Text TargetDestroyed;
    public Text TimeSpent;
    public Text FinalScore;
    
    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Display()
    {
        gameObject.SetActive(true);
        
        float time = GameSystem.Instance.RunTime;
        int targetDestroyed = GameSystem.Instance.DestroyedTarget;
        int totalTarget = GameSystem.Instance.TargetCount;
            
        TargetDestroyed.text = targetDestroyed + "/" + totalTarget;
        TimeSpent.text = time.ToString("N2") + "s";
        FinalScore.text = GameSystem.Instance.Score.ToString("N");
    }
}
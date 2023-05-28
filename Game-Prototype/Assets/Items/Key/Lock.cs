using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Add to the door
public class Lock : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lock"))
        {
            if (gameManager.UseKey())
            {
                Destroy(this);
            }
        }
    }
}

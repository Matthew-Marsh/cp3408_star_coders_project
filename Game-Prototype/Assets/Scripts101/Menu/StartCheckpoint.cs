using UnityEngine;

// Add this to an empty game object with a box collider at the start of the level, where the player spawns
public class StartCheckpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameSystem.Instance.ResetTimer();
        GameSystem.Instance.StartTimer();
        Destroy(gameObject);
    }
}
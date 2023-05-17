using UnityEngine;

// Add this to an empty game object with a box collider at the start of the level, where it will spawn the player spawns
public class StartCheckpoint : MonoBehaviour
{
    public GameObject playerPrefab;
    void Awake()
    {
        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        GameSystem.Instance.ResetTimer();
        GameSystem.Instance.StartTimer();
        Destroy(gameObject);
    }
}
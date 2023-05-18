using UnityEngine;

// Add this to an empty game object with a box collider at the end of the level
public class EndCheckpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Controller>() == null)
            return;

        GameSystem.Instance.StopTimer();
        GameSystem.Instance.FinishRun();
        Destroy(gameObject);
    }
}
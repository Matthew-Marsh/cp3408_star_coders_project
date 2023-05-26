using UnityEngine;

public class KeyGenerator : MonoBehaviour
{
    public GameObject keyObject;
    public float dropChance = 0.5f;

    private void OnDestroy()
    {
        if(keyObject != null && Random.value < dropChance)
        {
            Instantiate(keyObject, transform.position, Quaternion.identity);
        }
    }
}

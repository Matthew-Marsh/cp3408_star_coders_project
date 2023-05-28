using UnityEngine;

public class KeyGenerator : MonoBehaviour
{
    public GameObject dropObject;
    public GameObject bonusDropObject;
    public float dropChance = 0.5f;
    public float bonusDropChance = 0.1f;
    private bool hasDropped = false;

    public void CheckDrop()
    {
        if (!hasDropped && Random.value < dropChance)
        {
            Debug.Log("Key dropped.");
            DropObject();
            hasDropped = true;
        }
    }

    private void DropObject()
    {
        Instantiate(dropObject, transform.position, Quaternion.identity);
        if (!hasDropped && Random.value < dropChance)
        {
            Instantiate(bonusDropObject, transform.position, Quaternion.identity);
        }
    }
}

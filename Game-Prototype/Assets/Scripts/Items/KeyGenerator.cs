using UnityEngine;

public class KeyGenerator : MonoBehaviour
{
    public GameObject dropObject;
    public float dropChance = 0.5f;
    public float dropDelay = 10.0f;
    private bool hasDropped = false;

    private void Start()
    {
        InvokeRepeating("CheckDrop", dropDelay, dropDelay);
    }

    public void CheckDrop()
    {
        if (!hasDropped && Random.value < dropChance)
        {
            DropObject();
            hasDropped = true;
        }
    }

    private void DropObject()
    {
        Instantiate(dropObject, transform.position, Quaternion.identity);
    }
}

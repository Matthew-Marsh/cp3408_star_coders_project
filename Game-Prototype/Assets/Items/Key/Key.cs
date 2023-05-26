using UnityEngine;
using UnityEngine.UI;

//  Add to key
public class Key : MonoBehaviour
{
    public string keyType;
    public Text KeyNameText;

    void OnEnable()
    {
        KeyNameText.text = keyType;
    }

    void OnTriggerEnter(Collider other)
    {
        var keychain = other.GetComponent<Keychain>();

        if (keychain != null)
        {
            keychain.GrabbedKey(keyType);
            Destroy(gameObject);
        }
    }
}
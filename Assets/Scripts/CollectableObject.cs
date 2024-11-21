using UnityEngine;

// Place this on collectible objects in the level
public class CollectibleObject : MonoBehaviour
{
    public static event System.Action<CollectibleObject> OnCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
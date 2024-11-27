using UnityEngine;

// Place this on collectible objects in the level
public class CollectibleObject : MonoBehaviour
{
    public static event System.Action<CollectibleObject> OnCollected;

    [Header("VFX Settings")]
    [SerializeField] private GameObject smokeVFXPrefab; // Reference to the smoke particle prefab

    private void Update()
    {
        // Rotate the object by 0.25 degrees on the Y-axis every frame
        transform.Rotate(0, 0.25f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the particle effect
            if (smokeVFXPrefab != null)
            {
                GameObject smokeVFX = Instantiate(smokeVFXPrefab, transform.position, Quaternion.identity);
                Destroy(smokeVFX, 2f); // Destroy the particle system after 2 seconds
            }

            // Trigger the collected event and destroy the object
            OnCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
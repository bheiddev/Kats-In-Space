using UnityEngine;

public class PowerCellHandler : MonoBehaviour
{
    public bool hasGreenPowerCell = false;
    public bool hasRedPowerCell = false;
    public bool hasYellowPowerCell = false;
    public float detectionRadius = 2f;
    public float rayHeightOffset = 1f;
    private PowerCellContainer nearestContainer = null;

    // References to particle system prefabs for each color
    public GameObject greenSmokePrefab;
    public GameObject yellowSmokePrefab;
    public GameObject redSmokePrefab;
    public GameObject grabPowerCellUI;
    public GameObject placePowerCellUI;

    public enum PowerCellType
    {
        Green,
        Yellow,
        Red
    }

    [SerializeField] private GameObject greenPowerCellUI;
    [SerializeField] private GameObject yellowPowerCellUI;
    [SerializeField] private GameObject redPowerCellUI;
    [SerializeField] private GameObject greyGreenPowerCellUI;
    [SerializeField] private GameObject greyYellowPowerCellUI;
    [SerializeField] private GameObject greyRedPowerCellUI;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip powerCellAudioClip; // Audio clip for power cell actions
    private AudioSource audioSource;

    void Start()
    {
        // Ensure an AudioSource is attached to the player object for sound playback
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        CheckForProximityToPowerCellsAndContainers();

        if (Input.GetKeyDown(KeyCode.F))
        {
            InteractWithPowerCellOrContainer();
        }
    }

    private void CheckForProximityToPowerCellsAndContainers()
    {
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;
        Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius);

        bool isNearPowerCell = false;
        nearestContainer = null;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("GreenPowerCell") || hit.CompareTag("YellowPowerCell") || hit.CompareTag("RedPowerCell"))
            {
                isNearPowerCell = true; // Player is near a power cell
            }
            else if (hit.CompareTag("PowerCellContainer"))
            {
                PowerCellContainer container = hit.GetComponent<PowerCellContainer>();
                if (container != null && !container.isPowered)
                {
                    if ((container.containerColor == PowerCellContainer.PowerCellColor.Green && hasGreenPowerCell) ||
                        (container.containerColor == PowerCellContainer.PowerCellColor.Yellow && hasYellowPowerCell) ||
                        (container.containerColor == PowerCellContainer.PowerCellColor.Red && hasRedPowerCell))
                    {
                        nearestContainer = container;
                    }
                }
            }
        }

        // Show the appropriate UI based on proximity
        grabPowerCellUI.SetActive(isNearPowerCell); // Show grab UI when near a power cell
        placePowerCellUI.SetActive(nearestContainer != null); // Show place UI when near a valid container
    }

    private void TogglePowerCellUI(PowerCellType cellType, bool isPickedUp)
    {
        switch (cellType)
        {
            case PowerCellType.Green:
                greenPowerCellUI.SetActive(isPickedUp);
                greyGreenPowerCellUI.SetActive(!isPickedUp);
                break;
            case PowerCellType.Yellow:
                yellowPowerCellUI.SetActive(isPickedUp);
                greyYellowPowerCellUI.SetActive(!isPickedUp);
                break;
            case PowerCellType.Red:
                redPowerCellUI.SetActive(isPickedUp);
                greyRedPowerCellUI.SetActive(!isPickedUp);
                break;
        }
    }

    void InteractWithPowerCellOrContainer()
    {
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;
        Collider[] hitColliders = Physics.OverlapSphere(detectionCenter, detectionRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("GreenPowerCell") && !hasGreenPowerCell)
            {
                InstantiateAndDestroyParticle(greenSmokePrefab, hit.transform.position);
                Destroy(hit.gameObject);
                hasGreenPowerCell = true;

                // Update UI
                TogglePowerCellUI(PowerCellType.Green, true);

                // Play audio when the power cell is picked up
                PlayPowerCellAudio();

                Debug.Log("Picked up Green Power Cell!");
                return;
            }
            else if (hit.CompareTag("YellowPowerCell") && !hasYellowPowerCell)
            {
                InstantiateAndDestroyParticle(yellowSmokePrefab, hit.transform.position);
                Destroy(hit.gameObject);
                hasYellowPowerCell = true;

                // Update UI
                TogglePowerCellUI(PowerCellType.Yellow, true);

                // Play audio when the power cell is picked up
                PlayPowerCellAudio();

                Debug.Log("Picked up Yellow Power Cell!");
                return;
            }
            else if (hit.CompareTag("RedPowerCell") && !hasRedPowerCell)
            {
                InstantiateAndDestroyParticle(redSmokePrefab, hit.transform.position);
                Destroy(hit.gameObject);
                hasRedPowerCell = true;

                // Update UI
                TogglePowerCellUI(PowerCellType.Red, true);

                // Play audio when the power cell is picked up
                PlayPowerCellAudio();

                Debug.Log("Picked up Red Power Cell!");
                return;
            }
            else if (hit.CompareTag("PowerCellContainer"))
            {
                PowerCellContainer container = hit.GetComponent<PowerCellContainer>();
                if (container != null && !container.isPowered)
                {
                    switch (container.containerColor)
                    {
                        case PowerCellContainer.PowerCellColor.Green:
                            if (hasGreenPowerCell)
                            {
                                container.PowerOn();
                                hasGreenPowerCell = false;

                                // Update UI
                                TogglePowerCellUI(PowerCellType.Green, false);

                                // Play audio when the power cell is placed
                                PlayPowerCellAudio();

                                Debug.Log("Powered Green Power Cell Container!");
                            }
                            break;
                        case PowerCellContainer.PowerCellColor.Yellow:
                            if (hasYellowPowerCell)
                            {
                                container.PowerOn();
                                hasYellowPowerCell = false;

                                // Update UI
                                TogglePowerCellUI(PowerCellType.Yellow, false);

                                // Play audio when the power cell is placed
                                PlayPowerCellAudio();

                                Debug.Log("Powered Yellow Power Cell Container!");
                            }
                            break;
                        case PowerCellContainer.PowerCellColor.Red:
                            if (hasRedPowerCell)
                            {
                                container.PowerOn();
                                hasRedPowerCell = false;

                                // Update UI
                                TogglePowerCellUI(PowerCellType.Red, false);

                                // Play audio when the power cell is placed
                                PlayPowerCellAudio();

                                Debug.Log("Powered Red Power Cell Container!");
                            }
                            break;
                    }
                    return;
                }
            }
        }
    }

    void InstantiateAndDestroyParticle(GameObject particlePrefab, Vector3 position)
    {
        if (particlePrefab != null)
        {
            // Add a 2f offset on the Y axis
            Vector3 offsetPosition = position + new Vector3(0, .40f, 0);
            Instantiate(particlePrefab, offsetPosition, Quaternion.identity);
        }
    }

    // Method to play the power cell audio
    private void PlayPowerCellAudio()
    {
        if (powerCellAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(powerCellAudioClip);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 detectionCenter = transform.position + Vector3.up * rayHeightOffset;
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);
    }
}
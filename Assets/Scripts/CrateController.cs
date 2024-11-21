using UnityEngine;

public class CrateController : MonoBehaviour
{
    public GameObject player; // Assign the player object in the Inspector
    public float openDistance = 2f;

    private Animation crateAnimation;
    private bool isOpen = false; // Track if the crate is currently open

    void Start()
    {
        crateAnimation = GetComponent<Animation>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= openDistance && !isOpen)
        {
            crateAnimation.Play("Crate_Open");
            isOpen = true;
        }
        else if (distance > openDistance && isOpen)
        {
            crateAnimation.Play("Crate_Close");
            isOpen = false;
        }
    }
}
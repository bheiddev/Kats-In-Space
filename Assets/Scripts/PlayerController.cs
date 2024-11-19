using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public Animator animator;
    public PlayerFootsteps playerFootsteps;
    public LayerMask whatIsGrounded;
    private Collider characterCollider; 
    public bool grounded;
    public LayerMask jumpBoxLayer;
    public GameObject characterMesh; // Reference to the player's mesh

    [Header("Movement Values")]
    float horizontalInput;
    float verticalInput;
    public float moveSpeed = 5f;
    Vector3 moveDirection;

    [Header("Jump-to-Box Settings")]
    public float rayDistance = 2f;
    public float jumpHeight = 2f;
    private bool isJumpingToBox = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterCollider = GetComponent<Collider>(); 
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    public void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > 10)
        {
            Vector3 limitedVel = flatVel.normalized * 10;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Update()
    {
        // Listen for the F key press and initiate the jump-to-box action
        if (Input.GetKeyDown(KeyCode.F) && !isJumpingToBox)
        {
            JumpToBox();
        }

        MyInput();

        // Set the ray origin to the center of the character's collider for a more accurate ground check
        Vector3 rayOrigin = characterCollider.bounds.center;

        // Raycast downward from the collider's center to check for grounding
        grounded = Physics.Raycast(rayOrigin, Vector3.down, characterCollider.bounds.extents.y + 0.2f, whatIsGrounded);
        Debug.DrawRay(rayOrigin, Vector3.down * (characterCollider.bounds.extents.y + 0.2f), grounded ? Color.green : Color.red);

        if (Mathf.Approximately(horizontalInput, 0f) && Mathf.Approximately(verticalInput, 0f))
        {
            animator.SetBool("IsRunning", false);
            playerFootsteps.StopWalking();
        }
        else
        {
            animator.SetBool("IsRunning", true);

            if (grounded)
            {
                playerFootsteps.StartWalking();
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
    }

    private void JumpToBox()
    {
        // Adjust the ray origin to be higher on the y-axis for better detection
        Vector3 rayOrigin = transform.position + new Vector3(0, .25f, 0);

        // Cast a ray from the adjusted position forward to detect a jump box
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, characterMesh.transform.forward, out hit, rayDistance, jumpBoxLayer))
        {
            // Start moving to the detected box
            StartCoroutine(MoveToBox(hit.transform));
        }
    }

    private IEnumerator MoveToBox(Transform boxTransform)
    {
        isJumpingToBox = true;
        animator.SetBool("Jump", true); // Trigger jump animation

        // Target position on top of the box
        Vector3 targetPosition = new Vector3(boxTransform.position.x, boxTransform.position.y + jumpHeight, boxTransform.position.z);

        // Smoothly move the player to the target position
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Reset after reaching the target
        animator.SetBool("Jump", false);
        isJumpingToBox = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (characterMesh != null)
        {
            // Adjusted ray origin for visualizing in the Scene view
            Vector3 rayOrigin = transform.position + new Vector3(0, .25f, 0);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(rayOrigin, characterMesh.transform.forward * rayDistance);
        }
    }
}
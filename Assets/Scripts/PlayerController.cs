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
    private Collider characterCollider; // Added reference to the character's collider
    public bool grounded;

    [Header("Movement Values")]
    float horizontalInput;
    float verticalInput;
    public float moveSpeed = 5f;
    Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterCollider = GetComponent<Collider>(); // Initialize character collider
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void Update()
    {
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
}
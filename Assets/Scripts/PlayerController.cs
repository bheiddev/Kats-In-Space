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
    public float playerHeight;
    public LayerMask whatIsGrounded;
    public bool grounded;


    [Header("Movement Values")]
    float horizontalInput;
    float verticalInput;
    public float moveSpeed = 5f;
    Vector3 moveDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void Update()
    {
        MyInput();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGrounded);
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.5f + 0.2f), grounded ? Color.green : Color.red);

        if (Mathf.Approximately(horizontalInput, 0f) && Mathf.Approximately(verticalInput, 0f))
        {
            animator.SetBool("IsRunning", false);
            playerFootsteps.StopWalking();
        }
        else
        {
            animator.SetBool("IsRunning", true);

            if (grounded == true)
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
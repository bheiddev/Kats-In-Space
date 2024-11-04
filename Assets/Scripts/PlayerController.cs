using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public Animator animator;

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

        if (Mathf.Approximately(horizontalInput, 0f) && Mathf.Approximately(verticalInput, 0f))
        {
            animator.SetBool("IsRunning", false); // If there is no movement input, mark the player as not running
        }
        else
        {
            animator.SetBool("IsRunning", true); // If there is movement input, mark the player as running
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
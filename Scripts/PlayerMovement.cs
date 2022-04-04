using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float onWeaponSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    private CharacterController controller;
    private Animator animator;
    private Player player;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            } else
            {
                animator.SetBool("Jump", false);
            }
        }

        if (moveDirection != Vector3.zero)
        {
            Walk(moveX, moveZ);
        }
        else if (moveDirection == Vector3.zero)
        {
            Idle();
        }

        moveDirection = moveDirection.normalized * moveSpeed;
        controller.Move(Time.deltaTime * moveDirection);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void Idle()
    {
        moveSpeed = 0;
        animator.SetFloat("xAxis", 0, 0.1f, Time.deltaTime);
        animator.SetFloat("yAxis", 0, 0.1f, Time.deltaTime);
    }
    private void Walk(float x, float y)
    {
        animator.SetFloat("xAxis", x, 0.1f, Time.deltaTime);
        animator.SetFloat("yAxis", y, 0.1f, Time.deltaTime);
        if (player.onHand) moveSpeed = onWeaponSpeed;
        else moveSpeed = walkSpeed;
    }
    private void Jump()
    {
        animator.SetBool("Jump", true);
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
    
}

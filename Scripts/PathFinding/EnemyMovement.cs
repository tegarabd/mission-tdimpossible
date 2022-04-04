using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float onWeaponSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    private CharacterController controller;
    private Animator animator;

    private Grid grid;

    private void Awake()
    {
        grid = GetComponentInChildren<Grid>();
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (grid.path != null && grid.path.Count > 0)
        {
            moveDirection = grid.WorldPositionFromNode(grid.path[0]);
        } 
        else
            moveDirection = Vector3.zero;
        /*transform.rotation = Quaternion.LookRotation(moveDirection);*/

        if (moveDirection != Vector3.zero)
        {
            Walk();
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
        animator.SetFloat("Blend", 0f);
        moveSpeed = 0;
    }
    private void Walk()
    {
        animator.SetFloat("Blend", 1f);
        moveSpeed = walkSpeed;
    }

    public void MoveToDead()
    {
        velocity.y = -20f;
        controller.Move(velocity * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public static bool moving;

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

    private BossGrid grid;
    public Transform player;

    private void Start()
    {
        grid = transform.parent.GetComponentInChildren<BossGrid>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        moving = false;
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

        if (moving && grid.path != null && grid.path.Count > 0)
        {
            Vector3 dir = grid.path[0].worldPosition - transform.position;
            if (Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z) <= 2f)
            {
                grid.path.RemoveAt(0);
            }
            moveDirection = new Vector3(Mathf.Clamp(dir.x, -1, 1), 0, Mathf.Clamp(dir.z, -1, 1));
        }
        else
        {
            moveDirection = Vector3.zero;
            Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPos);
        }
            
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

       /* velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);*/
    }
    private void Idle()
    {
        animator.SetFloat("Blend", 1f);
        moveSpeed = 0;
    }
    private void Walk()
    {
        animator.SetFloat("Blend", 0f);
        moveSpeed = walkSpeed;
    }

    public void MoveToDead()
    {
        velocity.y = -20f;
        controller.Move(velocity * Time.deltaTime);
    }
}

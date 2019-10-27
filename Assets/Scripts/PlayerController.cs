using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jumpForce;

    private float moveInput;
    private Rigidbody2D rb;

    public bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;

    public bool jumping;
    float jumpTime = 0;
    public float jumpWindow;
    private float prevJumpVelocity;

    public bool isSwinging = false;
    public Vector2 ropeHook;
    public float swingForce = 4f;
    private Animator _animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void OnUpdate(InputController io)
    {
        if (jumpTime < jumpWindow && prevJumpVelocity > 0 && rb.velocity.y == 0)
        {
            jumpTime = jumpWindow;
        }
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
        if (io.ActionKeyPressed && extraJumps > 0 && !io.DownKeyHeld)
        {
            jumping = true;
            jumpTime = 0;
            extraJumps--;
        }
        else if (io.ActionKeyPressed && extraJumps == 0 && isGrounded && !io.DownKeyHeld)
        {
            jumping = true;
            jumpTime = 0;
            _animator.SetBool("isJumping", jumping);
        }
        prevJumpVelocity = rb.velocity.y;
    }

    public void OnFixedUpdate(InputController io)
    {
        whatIsGround = LayerMask.GetMask("Ground");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        moveInput = io.GetHorizontalDirection();
        

        if (io.GetHorizontalDirection() < 0f || io.GetHorizontalDirection() > 0f)
        {
            //animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
            //playerSprite.flipX = horizontalInput < 0f;
            if (isSwinging)
            {
                //animator.SetBool("IsSwinging", true);

                // 1 - Get a normalized direction vector from the player to the hook point
                var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

                // 2 - Inverse the direction to get a perpendicular direction
                Vector2 perpendicularDirection;
                if (io.GetHorizontalDirection() < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                }

                var force = perpendicularDirection * swingForce;
                rb.AddForce(force, ForceMode2D.Force);
            }
            else
            {
                //animator.SetBool("IsSwinging", false);
                if (groundCheck)
                {
                    rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
                }
            }
        }
        else
        {
            //animator.SetBool("IsSwinging", false);
            //animator.SetFloat("Speed", 0f);
        }

        if (!isSwinging)
        {
            if (!groundCheck) return;

            if (io.ActionKeyHeld && jumpTime <= jumpWindow && jumping == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTime += Time.deltaTime;
            }
            else
            {
                jumping = false;
                _animator.SetBool("isJumping", jumping);
            }

            if (jumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }
}

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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnUpdate(InputController io)
    {
        if (jumpTime < jumpWindow && prevJumpVelocity > 0 && rb.velocity.y == 0)
        {
            jumpTime = jumpWindow;
        }
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }
        if (io.ActionKeyPressed && extraJumps > 0 && !io.DownKeyHeld)
        {
            jumping = true;
            jumpTime = 0;
            extraJumps--;
        }
        else if (io.ActionKeyPressed && extraJumps == 0 && isGrounded == true && !io.DownKeyHeld)
        {
            jumping = true;
            jumpTime = 0;
        }
        prevJumpVelocity = rb.velocity.y;
    }

    public void OnFixedUpdate(InputController io)
    {
        whatIsGround = LayerMask.GetMask("Ground");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = io.GetHorizontalDirection();
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        
        if (io.ActionKeyHeld && jumpTime <= jumpWindow && jumping == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime += Time.deltaTime;
        }
        else
        {
            jumping = false;
        }
    }
}

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


    //private Animator characterAnim;

    private void Start()
    {
        //characterAnim = gameObject.GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (jumpTime < jumpWindow && prevJumpVelocity > 0 && rb.velocity.y == 0)
        {
            jumpTime = jumpWindow;
        }

        if (isGrounded == false)
        {
            //characterAnim.SetBool("Grounded", false);
        }
        if (isGrounded == true)
        {

            //characterAnim.SetBool("Jumping", false);
            //characterAnim.SetBool("Grounded", true);
            extraJumps = extraJumpsValue;
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !Input.GetKey(KeyCode.DownArrow))
        {
            jumping = true;
            jumpTime = 0;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true && !Input.GetKey(KeyCode.DownArrow))
        {
            jumping = true;
            jumpTime = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
        {
            //characterAnim.SetTrigger("Right");
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp("d"))
        {
            //characterAnim.ResetTrigger("Right");
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            //characterAnim.SetTrigger("Left");
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp("a"))
        {
            //characterAnim.ResetTrigger("Left");
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("d") && Input.GetKey("a"))
        {
            //characterAnim.ResetTrigger("Right");
            //characterAnim.ResetTrigger("Left");
            if (moveInput > 0)
            {
                //characterAnim.SetTrigger("Right");
            }
            if (moveInput < 0)
            {
                //characterAnim.SetTrigger("Left");
            }

        }
        prevJumpVelocity = rb.velocity.y;
    }

    private void FixedUpdate()
    {
        whatIsGround = LayerMask.GetMask("Ground");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput != 0)
        {
            //characterAnim.SetBool("Walking", true);
        }
        else
        {
            //characterAnim.SetBool("Walking", false);

        }

        if (Input.GetKey(KeyCode.Space) && jumpTime <= jumpWindow && jumping == true)
        {
            //characterAnim.SetBool("Jumping", true);
            //characterAnim.SetBool("Grounded", false);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime += Time.deltaTime;
        }
        else
        {
            //characterAnim.SetBool("Jumping", false);
            jumping = false;
        }
    }
}

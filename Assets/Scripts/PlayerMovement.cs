using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //makes the Rigidbod2D 
    public Rigidbody2D rb;
    bool isFacingRight = true;
    //creates a title for the values below and shows it in the inspector 
    [Header("Movement")]
    //makes the move speed of the player 5.0 
    public float moveSpeed = 5f;
    float horizontalMovement;
    //creates a title for the values below and shows it in the inspector 
    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer  trailRenderer;
    //creates a title for the values below and shows it in the inspector    
    [Header("Jumping")]
    //sets a public float value of 10.0 
    public float jumpPower = 10f;

    //creates a title for the values below and shows it in the inspector 
    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    //creates a title for the values below and shows it in the inspector 
    [Header("Gravity")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        //makes the character move either left or right with multiplying the values of the following 
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        Gravity();
        Flip();

 
    }
    //Player gravity 
    private void Gravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //Makes our character fall faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));//caps out player fall speed so it doesn't fall any faster 
        }
        else
        {
            rb.gravityScale = baseGravity; //if we are not falling set back to base gravity 
        }
    }
    //The input actions that make the character move(right arrow or D) 
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if(context.performed && canDash)    
        {
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;
        float dashDirection = isFacingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocityY); //dash movement 

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocityY); //reset horizontal velocity 

        isDashing = false;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void Flip()
    {
        if((isFacingRight) && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    //The input action that make the character move(Left arrow or A) 
    public void Jump(InputAction.CallbackContext context) 
    {
        //if the character is grounded do the following 
        if (isGrounded())
        {
            //if the input action is fully performed do a full jump 
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            }
            //if the input action is half pressed or tapped do half the jump of a full jump 
            else if (context.canceled) 
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }
    //checks if the character is grounded 
    private bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer)) 
        {
            return true;
        }
        return false;
    }
    //frame to show where the ground check is and how big it is 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

    }
}

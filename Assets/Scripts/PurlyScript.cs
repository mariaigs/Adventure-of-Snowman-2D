using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PurlyScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float jumpForce = 7.0f;
    private Rigidbody2D rb;
    private bool facingRightDirection = true;
    private Animator animator;
    public bool isGrounded = true;
    private bool isLanding = false;
    private bool jumpRequested = false;

   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        // Handle horizontal movement
        float moveX = Input.GetAxis("Horizontal");
        // Set isRunning parameter when moving horizontally
        bool isMoving = Mathf.Abs(moveX) > 0.1f;
        animator.SetBool("isRunning", isMoving);
        // Handle Jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
            // Trigger jump animation sequence
            animator.SetTrigger("takeof");
            // Set isJumping to true to transition to the Jump state
            animator.SetBool("isJumping", true);

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX("Jump");
            }

        }
        // Flip the character based on movement direction
        FlipPlayer(moveX);
       
    }
    void FixedUpdate()
    {
        // Handle horizontal movement in FixedUpdate for physics
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (jumpRequested && isGrounded)
        {
            // Apply jump force - keep horizontal velocity and add vertical force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            jumpRequested = false;
        }
    }
    void FlipPlayer(float moveX)
    {
        if (moveX > 0 && !facingRightDirection || moveX < 0 && facingRightDirection)
        {
            facingRightDirection = !facingRightDirection;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    // Handle ground detection with collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            if (!isGrounded && AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX("WaterSplash");
            }

            isGrounded = true;
            // Set isJumping to false to transition to land animation
            animator.SetBool("isJumping", false);
            // Start the landing sequence
            StartCoroutine(HandleLanding());
        }
    }
    // Coroutine to handle the landing sequence
    IEnumerator HandleLanding()
    {
        isLanding = true;
        // Wait for the land animation to complete
        // You may need to adjust this time to match your animation duration
        yield return new WaitForSeconds(0.1f);
        isLanding = false;
    }

    
}
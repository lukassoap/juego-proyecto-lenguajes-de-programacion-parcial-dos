using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //variables to be declared for our 2D project
    public GameManager myGameManager;
    public Rigidbody2D rb;
    Animator animator;
    bool isFacingRight = true;
    //aqui cambia escena 
    public string nextSceneName;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 10f;
    bool isGrounded = false;
    int jumpsRemaining;
    public int maxJumps = 2;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    [Header("Stomp")]
    public float bounceForce = 8f;
    public Transform playerFeet;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myGameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            FlipSprite();
        }

        animator.SetFloat("yVelocidad", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);
    }

    private void ProcessGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void ProcessWallSlide()
    {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
            animator.SetBool("isWallSliding", isWallSliding);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("isWallSliding", isWallSliding);
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            float normalizedSpeed = Mathf.Abs(rb.linearVelocity.x / moveSpeed);
            if (normalizedSpeed < 0.05f)
                normalizedSpeed = 0f;
            animator.SetFloat("xVelocidad", normalizedSpeed);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (jumpsRemaining > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
            else if (isWallSliding)
            {
                isWallJumping = true;
                rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
                wallJumpTimer = 0f;
                animator.SetTrigger("jump");
                if (transform.localScale.x != wallJumpDirection)
                {
                    isFacingRight = !isFacingRight;
                    Vector3 ls = transform.localScale;
                    ls.x *= -1f;
                    transform.localScale = ls;
                }
                Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
            }
        }
        else if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void FlipSprite()
    {
        if (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    // --- Lógica de colisión con el enemigo ---
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto con el que colisionamos tiene el tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Calcula el punto de contacto
            ContactPoint2D contact = collision.contacts[0];

            // Si el punto de contacto está en la parte superior del enemigo (pisotón)
            // y la velocidad vertical del jugador es negativa (yendo hacia abajo)
            // Se puede usar el Vector de la normal para saber de donde viene el golpe
            if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f && rb.linearVelocity.y < 0)
            {
                // Llama a la función de muerte del enemigo
                EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.Die();
                }

                // Hace que el jugador rebote
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            }
            // Si el contacto no es un pisotón, el jugador muere
            else
            {
                Debug.Log("Player was hit! Restarting scene.");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                SoundEffectManager.Play("Loss"); // deberia funcionar 
                SceneManager.LoadScene(nextSceneName);
            }

        }
        if (collision.gameObject.CompareTag("DeadZone")){
            Debug.Log("Player hit the DeadZone! Restarting scene.");
            SoundEffectManager.Play("Loss"); // deberia funcionar 
            SceneManager.LoadScene(nextSceneName);
        }
    }
    // --- Fin de la lógica de colisión con el enemigo ---

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wallCheckPos.position, wallCheckSize);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemGood"))
        {
            Destroy(collision.gameObject);
            myGameManager.AddScore();
            SoundEffectManager.Play("Gems"); // deberia funcionar 

        }
    }
}

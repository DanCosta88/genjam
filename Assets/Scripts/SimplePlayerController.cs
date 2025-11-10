using UnityEngine;

/// <summary>
/// Controller semplice per un personaggio 2D side-scrolling.
/// Esempio di come interagire con il sistema di parallax scrolling.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Velocità di movimento orizzontale")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Tooltip("Forza di salto")]
    [SerializeField] private float jumpForce = 10f;
    
    [Tooltip("Può muoversi nell'aria")]
    [SerializeField] private bool canAirControl = true;

    [Header("Ground Check")]
    [Tooltip("Punto per il controllo del terreno")]
    [SerializeField] private Transform groundCheck;
    
    [Tooltip("Raggio del controllo del terreno")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    
    [Tooltip("Layer del terreno")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation")]
    [Tooltip("Sprite Renderer per il flip")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Parallax Integration")]
    [Tooltip("Riferimento al ParallaxBackground (opzionale)")]
    [SerializeField] private ParallaxBackground parallaxBackground;
    
    [Tooltip("Modifica la velocità del parallax in base al movimento")]
    [SerializeField] private bool controlParallaxSpeed = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;
    private float baseParallaxSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Salva la velocità base del parallax
        if (parallaxBackground != null)
            baseParallaxSpeed = 3f; // Valore di default
    }

    private void Update()
    {
        // Input
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Flip dello sprite
        if (spriteRenderer != null && horizontalInput != 0)
        {
            spriteRenderer.flipX = horizontalInput < 0;
        }

        // Controlla il parallax in base al movimento
        if (controlParallaxSpeed && parallaxBackground != null)
        {
            float speedMultiplier = Mathf.Abs(horizontalInput);
            parallaxBackground.SetSpeedMultiplier(speedMultiplier);
        }
    }

    private void FixedUpdate()
    {
        // Controlla se è a terra
        CheckGround();

        // Movimento
        if (isGrounded || canAirControl)
        {
            Move();
        }
    }

    private void Move()
    {
        // Movimento orizzontale
        float targetVelocity = horizontalInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocity, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void CheckGround()
    {
        if (groundCheck == null)
        {
            // Fallback: usa la posizione del player
            Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
            isGrounded = Physics2D.OverlapCircle(checkPosition, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizza il ground check
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 checkPos = groundCheck != null ? groundCheck.position : transform.position + Vector3.down * 0.5f;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }

    /// <summary>
    /// Imposta la velocità di movimento
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    /// <summary>
    /// Imposta la forza di salto
    /// </summary>
    public void SetJumpForce(float force)
    {
        jumpForce = force;
    }

    /// <summary>
    /// Abilita o disabilita il controllo in aria
    /// </summary>
    public void SetAirControl(bool enabled)
    {
        canAirControl = enabled;
    }

    /// <summary>
    /// Ritorna true se il player è a terra
    /// </summary>
    public bool IsGrounded()
    {
        return isGrounded;
    }

    /// <summary>
    /// Ritorna l'input orizzontale corrente
    /// </summary>
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
}


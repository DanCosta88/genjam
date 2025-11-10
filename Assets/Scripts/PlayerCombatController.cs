using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controller completo per il player con movimento, salto, attacco e animazioni.
/// Controlli: Frecce per muoversi, Spazio per saltare, F per attaccare
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCombatController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Velocità di movimento orizzontale")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Tooltip("Forza di salto")]
    [SerializeField] private float jumpForce = 12f;
    
    [Tooltip("Moltiplicatore per la corsa (opzionale)")]
    [SerializeField] private float runMultiplier = 1.5f;

    [Header("Ground Check")]
    [Tooltip("Punto per controllare se il player è a terra")]
    [SerializeField] private Transform groundCheck;
    
    [Tooltip("Raggio del controllo terreno")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    
    [Tooltip("Layer del terreno")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Combat Settings")]
    [Tooltip("Danno inflitto dall'attacco")]
    [SerializeField] private int attackDamage = 10;
    
    [Tooltip("Raggio dell'attacco")]
    [SerializeField] private float attackRange = 1.5f;
    
    [Tooltip("Punto da cui parte l'attacco")]
    [SerializeField] private Transform attackPoint;
    
    [Tooltip("Layer dei nemici")]
    [SerializeField] private LayerMask enemyLayer;
    
    [Tooltip("Durata dell'animazione di attacco in secondi")]
    [SerializeField] private float attackDuration = 0.5f;

    [Header("Visual Settings")]
    [Tooltip("Sprite Renderer del player")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Sprite Settings")]
    [Tooltip("Sprite quando il player è fermo")]
    [SerializeField] private Sprite idleSprite;
    
    [Tooltip("Sprite quando il player cammina")]
    [SerializeField] private Sprite walkSprite;
    
    [Tooltip("Sprite quando il player salta")]
    [SerializeField] private Sprite jumpSprite;
    
    [Tooltip("Sprite quando il player attacca")]
    [SerializeField] private Sprite attackSprite;

    // Components
    private Rigidbody2D rb;
    private Animator animator;
    
    // State
    private bool isGrounded;
    private bool isAttacking;
    private float attackTimer;
    private float horizontalInput;
    private bool isFacingRight = true;

    // Animation parameter hashes (per performance)
    private int animIsMoving;
    private int animIsGrounded;
    private int animAttack;
    private int animVerticalVelocity;

    // Input actions
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Cache animation parameter hashes (se hai Animator)
        if (animator != null)
        {
            animIsMoving = Animator.StringToHash("IsMoving");
            animIsGrounded = Animator.StringToHash("IsGrounded");
            animAttack = Animator.StringToHash("Attack");
            animVerticalVelocity = Animator.StringToHash("VerticalVelocity");
        }
    }

    private void OnEnable()
    {
        // Input con tastiera diretta (più semplice per iniziare)
        // Non richiede PlayerInput component
    }

    private void OnDisable()
    {
        // Cleanup se necessario
    }

    private void Update()
    {
        // Input usando Keyboard direttamente (Input System)
        horizontalInput = 0f;
        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                horizontalInput = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                horizontalInput = 1f;

            // Salto (Spazio)
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && !isAttacking)
            {
                Jump();
            }

            // Attacco (F)
            if (Keyboard.current.fKey.wasPressedThisFrame && !isAttacking)
            {
                Attack();
            }
        }

        // Update attack timer
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
            }
        }

        // Flip sprite based on movement direction
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        // Update animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // Controlla se è a terra
        CheckGround();

        // Movimento (solo se non sta attaccando)
        if (!isAttacking)
        {
            Move();
        }
        else
        {
            // Rallenta durante l'attacco
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
        }
    }

    private void Move()
    {
        // Calcola velocità target
        float targetVelocity = horizontalInput * moveSpeed;
        
        // Applica il movimento
        rb.linearVelocity = new Vector2(targetVelocity, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        
        // Opzionale: suono di salto
        // AudioManager.Instance.PlaySFX("Jump");
    }

    private void Attack()
    {
        isAttacking = true;
        attackTimer = attackDuration;

        // Trigger animation
        if (animator != null)
        {
            animator.SetTrigger(animAttack);
        }

        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(GetAttackPoint(), attackRange, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            // Applica danno al nemico
            // Per ora solo log, puoi implementare un sistema di health
            Debug.Log($"Hit enemy: {enemy.name} for {attackDamage} damage!");
            
            // Se il nemico ha un component Health, chiamalo
            var enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }

        // Opzionale: suono di attacco
        // AudioManager.Instance.PlaySFX("Attack");
    }

    private void CheckGround()
    {
        Vector2 checkPosition = groundCheck != null ? groundCheck.position : (Vector2)transform.position + Vector2.down * 0.5f;
        isGrounded = Physics2D.OverlapCircle(checkPosition, groundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        
        // Flip usando la scale (più performante che ruotare)
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void UpdateAnimator()
    {
        // Sistema semplice: cambia sprite in base allo stato
        UpdateSprite();

        // Se hai un Animator configurato, aggiorna anche quello
        if (animator == null) return;

        // IsMoving: true se si sta muovendo orizzontalmente
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;
        animator.SetBool(animIsMoving, isMoving);

        // IsGrounded
        animator.SetBool(animIsGrounded, isGrounded);

        // VerticalVelocity per animazione di salto/caduta
        animator.SetFloat(animVerticalVelocity, rb.linearVelocity.y);
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        // Priorità: Attack > Jump > Walk > Idle
        if (isAttacking && attackSprite != null)
        {
            spriteRenderer.sprite = attackSprite;
        }
        else if (!isGrounded && jumpSprite != null)
        {
            spriteRenderer.sprite = jumpSprite;
        }
        else if (Mathf.Abs(horizontalInput) > 0.1f && walkSprite != null)
        {
            spriteRenderer.sprite = walkSprite;
        }
        else if (idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private Vector2 GetAttackPoint()
    {
        if (attackPoint != null)
        {
            return attackPoint.position;
        }
        
        // Default: davanti al player
        float offsetX = isFacingRight ? attackRange * 0.5f : -attackRange * 0.5f;
        return (Vector2)transform.position + new Vector2(offsetX, 0);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizza il ground check
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 checkPos = groundCheck != null ? groundCheck.position : transform.position + Vector3.down * 0.5f;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);

        // Visualizza il range di attacco
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GetAttackPoint(), attackRange);
    }

    // Public getters
    public bool IsGrounded => isGrounded;
    public bool IsAttacking => isAttacking;
    public bool IsFacingRight => isFacingRight;
    public float HorizontalInput => horizontalInput;
}

/// <summary>
/// Componente semplice per la salute dei nemici
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        // Qui puoi aggiungere effetti di morte, drop items, ecc.
        Destroy(gameObject);
    }

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
}


using UnityEngine;

/// <summary>
/// Controller completo per il player con movimento, salto, attacco e animazioni.
/// Controlli: Frecce per muoversi, Spazio per saltare, F per attaccare
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Cache animation parameter hashes
        animIsMoving = Animator.StringToHash("IsMoving");
        animIsGrounded = Animator.StringToHash("IsGrounded");
        animAttack = Animator.StringToHash("Attack");
        animVerticalVelocity = Animator.StringToHash("VerticalVelocity");
    }

    private void Update()
    {
        // Input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        // Salto (Spazio)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isAttacking)
        {
            Jump();
        }

        // Attacco (F)
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            Attack();
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
        if (animator == null) return;

        // IsMoving: true se si sta muovendo orizzontalmente
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;
        animator.SetBool(animIsMoving, isMoving);

        // IsGrounded
        animator.SetBool(animIsGrounded, isGrounded);

        // VerticalVelocity per animazione di salto/caduta
        animator.SetFloat(animVerticalVelocity, rb.linearVelocity.y);
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


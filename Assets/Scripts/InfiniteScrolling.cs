using UnityEngine;

/// <summary>
/// Gestisce lo scrolling infinito del background per giochi 2D side-scrolling.
/// Il background si ripete seamlessly quando raggiunge una certa posizione.
/// </summary>
public class InfiniteScrolling : MonoBehaviour
{
    [Header("Scrolling Settings")]
    [Tooltip("Velocità di scrolling (unità per secondo)")]
    [SerializeField] private float scrollSpeed = 2f;
    
    [Tooltip("Larghezza del background per calcolare il punto di reset")]
    [SerializeField] private float backgroundWidth = 20f;
    
    [Tooltip("Abilita lo scrolling automatico")]
    [SerializeField] private bool autoScroll = true;
    
    [Tooltip("Usa l'input del giocatore per controllare lo scrolling")]
    [SerializeField] private bool usePlayerInput = false;

    [Header("Parallax Settings")]
    [Tooltip("Fattore di parallax (0 = nessun movimento, 1 = movimento completo)")]
    [SerializeField] private float parallaxFactor = 1f;

    private Vector3 startPosition;
    private float singleBackgroundWidth;

    private void Start()
    {
        startPosition = transform.position;
        
        // Calcola la larghezza del background basandosi sullo sprite renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            singleBackgroundWidth = spriteRenderer.sprite.bounds.size.x * transform.localScale.x;
            backgroundWidth = singleBackgroundWidth;
        }
    }

    private void Update()
    {
        float moveAmount = 0f;

        // Scrolling automatico
        if (autoScroll)
        {
            moveAmount = -scrollSpeed * Time.deltaTime * parallaxFactor;
        }

        // Input del giocatore (opzionale)
        if (usePlayerInput)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            moveAmount += -horizontalInput * scrollSpeed * Time.deltaTime * parallaxFactor;
        }

        // Muovi il background
        transform.position += new Vector3(moveAmount, 0, 0);

        // Reset della posizione quando il background è completamente fuori schermo
        if (transform.position.x <= startPosition.x - backgroundWidth)
        {
            Vector3 resetPosition = transform.position;
            resetPosition.x += backgroundWidth;
            transform.position = resetPosition;
        }
        else if (transform.position.x >= startPosition.x + backgroundWidth)
        {
            Vector3 resetPosition = transform.position;
            resetPosition.x -= backgroundWidth;
            transform.position = resetPosition;
        }
    }

    /// <summary>
    /// Imposta la velocità di scrolling a runtime
    /// </summary>
    public void SetScrollSpeed(float speed)
    {
        scrollSpeed = speed;
    }

    /// <summary>
    /// Abilita o disabilita lo scrolling automatico
    /// </summary>
    public void SetAutoScroll(bool enabled)
    {
        autoScroll = enabled;
    }

    /// <summary>
    /// Imposta il fattore di parallax
    /// </summary>
    public void SetParallaxFactor(float factor)
    {
        parallaxFactor = Mathf.Clamp01(factor);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizza la larghezza del background nell'editor
        Gizmos.color = Color.yellow;
        Vector3 gizmoPos = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawWireCube(gizmoPos, new Vector3(backgroundWidth, 10, 0));
    }
}


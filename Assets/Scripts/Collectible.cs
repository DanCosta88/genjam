using UnityEngine;

/// <summary>
/// Script per oggetti collezionabili (monete, power-up, ecc.)
/// </summary>
public class Collectible : MonoBehaviour
{
    public enum CollectibleType
    {
        Coin,
        PowerUp,
        Life
    }

    [Header("Collectible Settings")]
    [SerializeField] private CollectibleType type = CollectibleType.Coin;
    [SerializeField] private int scoreValue = 100;
    [SerializeField] private AudioClip collectSound;

    [Header("Visual")]
    [SerializeField] private bool rotateAnimation = true;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private bool bobAnimation = true;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float bobHeight = 0.2f;

    private Vector3 startPosition;
    private float bobTimer;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Animazione rotazione
        if (rotateAnimation)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        // Animazione bobbing (su e giù)
        if (bobAnimation)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float yOffset = Mathf.Sin(bobTimer) * bobHeight;
            transform.position = startPosition + new Vector3(0, yOffset, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Controlla se è il player
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (GameManager.Instance != null)
        {
            switch (type)
            {
                case CollectibleType.Coin:
                    GameManager.Instance.AddCoin(1);
                    GameManager.Instance.AddScore(scoreValue);
                    break;

                case CollectibleType.PowerUp:
                    GameManager.Instance.AddScore(scoreValue);
                    // Qui puoi aggiungere logica per power-up
                    break;

                case CollectibleType.Life:
                    GameManager.Instance.AddLife(1);
                    GameManager.Instance.AddScore(scoreValue);
                    break;
            }
        }

        // Suono
        if (collectSound != null)
        {
            // AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Distruggi oggetto
        Destroy(gameObject);
    }
}


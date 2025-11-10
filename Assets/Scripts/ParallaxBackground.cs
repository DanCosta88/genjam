using UnityEngine;

/// <summary>
/// Gestisce multiple layer di parallax per creare profondità nella scena.
/// Ogni layer si muove a velocità diversa creando l'effetto parallax.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        [Tooltip("Il GameObject del layer (deve avere SpriteRenderer e InfiniteScrolling)")]
        public GameObject layerObject;
        
        [Tooltip("Velocità relativa del layer (0 = statico, 1 = velocità normale)")]
        [Range(0f, 1f)]
        public float parallaxSpeed = 0.5f;
        
        [Tooltip("Offset iniziale sull'asse Z per il sorting")]
        public float zOffset = 0f;
    }

    [Header("Parallax Layers")]
    [Tooltip("Lista di layer ordinati dal più lontano al più vicino")]
    [SerializeField] private ParallaxLayer[] layers;

    [Header("Global Settings")]
    [Tooltip("Velocità base di scrolling")]
    [SerializeField] private float baseScrollSpeed = 2f;
    
    [Tooltip("Moltiplicatore globale per tutte le velocità")]
    [SerializeField] private float speedMultiplier = 1f;

    private void Start()
    {
        InitializeLayers();
    }

    private void InitializeLayers()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].layerObject == null) continue;

            // Imposta la posizione Z per il sorting
            Vector3 pos = layers[i].layerObject.transform.position;
            pos.z = layers[i].zOffset;
            layers[i].layerObject.transform.position = pos;

            // Configura lo script InfiniteScrolling
            InfiniteScrolling scrollScript = layers[i].layerObject.GetComponent<InfiniteScrolling>();
            if (scrollScript != null)
            {
                scrollScript.SetScrollSpeed(baseScrollSpeed * speedMultiplier);
                scrollScript.SetParallaxFactor(layers[i].parallaxSpeed);
            }
            else
            {
                Debug.LogWarning($"Layer {layers[i].layerObject.name} non ha il component InfiniteScrolling!");
            }
        }
    }

    /// <summary>
    /// Cambia la velocità globale di scrolling
    /// </summary>
    public void SetGlobalSpeed(float speed)
    {
        baseScrollSpeed = speed;
        UpdateLayerSpeeds();
    }

    /// <summary>
    /// Imposta il moltiplicatore di velocità
    /// </summary>
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        UpdateLayerSpeeds();
    }

    private void UpdateLayerSpeeds()
    {
        foreach (var layer in layers)
        {
            if (layer.layerObject == null) continue;
            
            InfiniteScrolling scrollScript = layer.layerObject.GetComponent<InfiniteScrolling>();
            if (scrollScript != null)
            {
                scrollScript.SetScrollSpeed(baseScrollSpeed * speedMultiplier);
            }
        }
    }

    /// <summary>
    /// Pausa o riprendi lo scrolling di tutti i layer
    /// </summary>
    public void PauseScrolling(bool pause)
    {
        foreach (var layer in layers)
        {
            if (layer.layerObject == null) continue;
            
            InfiniteScrolling scrollScript = layer.layerObject.GetComponent<InfiniteScrolling>();
            if (scrollScript != null)
            {
                scrollScript.SetAutoScroll(!pause);
            }
        }
    }
}


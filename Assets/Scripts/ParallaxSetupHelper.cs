using UnityEngine;

/// <summary>
/// Script helper per configurare rapidamente un sistema di parallax scrolling.
/// Attach questo script a un GameObject vuoto per creare automaticamente la struttura necessaria.
/// </summary>
public class ParallaxSetupHelper : MonoBehaviour
{
    [Header("Background Setup")]
    [Tooltip("Sprite del background da usare")]
    [SerializeField] private Sprite backgroundSprite;
    
    [Tooltip("Numero di copie del background per il loop")]
    [SerializeField] private int numberOfCopies = 2;
    
    [Tooltip("Scala del background")]
    [SerializeField] private Vector3 backgroundScale = Vector3.one * 2;
    
    [Tooltip("Z offset per il sorting")]
    [SerializeField] private float zOffset = 10f;

    [Header("Scrolling Settings")]
    [Tooltip("Velocità base di scrolling")]
    [SerializeField] private float scrollSpeed = 3f;
    
    [Tooltip("Abilita scrolling automatico")]
    [SerializeField] private bool autoScroll = true;

    [Header("Material Settings")]
    [Tooltip("Material da usare per il rendering 2D (lascia vuoto per default)")]
    [SerializeField] private Material spriteMaterial;

    [ContextMenu("Create Parallax System")]
    public void CreateParallaxSystem()
    {
        if (backgroundSprite == null)
        {
            Debug.LogError("Assegna uno sprite del background prima di creare il sistema!");
            return;
        }

        // Pulisci i figli esistenti
        ClearChildren();

        // Aggiungi ParallaxBackground se non esiste
        ParallaxBackground parallaxBg = GetComponent<ParallaxBackground>();
        if (parallaxBg == null)
        {
            parallaxBg = gameObject.AddComponent<ParallaxBackground>();
        }

        // Calcola la larghezza del background
        float backgroundWidth = backgroundSprite.bounds.size.x * backgroundScale.x;

        // Crea le copie del background
        for (int i = 0; i < numberOfCopies; i++)
        {
            GameObject bgCopy = CreateBackgroundCopy(i, backgroundWidth);
            if (bgCopy != null)
            {
                Debug.Log($"Creata copia background {i + 1}: {bgCopy.name}");
            }
        }

        Debug.Log($"✅ Sistema Parallax creato con successo! Larghezza background: {backgroundWidth}");
        Debug.Log("Premi Play per vedere lo scrolling in azione!");
    }

    private GameObject CreateBackgroundCopy(int index, float backgroundWidth)
    {
        // Crea GameObject
        GameObject bgObject = new GameObject($"Background_{index + 1}");
        bgObject.transform.SetParent(transform);
        bgObject.transform.localScale = backgroundScale;
        bgObject.transform.localPosition = new Vector3(backgroundWidth * index, 0, zOffset);

        // Aggiungi SpriteRenderer
        SpriteRenderer spriteRenderer = bgObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = backgroundSprite;
        spriteRenderer.sortingOrder = 0;
        
        if (spriteMaterial != null)
        {
            spriteRenderer.material = spriteMaterial;
        }

        // Aggiungi InfiniteScrolling
        InfiniteScrolling scrolling = bgObject.AddComponent<InfiniteScrolling>();
        
        // Configura tramite reflection per impostare i valori private
        var scrollSpeedField = typeof(InfiniteScrolling).GetField("scrollSpeed", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        scrollSpeedField?.SetValue(scrolling, scrollSpeed);

        var bgWidthField = typeof(InfiniteScrolling).GetField("backgroundWidth", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        bgWidthField?.SetValue(scrolling, backgroundWidth);

        var autoScrollField = typeof(InfiniteScrolling).GetField("autoScroll", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        autoScrollField?.SetValue(scrolling, autoScroll);

        return bgObject;
    }

    private void ClearChildren()
    {
        // Rimuovi tutti i figli esistenti
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    [ContextMenu("Calculate Background Width")]
    public void CalculateBackgroundWidth()
    {
        if (backgroundSprite == null)
        {
            Debug.LogError("Assegna uno sprite del background!");
            return;
        }

        float width = backgroundSprite.bounds.size.x * backgroundScale.x;
        float height = backgroundSprite.bounds.size.y * backgroundScale.y;
        
        Debug.Log($"📐 Dimensioni Background:");
        Debug.Log($"   Larghezza: {width} unità");
        Debug.Log($"   Altezza: {height} unità");
        Debug.Log($"   Sprite originale: {backgroundSprite.texture.width}x{backgroundSprite.texture.height} pixels");
        Debug.Log($"   Pixels Per Unit: {backgroundSprite.pixelsPerUnit}");
        Debug.Log($"   Scala applicata: {backgroundScale}");
    }

    [ContextMenu("Test Scrolling")]
    public void TestScrolling()
    {
        ParallaxBackground parallaxBg = GetComponent<ParallaxBackground>();
        if (parallaxBg != null)
        {
            parallaxBg.SetGlobalSpeed(scrollSpeed);
            Debug.Log($"✅ Scrolling configurato a velocità: {scrollSpeed}");
        }
        else
        {
            Debug.LogWarning("⚠️ Nessun componente ParallaxBackground trovato!");
        }
    }

    private void OnValidate()
    {
        // Assicurati che i valori siano ragionevoli
        numberOfCopies = Mathf.Max(2, numberOfCopies);
        scrollSpeed = Mathf.Max(0, scrollSpeed);
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ParallaxSetupHelper))]
public class ParallaxSetupHelperEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ParallaxSetupHelper helper = (ParallaxSetupHelper)target;

        UnityEditor.EditorGUILayout.Space(10);
        UnityEditor.EditorGUILayout.LabelField("Quick Actions", UnityEditor.EditorStyles.boldLabel);

        if (GUILayout.Button("🎬 Create Parallax System", GUILayout.Height(30)))
        {
            helper.CreateParallaxSystem();
        }

        if (GUILayout.Button("📐 Calculate Background Width"))
        {
            helper.CalculateBackgroundWidth();
        }

        if (GUILayout.Button("▶️ Test Scrolling"))
        {
            helper.TestScrolling();
        }

        UnityEditor.EditorGUILayout.Space(10);
        UnityEditor.EditorGUILayout.HelpBox(
            "1. Assegna uno sprite del background\n" +
            "2. Configura le impostazioni\n" +
            "3. Clicca 'Create Parallax System'\n" +
            "4. Premi Play per testare!",
            UnityEditor.MessageType.Info
        );
    }
}
#endif


using UnityEngine;

/// <summary>
/// Helper per creare rapidamente un player di base nella scena.
/// Menu: GameObject → 2D Object → Create Player Setup
/// </summary>
public class PlayerSetupHelper : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Sprite del player (opzionale - se vuoto usa un square)")]
    [SerializeField] private Sprite playerSprite;
    
    [Tooltip("Colore del player se non usi uno sprite")]
    [SerializeField] private Color playerColor = Color.cyan;
    
    [Tooltip("Scala del player")]
    [SerializeField] private Vector3 playerScale = new Vector3(0.5f, 1f, 1f);
    
    [Tooltip("Posizione iniziale del player")]
    [SerializeField] private Vector3 playerPosition = Vector3.zero;

    [Header("Physics Settings")]
    [Tooltip("Gravità del player")]
    [SerializeField] private float gravityScale = 3f;
    
    [Tooltip("Velocità di movimento")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Tooltip("Forza di salto")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Settings")]
    [Tooltip("Crea automaticamente un terreno di test")]
    [SerializeField] private bool createGround = true;
    
    [Tooltip("Posizione del terreno")]
    [SerializeField] private Vector3 groundPosition = new Vector3(0, -4, 0);
    
    [Tooltip("Scala del terreno")]
    [SerializeField] private Vector3 groundScale = new Vector3(20, 1, 1);
    
    [Tooltip("Colore del terreno")]
    [SerializeField] private Color groundColor = new Color(0.4f, 0.3f, 0.2f);

    [ContextMenu("Create Player Setup")]
    public void CreatePlayerSetup()
    {
        // Crea il player
        GameObject player = CreatePlayer();
        
        // Crea il terreno se richiesto
        if (createGround)
        {
            CreateGround();
        }

        // Seleziona il player nell'editor
#if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = player;
#endif

        Debug.Log("✅ Player creato con successo! Premi Play per testarlo.");
        Debug.Log("📝 Usa WASD o Frecce per muoverti, Spazio per saltare.");
    }

    private GameObject CreatePlayer()
    {
        // Controlla se esiste già un player
        GameObject existingPlayer = GameObject.Find("Player");
        if (existingPlayer != null)
        {
            Debug.LogWarning("⚠️ Esiste già un GameObject chiamato 'Player'. Lo sostituisco...");
            DestroyImmediate(existingPlayer);
        }

        // Crea il GameObject player
        GameObject player = new GameObject("Player");
        player.transform.position = playerPosition;
        player.transform.localScale = playerScale;

        // Aggiungi Sprite Renderer
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        if (playerSprite != null)
        {
            sr.sprite = playerSprite;
        }
        else
        {
            // Usa lo sprite square di default di Unity
            sr.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd");
            sr.color = playerColor;
        }
        sr.sortingOrder = 100; // Sopra il background

        // Aggiungi Rigidbody 2D
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Aggiungi Box Collider 2D
        BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.8f, 0.9f); // Leggermente più piccolo per migliori collisioni

        // Crea Ground Check
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = new Vector3(0, -0.5f, 0);

        // Aggiungi SimplePlayerController se esiste
        SimplePlayerController controller = player.AddComponent<SimplePlayerController>();
        // Usa reflection per impostare i campi private
        SetPrivateField(controller, "moveSpeed", moveSpeed);
        SetPrivateField(controller, "jumpForce", jumpForce);
        SetPrivateField(controller, "groundCheck", groundCheck.transform);
        SetPrivateField(controller, "groundCheckRadius", 0.2f);
        SetPrivateField(controller, "groundLayer", LayerMask.GetMask("Default"));
        SetPrivateField(controller, "spriteRenderer", sr);

        Debug.Log($"✅ Player creato a posizione {playerPosition}");
        return player;
    }

    private void CreateGround()
    {
        // Controlla se esiste già un ground
        GameObject existingGround = GameObject.Find("Ground");
        if (existingGround != null)
        {
            Debug.LogWarning("⚠️ Esiste già un GameObject chiamato 'Ground'. Lo sostituisco...");
            DestroyImmediate(existingGround);
        }

        // Crea il GameObject ground
        GameObject ground = new GameObject("Ground");
        ground.transform.position = groundPosition;
        ground.transform.localScale = groundScale;

        // Aggiungi Sprite Renderer
        SpriteRenderer sr = ground.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd");
        sr.color = groundColor;
        sr.sortingOrder = 50; // Sopra il background ma sotto il player

        // Aggiungi Box Collider 2D
        BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();

        Debug.Log($"✅ Terreno creato a posizione {groundPosition}");
    }

    private void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            field.SetValue(obj, value);
        }
    }

    [ContextMenu("Delete Player and Ground")]
    public void DeletePlayerAndGround()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            DestroyImmediate(player);
            Debug.Log("🗑️ Player rimosso");
        }

        GameObject ground = GameObject.Find("Ground");
        if (ground != null)
        {
            DestroyImmediate(ground);
            Debug.Log("🗑️ Ground rimosso");
        }
    }
}

#if UNITY_EDITOR
/// <summary>
/// Aggiunge una voce di menu per creare rapidamente il player setup
/// </summary>
public static class PlayerSetupMenu
{
    [UnityEditor.MenuItem("GameObject/2D Object/Create Player Setup", false, 10)]
    public static void CreatePlayerSetup()
    {
        // Crea un GameObject temporaneo con il helper
        GameObject temp = new GameObject("_PlayerSetupHelper");
        PlayerSetupHelper helper = temp.AddComponent<PlayerSetupHelper>();
        helper.CreatePlayerSetup();
        Object.DestroyImmediate(temp);
    }

    [UnityEditor.MenuItem("Tools/GenJam/Create Complete 2D Setup")]
    public static void CreateComplete2DSetup()
    {
        // Crea player
        GameObject temp = new GameObject("_PlayerSetupHelper");
        PlayerSetupHelper helper = temp.AddComponent<PlayerSetupHelper>();
        helper.CreatePlayerSetup();
        Object.DestroyImmediate(temp);

        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log("✅ Setup 2D Completo Creato!");
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log("📋 Nella scena ora hai:");
        Debug.Log("   • Player con controlli (WASD + Spazio)");
        Debug.Log("   • Terreno per testare il movimento");
        Debug.Log("   • Background scrolling infinito");
        Debug.Log("");
        Debug.Log("🎮 Premi PLAY per testare!");
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    }
}

[UnityEditor.CustomEditor(typeof(PlayerSetupHelper))]
public class PlayerSetupHelperEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerSetupHelper helper = (PlayerSetupHelper)target;

        UnityEditor.EditorGUILayout.Space(10);
        UnityEditor.EditorGUILayout.LabelField("Quick Actions", UnityEditor.EditorStyles.boldLabel);

        if (GUILayout.Button("🎮 Create Player Setup", GUILayout.Height(30)))
        {
            helper.CreatePlayerSetup();
        }

        UnityEditor.EditorGUILayout.Space(5);

        if (GUILayout.Button("🗑️ Delete Player and Ground"))
        {
            if (UnityEditor.EditorUtility.DisplayDialog(
                "Conferma Eliminazione", 
                "Sei sicuro di voler eliminare Player e Ground?", 
                "Sì", "No"))
            {
                helper.DeletePlayerAndGround();
            }
        }

        UnityEditor.EditorGUILayout.Space(10);
        UnityEditor.EditorGUILayout.HelpBox(
            "1. (Opzionale) Trascina uno sprite nel campo 'Player Sprite'\n" +
            "2. Configura le impostazioni\n" +
            "3. Clicca 'Create Player Setup'\n" +
            "4. Premi Play per testare!\n\n" +
            "Controlli: WASD + Spazio per saltare",
            UnityEditor.MessageType.Info
        );
    }
}
#endif


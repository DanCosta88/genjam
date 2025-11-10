using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

/// <summary>
/// Script helper per setup automatico completo del player nella scena.
/// Menu: Tools → GenJam → Auto Setup Player in Scene
/// </summary>
public class AutoPlayerSetup : MonoBehaviour
{
    [Header("Player Sprites")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite walkSprite;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite attackSprite;

    [Header("Player Settings")]
    [SerializeField] private Vector3 playerPosition = new Vector3(-5, 0, 0);
    [SerializeField] private Vector3 playerScale = Vector3.one;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Settings")]
    [SerializeField] private bool createGround = true;
    [SerializeField] private Vector3 groundPosition = new Vector3(0, -4, 0);
    [SerializeField] private Vector3 groundScale = new Vector3(30, 1, 1);

#if UNITY_EDITOR
    [ContextMenu("Auto Setup Complete Player")]
    public void AutoSetupPlayer()
    {
        // Carica automaticamente gli sprite dalla cartella Player
        LoadPlayerSprites();

        // Crea le animation clips se non esistono
        CreateAnimationClips();

        // Crea il player
        GameObject player = CreateCompletePlayer();

        // Crea il terreno
        if (createGround)
        {
            CreateGround();
        }

        // Seleziona il player
        Selection.activeGameObject = player;

        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log("✅ Player Setup Completato!");
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log("📋 Configurazione:");
        Debug.Log($"   • Position: {playerPosition}");
        Debug.Log($"   • Move Speed: {moveSpeed}");
        Debug.Log($"   • Jump Force: {jumpForce}");
        Debug.Log("");
        Debug.Log("🎮 Controlli:");
        Debug.Log("   • ← → : Movimento");
        Debug.Log("   • Spazio: Salto");
        Debug.Log("   • F: Attacco");
        Debug.Log("");
        Debug.Log("🎯 Premi PLAY per testare!");
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    }

    private void CreateAnimationClips()
    {
        // Crea la cartella Animations se non esiste
        if (!AssetDatabase.IsValidFolder("Assets/Animations"))
        {
            AssetDatabase.CreateFolder("Assets", "Animations");
        }

        // Crea le animation clips usando l'API di Unity
        CreateAnimationClip("Player_Idle", idleSprite, 10f, true);
        CreateAnimationClip("Player_Walk", walkSprite, 10f, true);
        CreateAnimationClip("Player_Jump", jumpSprite, 10f, false);
        CreateAnimationClip("Player_Attack", attackSprite, 10f, false);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void CreateAnimationClip(string clipName, Sprite sprite, float frameRate, bool loop)
    {
        string path = $"Assets/Animations/{clipName}.anim";
        
        // Se esiste già, non ricrearla
        if (AssetDatabase.LoadAssetAtPath<AnimationClip>(path) != null)
        {
            Debug.Log($"Animation clip {clipName} già esistente, skip.");
            return;
        }

        if (sprite == null)
        {
            Debug.LogWarning($"Sprite per {clipName} non trovato, skip.");
            return;
        }

        // Crea nuova animation clip
        AnimationClip clip = new AnimationClip();
        clip.frameRate = frameRate;

        // Configura il loop
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = loop;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        // Crea il keyframe per lo sprite
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] spriteKeyframes = new ObjectReferenceKeyframe[1];
        spriteKeyframes[0] = new ObjectReferenceKeyframe();
        spriteKeyframes[0].time = 0f;
        spriteKeyframes[0].value = sprite;

        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeyframes);

        // Salva l'animation clip
        AssetDatabase.CreateAsset(clip, path);
        Debug.Log($"✅ Creata animation clip: {clipName}");
    }

    private void LoadPlayerSprites()
    {
        // Carica automaticamente gli sprite se non sono già assegnati
        if (idleSprite == null)
        {
            idleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Player/idle.png");
        }
        if (walkSprite == null)
        {
            walkSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Player/walk.png");
        }
        if (jumpSprite == null)
        {
            jumpSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Player/jump.png");
        }
        if (attackSprite == null)
        {
            attackSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Images/Player/attack02.png");
        }
    }

    private GameObject CreateCompletePlayer()
    {
        // Rimuovi player esistente se presente
        GameObject existingPlayer = GameObject.Find("Player");
        if (existingPlayer != null)
        {
            DestroyImmediate(existingPlayer);
        }

        // Crea il GameObject principale
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = playerPosition;
        player.transform.localScale = playerScale;

        // Sprite Renderer
        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
        sr.sortingOrder = 100;

        // Rigidbody 2D
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Box Collider 2D
        BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.8f, 1.5f);

        // Animator
        Animator animator = player.AddComponent<Animator>();
        RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
            "Assets/Scripts/PlayerAnimator.controller");
        animator.runtimeAnimatorController = controller;

        // Player Combat Controller
        PlayerCombatController combat = player.AddComponent<PlayerCombatController>();

        // Crea GroundCheck
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = new Vector3(0, -0.8f, 0);

        // Crea AttackPoint
        GameObject attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.SetParent(player.transform);
        attackPoint.transform.localPosition = new Vector3(1f, 0, 0);

        // Configura i riferimenti usando reflection
        SetPrivateField(combat, "moveSpeed", moveSpeed);
        SetPrivateField(combat, "jumpForce", jumpForce);
        SetPrivateField(combat, "groundCheck", groundCheck.transform);
        SetPrivateField(combat, "groundCheckRadius", 0.2f);
        SetPrivateField(combat, "groundLayer", LayerMask.GetMask("Default"));
        SetPrivateField(combat, "attackPoint", attackPoint.transform);
        SetPrivateField(combat, "attackRange", 1.5f);
        SetPrivateField(combat, "attackDamage", 10);
        SetPrivateField(combat, "enemyLayer", LayerMask.GetMask("Default"));
        SetPrivateField(combat, "attackDuration", 0.5f);
        SetPrivateField(combat, "spriteRenderer", sr);

        EditorUtility.SetDirty(player);
        return player;
    }

    private void CreateGround()
    {
        // Rimuovi ground esistente se presente
        GameObject existingGround = GameObject.Find("Ground");
        if (existingGround != null)
        {
            DestroyImmediate(existingGround);
        }

        // Crea GameObject
        GameObject ground = new GameObject("Ground");
        ground.transform.position = groundPosition;
        ground.transform.localScale = groundScale;

        // Sprite Renderer con sprite built-in
        SpriteRenderer sr = ground.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        sr.color = new Color(0.3f, 0.25f, 0.2f, 1f);
        sr.sortingOrder = 50;

        // Box Collider 2D
        BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();

        EditorUtility.SetDirty(ground);
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
#endif
}

#if UNITY_EDITOR
public static class AutoPlayerSetupMenu
{
    [MenuItem("Tools/GenJam/Auto Setup Player in Scene", false, 1)]
    public static void AutoSetupPlayerInScene()
    {
        // Crea un GameObject temporaneo con lo script
        GameObject temp = new GameObject("_AutoPlayerSetup");
        AutoPlayerSetup setup = temp.AddComponent<AutoPlayerSetup>();

        // Esegui il setup
        setup.AutoSetupPlayer();

        // Rimuovi il GameObject temporaneo
        Object.DestroyImmediate(temp);
    }

    [MenuItem("Tools/GenJam/Open ScrollingScene", false, 2)]
    public static void OpenScrollingScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
            "Assets/Scenes/ScrollingScene.unity");
        
        Debug.Log("📂 Scena ScrollingScene aperta!");
        Debug.Log("💡 Usa: Tools → GenJam → Auto Setup Player in Scene");
    }

    [MenuItem("Tools/GenJam/Complete Setup (Scene + Player)", false, 3)]
    public static void CompleteSetup()
    {
        // Apri la scena
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
            "Assets/Scenes/ScrollingScene.unity");

        // Setup player
        AutoSetupPlayerInScene();

        Debug.Log("🎉 Setup Completo! Premi Play per testare!");
    }
}
#endif


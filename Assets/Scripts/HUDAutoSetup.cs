using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Script helper per creare automaticamente l'HUD in stile Super Mario
/// Menu: Tools → GenJam → Create Mario-Style HUD
/// </summary>
public class HUDAutoSetup : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/GenJam/Create Mario-Style HUD")]
    public static void CreateMarioHUD()
    {
        // Controlla se esiste già
        Canvas existingCanvas = FindObjectOfType<Canvas>();
        if (existingCanvas != null && existingCanvas.name == "HUD_Canvas")
        {
            if (!EditorUtility.DisplayDialog("HUD Exists", 
                "Un HUD_Canvas esiste già. Vuoi sostituirlo?", 
                "Sì", "No"))
            {
                return;
            }
            DestroyImmediate(existingCanvas.gameObject);
        }

        // 1. Crea Canvas
        GameObject canvasGO = new GameObject("HUD_Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        
        canvasGO.AddComponent<GraphicRaycaster>();

        // Crea EventSystem se non esiste
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // 2. Crea Top Panel (full width, in alto)
        GameObject topPanel = new GameObject("Top_Panel");
        topPanel.transform.SetParent(canvas.transform, false);
        
        RectTransform topRect = topPanel.AddComponent<RectTransform>();
        topRect.anchorMin = new Vector2(0, 1);
        topRect.anchorMax = new Vector2(1, 1);
        topRect.pivot = new Vector2(0.5f, 1);
        topRect.anchoredPosition = Vector2.zero;
        topRect.sizeDelta = new Vector2(0, 100);
        
        Image panelImage = topPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.6f);

        // 3. Left Group (Player Name + Score)
        GameObject leftGroup = CreateUIGroup(topPanel.transform, "Left_Group",
            new Vector2(0, 1), new Vector2(50, -40), new Vector2(300, 60));
        
        CreateText(leftGroup.transform, "PlayerName_Text", "MARIO",
            24, Color.white, TextAlignmentOptions.Left, new Vector2(0, 15));
        
        CreateText(leftGroup.transform, "Score_Text", "000000",
            28, new Color(1f, 0.84f, 0f), TextAlignmentOptions.Left, new Vector2(0, -15), true);

        // 4. Center Group (World + Coins + Lives)
        GameObject centerGroup = CreateUIGroup(topPanel.transform, "Center_Group",
            new Vector2(0.5f, 1), new Vector2(0, -40), new Vector2(400, 60));
        
        CreateText(centerGroup.transform, "World_Text", "WORLD 1-1",
            20, Color.white, TextAlignmentOptions.Center, new Vector2(0, 20));

        // Coins Group
        GameObject coinsGroup = CreateHorizontalGroup(centerGroup.transform, "Coins_Group",
            new Vector2(-60, -10));
        CreateText(coinsGroup.transform, "CoinIcon_Text", "🪙",
            24, Color.yellow, TextAlignmentOptions.Center, Vector2.zero);
        CreateText(coinsGroup.transform, "Coins_Text", "×00",
            24, Color.white, TextAlignmentOptions.Left, Vector2.zero);

        // Lives Group
        GameObject livesGroup = CreateHorizontalGroup(centerGroup.transform, "Lives_Group",
            new Vector2(60, -10));
        CreateText(livesGroup.transform, "LifeIcon_Text", "❤",
            24, Color.red, TextAlignmentOptions.Center, Vector2.zero);
        CreateText(livesGroup.transform, "Lives_Text", "×3",
            24, Color.white, TextAlignmentOptions.Left, Vector2.zero);

        // 5. Right Group (Timer)
        GameObject rightGroup = CreateUIGroup(topPanel.transform, "Right_Group",
            new Vector2(1, 1), new Vector2(-50, -40), new Vector2(200, 60));
        
        CreateText(rightGroup.transform, "Time_Label", "TIME",
            20, Color.white, TextAlignmentOptions.Right, new Vector2(0, 15));
        
        CreateText(rightGroup.transform, "Time_Text", "400",
            32, Color.white, TextAlignmentOptions.Right, new Vector2(0, -15), true);

        // 6. Aggiungi HUDManager e collega riferimenti
        HUDManager hudManager = canvasGO.AddComponent<HUDManager>();
        
        // Usa reflection per assegnare i campi private
        SetPrivateField(hudManager, "playerNameText", 
            FindTextInChildren(canvasGO.transform, "PlayerName_Text"));
        SetPrivateField(hudManager, "scoreText", 
            FindTextInChildren(canvasGO.transform, "Score_Text"));
        SetPrivateField(hudManager, "coinsText", 
            FindTextInChildren(canvasGO.transform, "Coins_Text"));
        SetPrivateField(hudManager, "worldText", 
            FindTextInChildren(canvasGO.transform, "World_Text"));
        SetPrivateField(hudManager, "timeText", 
            FindTextInChildren(canvasGO.transform, "Time_Text"));
        SetPrivateField(hudManager, "livesText", 
            FindTextInChildren(canvasGO.transform, "Lives_Text"));

        // 7. Crea GameManager se non esiste
        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject gmGO = new GameObject("GameManager");
            gmGO.AddComponent<GameManager>();
        }

        EditorUtility.SetDirty(canvasGO);
        Selection.activeGameObject = canvasGO;

        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log("✅ HUD in Stile Super Mario Creato!");
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        Debug.Log("📋 Elementi creati:");
        Debug.Log("   • Canvas con CanvasScaler");
        Debug.Log("   • Top Panel con background nero");
        Debug.Log("   • Player Name + Score (sinistra)");
        Debug.Log("   • World + Coins + Lives (centro)");
        Debug.Log("   • Timer (destra)");
        Debug.Log("   • HUDManager configurato");
        Debug.Log("   • GameManager creato");
        Debug.Log("");
        Debug.Log("🎮 Premi Play per testare!");
        Debug.Log("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
    }

    private static GameObject CreatePanel(Transform parent, string name, 
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
        Vector2 offsetMin, Vector2 offsetMax, Vector2 sizeDelta)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent);
        
        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
        rect.sizeDelta = sizeDelta;
        rect.localScale = Vector3.one;
        
        panel.AddComponent<Image>();
        
        return panel;
    }

    private static GameObject CreateUIGroup(Transform parent, string name,
        Vector2 anchor, Vector2 position, Vector2 size)
    {
        GameObject group = new GameObject(name);
        group.transform.SetParent(parent);
        
        RectTransform rect = group.AddComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        rect.localScale = Vector3.one;
        
        return group;
    }

    private static GameObject CreateHorizontalGroup(Transform parent, string name, Vector2 position)
    {
        GameObject group = CreateUIGroup(parent, name, new Vector2(0.5f, 0.5f), position, new Vector2(100, 40));
        
        HorizontalLayoutGroup layout = group.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 5;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        
        return group;
    }

    private static TextMeshProUGUI CreateText(Transform parent, string name, string text,
        int fontSize, Color color, TextAlignmentOptions alignment, Vector2 position, bool bold = false)
    {
        GameObject textGO = new GameObject(name);
        textGO.transform.SetParent(parent);
        
        RectTransform rect = textGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(200, 50);
        rect.localScale = Vector3.one;
        
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = alignment;
        if (bold) tmp.fontStyle = FontStyles.Bold;
        
        return tmp;
    }

    private static TextMeshProUGUI FindTextInChildren(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
            {
                return child.GetComponent<TextMeshProUGUI>();
            }
        }
        return null;
    }

    private static void SetPrivateField(object obj, string fieldName, object value)
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


# 🎮 Come Aggiungere un Player alla Scena

## ❓ Perché non vedo il Player?

La scena **ScrollingScene.unity** contiene solo:
- ✅ Background scrolling funzionante
- ✅ Camera
- ✅ Illuminazione

**Non include un Player** perché ogni gioco ha esigenze diverse (sprite personalizzato, dimensioni, controlli, ecc.).

---

## 🚀 Metodo Rapido: Player Placeholder (2 minuti)

### Step 1: Crea il GameObject Player

1. **Nella Hierarchy**, click destro → **2D Object** → **Sprites** → **Square**
2. Rinominalo in **"Player"**
3. Posizionalo a: **X: 0, Y: 0, Z: 0**
4. Scalalo a: **X: 0.5, Y: 1, Z: 1** (per farlo sembrare un personaggio)

### Step 2: Cambia il Colore

1. Seleziona il Player
2. Nel **Sprite Renderer**, cambia **Color** in un colore che preferisci (es: rosso, blu, verde)

### Step 3: Aggiungi Fisica (Opzionale ma consigliato)

1. Seleziona il Player
2. **Add Component** → **Rigidbody 2D**
3. Nel Rigidbody 2D:
   - **Gravity Scale**: 3
   - **Constraints** → **Freeze Rotation Z**: ✓ (checked)
4. **Add Component** → **Box Collider 2D** (si auto-adatta)

### Step 4: Aggiungi Controllo (Opzionale)

1. Seleziona il Player
2. **Add Component** → cerca **"Simple Player Controller"**
3. Configura:
   - **Move Speed**: 5
   - **Jump Force**: 10
   - **Ground Layer**: Default (o crea un layer "Ground")

### Step 5: Crea un Terreno (per testare salto)

1. **Hierarchy** → Click destro → **2D Object** → **Sprites** → **Square**
2. Rinominalo **"Ground"**
3. Posizionalo: **X: 0, Y: -4, Z: 0**
4. Scalalo: **X: 20, Y: 1, Z: 1**
5. Cambia colore (es: marrone)
6. **Add Component** → **Box Collider 2D**

✅ **Premi Play** e usa WASD + Spazio per muoverti e saltare!

---

## 🎨 Metodo Avanzato: Player con Sprite Personalizzato

### Step 1: Importa il tuo Sprite

1. Trascina il tuo sprite del player in **Assets/Sprites/**
2. Seleziona lo sprite
3. Nell'Inspector:
   - **Texture Type**: Sprite (2D and UI)
   - **Pixels Per Unit**: 100 (o come preferisci)
   - **Filter Mode**: Point (per pixel art)
4. Clicca **Apply**

### Step 2: Crea il Player con il tuo Sprite

1. Trascina lo sprite dalla Project View alla Hierarchy
2. Rinominalo **"Player"**
3. Posizionalo dove preferisci (es: X: -5, Y: 0, Z: 0)

### Step 3: Aggiungi Componenti

Come nel metodo rapido:
- Rigidbody 2D
- Box Collider 2D (regola la size per fittare il personaggio)
- Simple Player Controller (opzionale)

### Step 4: Crea un Ground Check

1. Seleziona il Player nella Hierarchy
2. Click destro sul Player → **Create Empty**
3. Rinominalo **"GroundCheck"**
4. Posizionalo ai piedi del player (es: Y: -0.5 rispetto al player)

### Step 5: Configura il Controller

Se usi **SimplePlayerController**:
1. Seleziona il Player
2. Nel componente SimplePlayerController:
   - **Ground Check**: trascina il GameObject "GroundCheck"
   - **Ground Check Radius**: 0.2
   - **Ground Layer**: seleziona i layer che consideri "terreno"
   - **Sprite Renderer**: trascina il Sprite Renderer del player

---

## 🎯 Configurazione Camera Follow (Opzionale)

Per far seguire la camera al player:

### Opzione A: Parent Semplice
1. Nella Hierarchy, trascina **Main Camera** sul **Player**
2. Imposta Camera Position: **X: 0, Y: 2, Z: -10**
3. ✅ La camera seguirà il player!

### Opzione B: Smooth Follow (Avanzato)
Crea uno script `CameraFollow.cs`:

```csharp
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 2, -10);

    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
```

Poi:
1. Seleziona **Main Camera**
2. **Add Component** → **Camera Follow**
3. Trascina il **Player** nel campo **Target**

---

## 🎮 Controlli di Default

Se usi **SimplePlayerController**:

| Azione | Tasto |
|--------|-------|
| Muovi Sinistra | A o ← |
| Muovi Destra | D o → |
| Salta | Spazio |

---

## 🛠️ Setup Completo Esempio

Ecco una configurazione funzionante completa:

```
Hierarchy:
├── Main Camera
│   └── CameraFollow (opzionale)
├── Global Light 2D
├── ParallaxManager
│   ├── Background_1
│   └── Background_2
├── Ground (terreno)
│   └── Box Collider 2D
└── Player
    ├── Sprite Renderer
    ├── Rigidbody 2D
    ├── Box Collider 2D
    ├── SimplePlayerController
    └── GroundCheck (Empty child)
```

**Componenti Player:**
- Transform: (0, 0, 0)
- Sprite Renderer: tuo sprite o square
- Rigidbody 2D:
  - Mass: 1
  - Gravity Scale: 3
  - Freeze Rotation Z: ✓
- Box Collider 2D: auto-size
- Simple Player Controller:
  - Move Speed: 5
  - Jump Force: 10
  - Ground Check: GroundCheck transform
  - Ground Layer: Ground

---

## 🐛 Troubleshooting

### Il player non si muove
- ✅ Verifica che SimplePlayerController sia **enabled**
- ✅ Controlla che Move Speed > 0
- ✅ Assicurati di essere in Play Mode

### Il player cade all'infinito
- ✅ Crea un terreno con Box Collider 2D
- ✅ Verifica che il Rigidbody 2D abbia Gravity Scale > 0

### Il player non salta
- ✅ Verifica che Jump Force > 0
- ✅ Assicurati che Ground Check sia configurato
- ✅ Il Ground Layer deve corrispondere al layer del terreno

### Il player rotea quando cammina
- ✅ Nel Rigidbody 2D, attiva **Freeze Rotation Z**

---

## 💡 Tips Utili

1. **Layers**: Crea layer separati per Player, Ground, Enemy per gestire meglio le collisioni
2. **Sorting Layers**: Il player dovrebbe avere Sorting Order più alto del background (es: 100)
3. **Scale**: Mantieni scale uniforme (es: 1, 1, 1) e regola la dimensione dello sprite nell'import
4. **Collider**: Usa un Box Collider 2D leggermente più piccolo dello sprite per collisioni migliori

---

## 🎉 Fatto!

Ora dovresti vedere il player nella scena e poterlo controllare!

**Prossimi passi:**
1. Personalizza lo sprite del player
2. Aggiungi animazioni (walk, jump, idle)
3. Aggiungi effetti particellari (dust, jump particles)
4. Integra con il sistema di input

---

**Nota**: Ricorda che il background continua a scorrere indipendentemente dal player. Se vuoi sincronizzarli, consulta la documentazione del ParallaxBackground!


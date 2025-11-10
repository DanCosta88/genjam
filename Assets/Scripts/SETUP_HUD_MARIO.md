# 🎮 Setup HUD in Stile Super Mario

Guida completa per creare un'interfaccia HUD simile a Super Mario.

## 📦 File Creati

```
Scripts/
├── GameManager.cs      ← Gestisce score, monete, vite, timer
├── HUDManager.cs       ← Aggiorna l'interfaccia UI
└── Collectible.cs      ← Script per monete e power-up
```

---

## 🎨 Layout HUD (Stile Super Mario)

```
┌─────────────────────────────────────────────────┐
│ MARIO              WORLD 1-1           TIME     │
│ 000000             🪙 ×00               400     │
│                    ❤️ ×3                        │
└─────────────────────────────────────────────────┘
```

**Elementi:**
- **Top Left**: Nome player + Score
- **Top Center**: Nome mondo + Monete + Vite
- **Top Right**: Timer

---

## 🚀 Setup Rapido (10 minuti)

### Step 1: Crea il Canvas

1. **Hierarchy** → Click destro → **UI** → **Canvas**
2. Rinomina in "**HUD_Canvas**"
3. Nel Canvas, imposta:
   - **Render Mode**: Screen Space - Overlay
   - **Canvas Scaler**:
     - UI Scale Mode: **Scale With Screen Size**
     - Reference Resolution: **1920 × 1080**
     - Match: **0.5** (medio tra width e height)

### Step 2: Crea il GameManager

1. **Hierarchy** → Create Empty → "**GameManager**"
2. **Add Component** → **GameManager** (lo script che ho creato)
3. Configura:
   - Score: 0
   - Coins: 0
   - Lives: 3
   - Time Remaining: 400
   - World Name: "WORLD 1-1"
   - Player Name: "MARIO"

### Step 3: Crea la Struttura UI

Nel Canvas "HUD_Canvas", crea questa struttura:

```
HUD_Canvas
├── Top_Panel (Panel - tutta la larghezza in alto)
│   ├── Left_Group
│   │   ├── PlayerName_Text (MARIO)
│   │   └── Score_Text (000000)
│   │
│   ├── Center_Group
│   │   ├── World_Text (WORLD 1-1)
│   │   ├── Coins_Group
│   │   │   ├── Coin_Icon (🪙)
│   │   │   └── Coins_Text (×00)
│   │   └── Lives_Group
│   │       ├── Life_Icon (❤️)
│   │       └── Lives_Text (×3)
│   │
│   └── Right_Group
│       ├── Time_Label (TIME)
│       └── Time_Text (400)
```

---

## 📝 Setup Dettagliato

### A) Top Panel

1. **Click destro su HUD_Canvas** → **UI** → **Panel**
2. Rinomina "**Top_Panel**"
3. **Rect Transform**:
   - Anchor Preset: **Top Stretch** (top row, center)
   - Height: **80**
   - Left: 0, Right: 0, Top: 0
4. **Image** Component:
   - Color: Nero semi-trasparente (0, 0, 0, 150)

### B) Left Group (Player Name + Score)

1. **Click destro su Top_Panel** → **Create Empty**
2. Rinomina "**Left_Group**"
3. **Rect Transform**:
   - Anchor: Top Left
   - Pos X: 50, Pos Y: -40
   - Width: 300, Height: 60

4. **Crea PlayerName_Text**:
   - **UI** → **Text - TextMeshPro** (prima volta: Import TMP Essentials)
   - Parent: Left_Group
   - Text: "MARIO"
   - Font Size: 24
   - Color: Bianco
   - Alignment: Left, Middle
   - Pos: (0, 15, 0)

5. **Crea Score_Text**:
   - **UI** → **Text - TextMeshPro**
   - Parent: Left_Group
   - Text: "000000"
   - Font Size: 28
   - Color: Giallo (#FFD700)
   - Alignment: Left, Middle
   - Pos: (0, -15, 0)
   - Font Style: Bold

### C) Center Group (World + Coins + Lives)

1. **Click destro su Top_Panel** → **Create Empty**
2. Rinomina "**Center_Group**"
3. **Rect Transform**:
   - Anchor: Top Center
   - Pos Y: -40
   - Width: 400, Height: 60

4. **World_Text**:
   - **UI** → **Text - TextMeshPro**
   - Text: "WORLD 1-1"
   - Font Size: 20
   - Color: Bianco
   - Alignment: Center, Top
   - Pos: (0, 20, 0)

5. **Coins_Group**:
   - Create Empty child
   - **Add Component** → **Horizontal Layout Group**
     - Child Alignment: Middle Center
     - Spacing: 10
   - Pos: (-50, -10, 0)

   a) **Coin_Icon** (child di Coins_Group):
      - **UI** → **Image**
      - Color: Giallo dorato
      - Width/Height: 30
      - Puoi usare un'emoji "🪙" in un Text o un'immagine

   b) **Coins_Text** (child di Coins_Group):
      - **Text - TextMeshPro**
      - Text: "×00"
      - Font Size: 24
      - Color: Bianco

6. **Lives_Group** (come Coins_Group):
   - Create Empty
   - Horizontal Layout Group
   - Pos: (50, -10, 0)
   - Life_Icon + Lives_Text

### D) Right Group (Timer)

1. **Click destro su Top_Panel** → **Create Empty**
2. Rinomina "**Right_Group**"
3. **Rect Transform**:
   - Anchor: Top Right
   - Pos X: -50, Pos Y: -40
   - Width: 200, Height: 60

4. **Time_Label**:
   - **Text - TextMeshPro**
   - Text: "TIME"
   - Font Size: 20
   - Color: Bianco
   - Alignment: Right, Top
   - Pos: (0, 15, 0)

5. **Time_Text**:
   - **Text - TextMeshPro**
   - Text: "400"
   - Font Size: 32
   - Color: Bianco
   - Font Style: Bold
   - Alignment: Right, Middle
   - Pos: (0, -15, 0)

---

## 🔗 Collega HUD Manager

1. **Seleziona HUD_Canvas**
2. **Add Component** → **HUD Manager**
3. **Assegna i riferimenti** trascinando gli oggetti:
   - Player Name Text → PlayerName_Text
   - Score Text → Score_Text
   - Coins Text → Coins_Text
   - World Text → World_Text
   - Time Text → Time_Text
   - Lives Text → Lives_Text

---

## 🎨 Stile Super Mario - Configurazione Font

### Font Consigliato
Usa un font pixelato per lo stile retro:
- **Press Start 2P** (free su Google Fonts)
- **Pixel Operator**
- O usa il font di default di TMP

### Colori Mario
```
Giallo Score: #FFD700
Bianco Testo: #FFFFFF
Nero Background: #000000 (alpha 150)
Rosso Warning: #FF0000
```

---

## 🪙 Creare una Moneta Collezionabile

1. **GameObject** → **2D Object** → **Sprite** → **Circle**
2. Rinomina "**Coin**"
3. **Transform**:
   - Scale: (0.5, 0.5, 1)
4. **Sprite Renderer**:
   - Color: Giallo dorato (#FFD700)
   - Sorting Order: 50
5. **Add Component** → **Circle Collider 2D**
   - Is Trigger: ✓ (checked!)
6. **Add Component** → **Collectible** (lo script)
   - Type: Coin
   - Score Value: 100
7. **Tag**: Assicurati che il Player abbia tag "Player"

**Duplica** per creare più monete!

---

## ⚡ Quick Test

### Test HUD:
1. **Seleziona GameManager** nella Hierarchy
2. In Play Mode, modifica i valori nell'Inspector:
   - Score → Si aggiorna nell'HUD
   - Coins → Si aggiorna nell'HUD
   - Lives → Si aggiorna nell'HUD
   - Time → Si aggiorna nell'HUD

### Test Monete:
1. Crea alcune monete nel livello
2. Premi Play
3. Muovi il player sulle monete
4. Score e monete si aggiornano automaticamente!

---

## 🎯 API per Scripting

### Da altri script, puoi usare:

```csharp
// Aggiungi punti
GameManager.Instance.AddScore(500);

// Aggiungi monete
GameManager.Instance.AddCoin(1);

// Perdi vita
GameManager.Instance.LoseLife();

// Aggiungi vita
GameManager.Instance.AddLife();

// Modifica tempo
GameManager.Instance.AddTime(50f);
GameManager.Instance.SetTime(200f);
```

---

## 🎨 Personalizzazione

### Cambia il Nome del Mondo

```csharp
GameManager.Instance.SetWorldName("FOREST 2-3");
```

### Cambia il Nome del Player

```csharp
GameManager.Instance.SetPlayerName("LUIGI");
```

### Timer Infinito

Nel GameManager:
- Countdown Timer: ✗ (unchecked)

---

## 📚 Features Bonus

### 1. Vita Extra ogni 100 Monete
- Automatico! Quando arrivi a 100 monete:
  - Ottieni 1 vita extra
  - Le monete tornano a 0
  - Continua a contare

### 2. Timer che Lampeggia
- Quando il tempo scende sotto 30 secondi
- Il numero lampeggia rosso/bianco
- Crea urgenza!

### 3. Game Over Automatico
- Quando le vite arrivano a 0
- Il gioco si mette in pausa
- Puoi aggiungere schermata game over

---

## 🐛 Troubleshooting

### I testi non si vedono
- ✅ Verifica che il Canvas sia in Screen Space Overlay
- ✅ Controlla che i text siano figli del Canvas
- ✅ Verifica che il colore sia visibile (non nero su nero)

### I valori non si aggiornano
- ✅ Verifica che HUDManager abbia tutti i riferimenti assegnati
- ✅ Controlla che GameManager esista nella scena
- ✅ Assicurati che i GameObject siano attivi

### Le monete non vengono raccolte
- ✅ Circle Collider 2D deve avere "Is Trigger" ✓
- ✅ Il Player deve avere tag "Player"
- ✅ Il Player deve avere un Collider 2D

---

## 💡 Estensioni Possibili

### Power-Up Mushroom
```csharp
public void CollectMushroom()
{
    // Ingrandisci player
    player.transform.localScale *= 1.5f;
    GameManager.Instance.AddScore(1000);
}
```

### Checkpoint
```csharp
public void SaveCheckpoint(Vector3 position)
{
    PlayerPrefs.SetFloat("CheckpointX", position.x);
    PlayerPrefs.SetFloat("CheckpointY", position.y);
}
```

### High Score
```csharp
public void SaveHighScore()
{
    int currentHigh = PlayerPrefs.GetInt("HighScore", 0);
    if (score > currentHigh)
    {
        PlayerPrefs.SetInt("HighScore", score);
    }
}
```

---

## 🎉 Esempio Completo

Quando tutto è configurato:

```
MARIO              WORLD 1-1            TIME
000000             🪙 ×05               395
                   ❤️ ×3
```

**Giocando:**
- Raccogli monete → Score aumenta, monete +1
- Tempo scorre → Countdown attivo
- 100 monete → +1 vita
- Tempo = 0 → Perdi vita
- Vite = 0 → Game Over

---

**🎮 Setup completato! Crea l'HUD seguendo gli step e avrai un'interfaccia perfetta in stile Super Mario!** ✨

*Nota: Per un setup ancora più veloce, posso creare uno script helper che genera tutto automaticamente!*


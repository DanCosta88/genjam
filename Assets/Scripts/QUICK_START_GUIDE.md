# 🚀 Guida Rapida - Scrolling Background

## ⚡ Setup in 3 Minuti

### Metodo 1: Usa la Scena Pronta ✨ (RACCOMANDATO)

1. **Apri la scena pronta**
   - Vai in `Assets/Scenes/ScrollingScene.unity`
   - Premi Play ▶️
   - Il background scorrerà automaticamente!

2. **Personalizza la velocità**
   - Seleziona `ParallaxManager` nella Hierarchy
   - Nel componente `ParallaxBackground`, modifica:
     - `Base Scroll Speed`: velocità dello scrolling (default: 3)
     - `Speed Multiplier`: moltiplicatore temporaneo

3. **Sostituisci il background**
   - Seleziona `Background_1` e `Background_2` nella Hierarchy
   - Nel `Sprite Renderer`, trascina il tuo sprite nel campo `Sprite`

**✅ FATTO! Hai uno scrolling infinito funzionante!**

---

### Metodo 2: Setup Automatico con Helper 🛠️

1. **Crea un nuovo GameObject**
   - Hierarchy → Click destro → Create Empty
   - Rinominalo "ParallaxManager"

2. **Aggiungi il Helper**
   - Seleziona il GameObject
   - Add Component → `Parallax Setup Helper`

3. **Configura il Background**
   - Trascina il tuo sprite nel campo `Background Sprite`
   - Imposta `Number Of Copies` a 2
   - Imposta `Background Scale` (es: 2, 2, 1)
   - Imposta `Scroll Speed` (es: 3)

4. **Crea il Sistema**
   - Clicca il pulsante **"🎬 Create Parallax System"**
   - Il sistema verrà creato automaticamente!

5. **Premi Play** ▶️
   - Il background scorrerà in loop infinito!

---

### Metodo 3: Setup Manuale (Avanzato) 🔧

<details>
<summary>Clicca per espandere</summary>

1. **Prepara il Background**
   ```
   - Importa l'immagine in Assets/Images/
   - Texture Type: Sprite (2D and UI)
   - Pixels Per Unit: 100
   - Filter Mode: Point (per pixel art)
   ```

2. **Crea la Struttura**
   ```
   ParallaxManager (Empty GameObject)
   ├── Background_1
   │   ├── Sprite Renderer (con il tuo sprite)
   │   └── InfiniteScrolling (script)
   └── Background_2
       ├── Sprite Renderer (con il tuo sprite)
       └── InfiniteScrolling (script)
   ```

3. **Posiziona i Background**
   ```
   Background_1:
   - Position: (0, 0, 10)
   - Scale: (2, 2, 1)
   
   Background_2:
   - Position: (20.48, 0, 10)  // Larghezza del background
   - Scale: (2, 2, 1)
   ```

4. **Configura InfiniteScrolling**
   Su entrambi i background:
   ```
   - Scroll Speed: 3
   - Background Width: 20.48 (auto-calcolato)
   - Auto Scroll: ✓ checked
   - Parallax Factor: 1
   ```

5. **Aggiungi ParallaxBackground**
   Sul ParallaxManager:
   ```
   - Add Component: ParallaxBackground
   - Layers Size: 1
   - Element 0:
     - Layer Object: Background_1
     - Parallax Speed: 1
     - Z Offset: 10
   - Base Scroll Speed: 3
   ```

</details>

---

## 🎮 Testare il Sistema

### 1. Avvia la Scena
Premi il pulsante **Play** in Unity Editor

### 2. Verifica il Comportamento
- ✅ Il background dovrebbe scorrere da destra a sinistra
- ✅ Il movimento dovrebbe essere fluido e continuo
- ✅ Non dovrebbero esserci gap o salti visibili

### 3. Regola la Velocità
Durante il Play Mode:
- Seleziona `ParallaxManager`
- Cambia `Base Scroll Speed` in tempo reale
- Vedi l'effetto immediatamente!

---

## 🔧 Parametri Comuni

### Velocità dello Scrolling

| Tipo di Gioco | Scroll Speed Consigliato |
|---------------|--------------------------|
| Slow Walking | 1 - 2 |
| Normal Platformer | 3 - 5 |
| Fast Runner | 6 - 10 |
| Speed Runner | 10+ |

### Dimensioni Background

| Risoluzione Sprite | Pixels Per Unit | Scale | Larghezza Unity |
|-------------------|-----------------|-------|-----------------|
| 1024 × 1024 | 100 | 2 | 20.48 |
| 2048 × 1024 | 100 | 1 | 20.48 |
| 512 × 512 | 50 | 2 | 20.48 |

**Formula**: `Larghezza = (Pixel Width / PPU) × Scale.x`

---

## 🎨 Aggiungere Effetto Parallax

### Per Creare Profondità:

1. **Duplica Background_1 e Background_2**
2. **Rinomina** in "FarBackground_1" e "FarBackground_2"
3. **Cambia lo sprite** con uno più lontano (es: cielo, montagne)
4. **Imposta Z Offset** più alto (es: 20)
5. **Nel ParallaxManager**, aggiungi un nuovo layer:
   ```
   - Layer Object: FarBackground_1
   - Parallax Speed: 0.3  ← Più lento!
   - Z Offset: 20
   ```

**Risultato**: Il background lontano si muove più lentamente, creando profondità!

---

## 🐛 Troubleshooting Veloce

### ❌ Il background non si muove
**Fix**: 
- Seleziona Background_1 e Background_2
- Verifica che `InfiniteScrolling` sia **enabled**
- Verifica che `Auto Scroll` sia **checked**
- Verifica che `Scroll Speed` sia **> 0**

### ❌ Vedo un gap tra i background
**Fix**:
- Seleziona Background_2
- Posizionalo esattamente a `X = Background Width`
- Usa "Calculate Background Width" nel helper per trovare il valore esatto

### ❌ Lo scrolling è troppo veloce/lento
**Fix**:
- Seleziona ParallaxManager
- Modifica `Base Scroll Speed`
- O modifica `Speed Multiplier` per cambi temporanei

### ❌ I background hanno scale diversi
**Fix**:
- Seleziona Background_1 e Background_2
- Premi F2 e digita lo stesso valore di Scale per entrambi
- Assicurati che siano identici: (2, 2, 1)

---

## 📝 Prossimi Passi

Dopo aver impostato lo scrolling di base:

1. **Aggiungi un Player**
   - Usa `SimplePlayerController.cs` come riferimento
   - Posizionalo nella scena
   - Configura il movimento

2. **Aggiungi Layer Parallax**
   - Crea background multipli
   - Assegna velocità diverse
   - Crea profondità visiva

3. **Personalizza la Camera**
   - Aggiungi camera follow
   - Configura i limiti
   - Aggiungi camera shake

4. **Aggiungi Effetti**
   - Particelle (foglie, neve)
   - Luci dinamiche (2D Light)
   - Post-processing

---

## 💡 Tips Utili

### Performance
- ✅ Usa texture compresse per build
- ✅ Mantieni 2-5 layer di parallax max
- ✅ Usa Sprite Atlases per batch rendering

### Visual Quality
- ✅ Filter Mode: **Point** per pixel art
- ✅ Filter Mode: **Bilinear** per art smooth
- ✅ Usa **HDR colors** per effetti luminosi

### Seamless Loop
- ✅ Il bordo destro deve matchare il bordo sinistro
- ✅ Usa tools come Photoshop per creare tile seamless
- ✅ Testa sempre in Play Mode

---

## 📚 Risorse Aggiuntive

- **README completo**: `Assets/Scripts/README_ScrollingSystem.md`
- **Script API**: Apri gli script e leggi i commenti
- **Scene di esempio**: `Assets/Scenes/ScrollingScene.unity`

---

## 🎉 Complimenti!

Hai completato il setup del sistema di scrolling infinito!

**Ora puoi**:
- ✅ Creare endless runners
- ✅ Platformer side-scrolling
- ✅ Giochi con parallax avanzato
- ✅ Background dinamici reattivi

**Buon game dev!** 🚀


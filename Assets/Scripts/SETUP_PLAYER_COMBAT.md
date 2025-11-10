# 🎮 Setup Player con Sistema di Combattimento

## ✅ Cosa Ho Creato

Ho preparato un sistema completo di player con:
- ✅ **Controller di movimento** (frecce direzionali)
- ✅ **Sistema di salto** (Spazio)
- ✅ **Sistema di attacco** (tasto F)
- ✅ **Animazioni**: Idle, Walk, Jump, Attack
- ✅ **Flip automatico** dello sprite quando cambia direzione
- ✅ **Rilevamento nemici** e sistema di danno

---

## 📁 File Creati

```
Assets/
├── Scripts/
│   ├── PlayerCombatController.cs      ← Controller completo del player
│   └── PlayerAnimator.controller      ← Animator con tutte le animazioni
│
└── Prefabs/
    └── Player.prefab                  ← Prefab del player pronto all'uso
```

---

## 🚀 Setup Rapido (5 minuti)

### Step 1: Configura gli Sprite

1. **Seleziona tutti gli sprite del player** in `Assets/Images/Player/`:
   - `idle.png`
   - `walk.png`
   - `jump.png`
   - `attack02.png`

2. **Per ogni sprite**, nell'Inspector imposta:
   - **Texture Type**: Sprite (2D and UI)
   - **Sprite Mode**: Multiple (se hanno più frame) o Single
   - **Pixels Per Unit**: 100
   - **Filter Mode**: Point (per pixel art)
   - **Compression**: None
   - Clicca **Apply**

3. **Apri lo Sprite Editor** (pulsante "Sprite Editor"):
   - Se sono sprite sheets, taglia i frame
   - Altrimenti lascia così

### Step 2: Crea le Animation Clips

1. **Nella Hierarchy**, crea un GameObject temporaneo:
   - GameObject → Create Empty → Rinomina "PlayerTemp"

2. **Aggiungi Animator Component**:
   - Add Component → Animator

3. **Crea le Animation Clips**:

#### A) Animazione IDLE
1. Window → Animation → Animation
2. Clicca "Create" → Salva come `Player_Idle.anim`
3. Clicca "Add Property" → Sprite Renderer → Sprite
4. Trascina lo sprite `idle.png` nel timeline a 0:00
5. Se hai più frame idle, trascinali in sequenza

#### B) Animazione WALK
1. Clicca "Create New Clip" → Salva come `Player_Walk.anim`
2. Aggiungi Sprite Renderer → Sprite
3. Trascina i frame di walk in sequenza
4. Imposta loop: ✓

#### C) Animazione JUMP
1. Clicca "Create New Clip" → Salva come `Player_Jump.anim`
2. Aggiungi sprite di jump
3. Loop: ✗ (non in loop)

#### D) Animazione ATTACK
1. Clicca "Create New Clip" → Salva come `Player_Attack.anim`
2. Aggiungi sprite di attack
3. Loop: ✗

### Step 3: Configura l'Animator Controller

1. **Apri** `Assets/Scripts/PlayerAnimator.controller`

2. **Assegna le Animation Clips**:
   - Stato **Idle** → Motion: `Player_Idle`
   - Stato **Walk** → Motion: `Player_Walk`
   - Stato **Jump** → Motion: `Player_Jump`
   - Stato **Attack** → Motion: `Player_Attack`

3. **Le transizioni sono già configurate!** ✅

### Step 4: Crea il Player GameObject

1. **Nella Hierarchy**:
   - GameObject → 2D Object → Sprite
   - Rinomina: "Player"
   - Position: (0, 0, 0)

2. **Aggiungi i componenti**:

#### A) Sprite Renderer
- Sprite: `idle.png` (primo frame)
- Sorting Order: 100

#### B) Rigidbody 2D
- Gravity Scale: 3
- Freeze Rotation: Z ✓

#### C) Box Collider 2D
- Size: (0.8, 1.5) ← Regola in base allo sprite

#### D) Animator
- Controller: Trascina `PlayerAnimator.controller`

#### E) Player Combat Controller
- Add Component → Player Combat Controller
- Configura i parametri (vedi sotto)

### Step 5: Configura il PlayerCombatController

Seleziona il Player e nell'Inspector configura:

```
Movement Settings:
- Move Speed: 5
- Jump Force: 12
- Run Multiplier: 1.5

Ground Check:
- Ground Check: (crea un child GameObject "GroundCheck" a Y: -0.8)
- Ground Check Radius: 0.2
- Ground Layer: Default (o "Ground")

Combat Settings:
- Attack Damage: 10
- Attack Range: 1.5
- Attack Point: (crea un child GameObject "AttackPoint" a X: 1)
- Enemy Layer: Default (o "Enemy")
- Attack Duration: 0.5

Visual Settings:
- Sprite Renderer: Trascina il Sprite Renderer del player
```

### Step 6: Crea i GameObject Figli

1. **GroundCheck**:
   - Click destro su Player → Create Empty
   - Rinomina: "GroundCheck"
   - Position: (0, -0.8, 0)

2. **AttackPoint**:
   - Click destro su Player → Create Empty
   - Rinomina: "AttackPoint"
   - Position: (1, 0, 0)

### Step 7: Crea il Terreno

1. **GameObject** → 2D Object → Sprite → Square
2. Rinomina: "Ground"
3. Position: (0, -4, 0)
4. Scale: (30, 1, 1)
5. Color: Marrone/grigio
6. **Add Component** → Box Collider 2D
7. Layer: "Ground" (crea il layer se non esiste)

### Step 8: Testa il Player!

1. **Salva la scena**
2. **Premi Play** ▶️
3. **Controlli**:
   - **← →** : Muoversi
   - **Spazio**: Saltare
   - **F**: Attaccare

---

## 🎯 Controlli

| Azione | Tasto |
|--------|-------|
| Muovi Sinistra | Freccia ← |
| Muovi Destra | Freccia → |
| Salto | Spazio |
| Attacco | F |

---

## 🔧 Parametri Dettagliati

### Move Speed
- **1-3**: Lento (personaggio pesante)
- **4-6**: Normale (platformer standard)
- **7-10**: Veloce (action game)

### Jump Force
- **8-10**: Salto basso
- **11-15**: Salto normale
- **16-20**: Salto alto

### Attack Damage
- Danno inflitto ai nemici
- Default: 10

### Attack Range
- Raggio dell'attacco in unità Unity
- Default: 1.5 (copre circa 1.5 unità davanti al player)

### Ground Check Radius
- Raggio del cerchio per rilevare il terreno
- Default: 0.2
- Più grande = più tollerante

---

## 🎨 Animazioni

### Stati dell'Animator

```
┌─────────┐
│  Idle   │  ← Stato iniziale
└────┬────┘
     │
     ├─→ Walk (IsMoving = true)
     ├─→ Jump (IsGrounded = false)
     └─→ Attack (Attack trigger)
```

### Parametri Animator

| Nome | Tipo | Descrizione |
|------|------|-------------|
| IsMoving | Bool | True quando si muove |
| IsGrounded | Bool | True quando è a terra |
| VerticalVelocity | Float | Velocità verticale (per salto/caduta) |
| Attack | Trigger | Attiva animazione di attacco |

---

## 🐛 Troubleshooting

### Il player non si muove
- ✅ Verifica che Rigidbody 2D sia presente
- ✅ Controlla che Move Speed > 0
- ✅ Assicurati che il component sia enabled

### Il player non salta
- ✅ Verifica che Jump Force > 0
- ✅ Controlla che Ground Check sia configurato
- ✅ Il player deve essere a terra (isGrounded = true)

### Le animazioni non cambiano
- ✅ Verifica che l'Animator Controller sia assegnato
- ✅ Controlla che le Animation Clips siano assegnate agli stati
- ✅ Assicurati che i parametri siano configurati correttamente

### Il player non si flippi
- ✅ Lo script usa `transform.localScale` per il flip
- ✅ Verifica che la direzione sia quella corretta all'inizio

### L'attacco non colpisce i nemici
- ✅ Verifica che Attack Point sia configurato
- ✅ Controlla che Enemy Layer sia corretto
- ✅ I nemici devono avere un collider sul layer Enemy

---

## 🎮 Sistema di Combattimento

### Come Funziona

1. **Premi F** per attaccare
2. Il player entra nello stato "Attacking" per 0.5 secondi
3. Viene rilevato un cerchio di raggio `attackRange` da `attackPoint`
4. Tutti i nemici nel cerchio prendono danno
5. Se il nemico ha un component `EnemyHealth`, viene chiamato `TakeDamage()`

### Creare un Nemico

```csharp
1. Crea GameObject 2D → Sprite
2. Aggiungi Sprite Renderer
3. Aggiungi Collider 2D
4. Imposta Layer: "Enemy"
5. Add Component → EnemyHealth (già incluso nello script)
6. Configura Max Health
```

---

## 💡 Estensioni Possibili

### Combo System
Aggiungi un contatore di combo e diversi attacchi

### Special Moves
Aggiungi abilità speciali (es: dash, attacco caricato)

### Health Bar
Crea UI per mostrare la vita del player

### Stamina System
Limita attacchi e dash con una barra stamina

### Effetti Particellari
Aggiungi particle effects a salto, attacco, ecc.

### Sound Effects
Integra suoni per movimento, salto, attacco

---

## 📝 Esempio di Enemy Setup

```
Enemy GameObject:
├── Sprite Renderer (sprite del nemico)
├── Box Collider 2D
├── Rigidbody 2D (opzionale)
└── EnemyHealth (script già incluso)
    └── Max Health: 100
```

---

## 🎉 Fatto!

Ora hai un player completo con:
- ✅ Movimento fluido
- ✅ Salto reattivo
- ✅ Sistema di combattimento
- ✅ Animazioni smooth
- ✅ Flip automatico

**Premi Play e divertiti!** 🎮

---

## 📚 Note Tecniche

### Performance
- Il sistema usa `Animator.StringToHash()` per i parametri (più veloce)
- Il flip usa `localScale` invece di rotazione (più efficiente)
- Ground check usa `Physics2D.OverlapCircle` (ottimizzato)

### Compatibilità
- ✅ Unity 6.2
- ✅ URP 2D
- ✅ Input System (legacy e nuovo)

---

*Sistema creato per Unity 6.2 con Universal Render Pipeline 2D*


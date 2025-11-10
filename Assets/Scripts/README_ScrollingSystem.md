# Sistema di Scrolling Infinito 2D

Sistema completo per creare background a scorrimento infinito per giochi 2D side-scrolling in Unity.

## 📁 Componenti

### 1. **InfiniteScrolling.cs**
Script principale che gestisce lo scrolling infinito di un singolo background.

**Funzionalità:**
- Scrolling automatico configurabile
- Supporto per input del giocatore
- Reset automatico della posizione per loop seamless
- Calcolo automatico della larghezza del background
- Fattore di parallax personalizzabile

**Parametri:**
- `scrollSpeed`: Velocità di scrolling (unità/secondo)
- `backgroundWidth`: Larghezza del background (calcolata automaticamente dallo sprite)
- `autoScroll`: Abilita scrolling automatico
- `usePlayerInput`: Usa l'input del giocatore per controllare la velocità
- `parallaxFactor`: Fattore di parallax (0-1)

### 2. **ParallaxBackground.cs**
Manager per gestire multiple layer di parallax contemporaneamente.

**Funzionalità:**
- Gestione di layer multipli
- Velocità differenziate per ogni layer
- Controllo globale della velocità
- Pausa/riprendi scrolling

**Parametri:**
- `layers`: Array di layer con GameObject, velocità parallax e z-offset
- `baseScrollSpeed`: Velocità base globale
- `speedMultiplier`: Moltiplicatore per tutte le velocità

## 🎮 Setup nella Scena

### Metodo 1: Scena Pronta (ScrollingScene.unity)
La scena `ScrollingScene.unity` è già configurata e pronta all'uso:

1. Apri la scena `Assets/Scenes/ScrollingScene.unity`
2. Premi Play per vedere lo scrolling in azione
3. Il background si ripete automaticamente in loop

**Struttura della scena:**
```
ScrollingScene
├── Main Camera (con Universal Additional Camera Data)
├── Global Light 2D
└── ParallaxManager
    ├── Background_1 (primo tile del background)
    └── Background_2 (secondo tile per il loop seamless)
```

### Metodo 2: Setup Manuale

#### Step 1: Preparare il Background
1. Importa l'immagine del background in `Assets/Images/`
2. Configura le impostazioni dell'immagine:
   - **Texture Type:** Sprite (2D and UI)
   - **Sprite Mode:** Single
   - **Pixels Per Unit:** 100 (o come preferisci)
   - **Filter Mode:** Point (per pixel art) o Bilinear
   - **Compression:** None (per qualità massima)

#### Step 2: Creare i GameObject
1. Crea un GameObject vuoto chiamato "ParallaxManager"
2. Aggiungi lo script `ParallaxBackground.cs`
3. Crea un GameObject figlio chiamato "Background_1"
4. Aggiungi un `Sprite Renderer` e assegna il tuo sprite
5. Aggiungi lo script `InfiniteScrolling.cs`
6. Duplica "Background_1" e chiamalo "Background_2"
7. Posiziona "Background_2" esattamente alla larghezza del background (es. X = 20.48 se il background è largo 20.48 unità)

#### Step 3: Configurare i Layer
Nel `ParallaxBackground`:
1. Imposta la size dell'array `layers` a 1 (o più se hai più layer)
2. Assegna "Background_1" al primo layer
3. Imposta `parallaxSpeed` a 1.0 per movimento completo
4. Imposta `zOffset` a 10 (per mettere il background dietro)

#### Step 4: Regolare la Camera
1. Assicurati che la camera sia **Orthographic**
2. Imposta un `Orthographic Size` appropriato (es. 5)
3. Posiziona la camera a Z = -10
4. Abilita il post-processing se desiderato

## 🎨 Creare Effetti Parallax Multipli

Per creare profondità con più layer:

```
ParallaxManager
├── FarBackground (parallaxSpeed: 0.3, zOffset: 20)
├── MidBackground (parallaxSpeed: 0.6, zOffset: 10)
└── NearBackground (parallaxSpeed: 1.0, zOffset: 5)
```

Ogni layer si muoverà a velocità diversa, creando l'illusione di profondità.

## 🎯 Integrare con il Player

### Esempio 1: Scrolling Automatico (Runner)
Il background scorre automaticamente, il player resta al centro:

```csharp
// In InfiniteScrolling:
autoScroll = true;
usePlayerInput = false;
scrollSpeed = 3f;
```

### Esempio 2: Scrolling Controllato dal Player
Il background si muove quando il player si muove:

```csharp
// In InfiniteScrolling:
autoScroll = false;
usePlayerInput = true;
```

### Esempio 3: Camera che Segue il Player
Usa un sistema di camera follow separato e imposta:

```csharp
// In InfiniteScrolling:
autoScroll = true;
parallaxFactor = 0.5; // I layer lontani si muovono più lentamente
```

## 📐 Calcolare la Larghezza del Background

Lo script calcola automaticamente la larghezza, ma puoi impostarla manualmente:

```
Larghezza in Unity = (Larghezza Pixel / Pixels Per Unit) × Scale.x
```

Esempio:
- Immagine: 1024 pixel
- Pixels Per Unit: 100
- Scale: 2
- **Larghezza = (1024 / 100) × 2 = 20.48 unità**

## 🔧 API e Controllo Runtime

### InfiniteScrolling

```csharp
// Cambia velocità
infiniteScrolling.SetScrollSpeed(5f);

// Attiva/disattiva auto scroll
infiniteScrolling.SetAutoScroll(true);

// Imposta parallax factor
infiniteScrolling.SetParallaxFactor(0.7f);
```

### ParallaxBackground

```csharp
// Cambia velocità globale
parallaxBackground.SetGlobalSpeed(4f);

// Imposta moltiplicatore (es. slow-motion)
parallaxBackground.SetSpeedMultiplier(0.5f);

// Pausa/riprendi
parallaxBackground.PauseScrolling(true);
```

## 🎨 Tips e Best Practices

### 1. Background Seamless
Assicurati che il bordo destro dell'immagine si colleghi perfettamente con il bordo sinistro per un loop seamless.

### 2. Sorting Layers
Usa i Sorting Layers per controllare cosa appare sopra/sotto:
- Background: -100
- Midground: 0
- Player: 100
- Foreground: 200

### 3. Performance
- Usa texture compresse per build finali
- Evita troppi layer di parallax (3-5 è ottimale)
- Usa atlanti di sprite se possibile

### 4. Illuminazione 2D
Il sistema funziona perfettamente con URP 2D Lighting:
- Aggiungi Global Light 2D per illuminazione base
- Usa Point Lights per effetti dinamici
- Configura Normal Maps per profondità

## 🐛 Troubleshooting

### Il background non scorre
- ✅ Verifica che `autoScroll` sia abilitato
- ✅ Controlla che `scrollSpeed` sia > 0
- ✅ Assicurati che lo script sia enabled

### Gap visibili tra i tile
- ✅ Verifica che Background_2 sia posizionato esattamente a `backgroundWidth`
- ✅ Controlla che entrambi i background abbiano lo stesso scale
- ✅ Usa Filter Mode: Point per pixel art

### Lo scrolling è troppo veloce/lento
- ✅ Regola `baseScrollSpeed` nel ParallaxManager
- ✅ Modifica `parallaxFactor` per i singoli layer
- ✅ Usa `speedMultiplier` per effetti temporanei

### I layer si muovono alla stessa velocità
- ✅ Verifica che ogni layer abbia un `parallaxSpeed` diverso
- ✅ Controlla che `parallaxFactor` sia impostato correttamente
- ✅ Assicurati che il ParallaxManager abbia inizializzato i layer

## 📝 Esempi di Configurazione

### Endless Runner
```
baseScrollSpeed: 5
Background_1 parallaxSpeed: 1.0
```

### Platformer con Parallax
```
baseScrollSpeed: 3
SkyLayer parallaxSpeed: 0.2
CloudLayer parallaxSpeed: 0.4
ForestLayer parallaxSpeed: 1.0
```

### Slow Motion Effect
```csharp
parallaxBackground.SetSpeedMultiplier(0.3f);
```

## 🎓 Prossimi Passi

1. Aggiungi più layer per profondità maggiore
2. Implementa un sistema di player movimento
3. Aggiungi effetti particellari (foglie, neve, ecc.)
4. Integra con il nuovo Input System
5. Crea variazioni del background per diversità


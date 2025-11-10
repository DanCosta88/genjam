# 🎮 GenJam - Unity 2D Side-Scrolling Game

Progetto Unity 6.2 con sistema di scrolling infinito e player combat system.

## 📋 Descrizione

Template completo per giochi 2D side-scrolling con:
- ✅ Background scrolling infinito con parallax
- ✅ Sistema di combattimento per il player
- ✅ Animazioni complete (Idle, Walk, Jump, Attack)
- ✅ Universal Render Pipeline (URP) 2D
- ✅ Sistema di illuminazione 2D

## 🚀 Quick Start

### Requisiti
- Unity 6.2 o superiore
- Universal Render Pipeline (URP) installato

### Primo Avvio
1. Clona il repository
2. Apri il progetto con Unity Hub
3. Apri la scena `Assets/Scenes/ScrollingScene.unity`
4. Premi Play!

## 📁 Struttura del Progetto

```
Assets/
├── Images/              # Sprite e texture
│   ├── Background.png   # Background della foresta magica
│   └── Player/          # Sprite del player (idle, walk, jump, attack)
│
├── Prefabs/             # Prefab riutilizzabili
│   └── Player.prefab    # Prefab del player
│
├── Scenes/              # Scene del gioco
│   ├── SampleScene.unity       # Scena base Unity
│   └── ScrollingScene.unity    # Scena con scrolling pronta ⭐
│
├── Scripts/             # Script C#
│   ├── InfiniteScrolling.cs           # Scrolling del background
│   ├── ParallaxBackground.cs          # Manager parallax
│   ├── PlayerCombatController.cs      # Controller player completo
│   ├── ParallaxSetupHelper.cs         # Tool setup automatico
│   ├── PlayerSetupHelper.cs           # Tool setup player
│   └── SimplePlayerController.cs      # Controller semplificato
│
├── Settings/            # Configurazioni URP
│   ├── UniversalRP.asset
│   └── Renderer2D.asset
│
└── Documentation/       # Documentazione (file .md e .txt)
```

## 🎮 Controlli

| Azione | Tasto |
|--------|-------|
| Muovi Destra | → o D |
| Muovi Sinistra | ← o A |
| Salto | Spazio |
| Attacco | F |

## 📚 Documentazione

### Guide Principali
- **`LEGGIMI_PRIMA.txt`** - Punto di partenza, nel root di Assets
- **`QUICK_START_GUIDE.md`** - Setup rapido in 3 minuti
- **`SCROLLING_SYSTEM_OVERVIEW.md`** - Overview completo del sistema di scrolling

### Guide Specifiche
- **`COME_AGGIUNGERE_PLAYER.md`** - Come aggiungere un player alla scena
- **`SETUP_PLAYER_COMBAT.md`** - Setup completo del sistema di combattimento
- **`README_ScrollingSystem.md`** - Documentazione tecnica scrolling

Tutte le guide si trovano in `Assets/Scripts/`

## ✨ Features

### Sistema di Scrolling
- Background infinito seamless
- Supporto parallax multi-layer
- Velocità configurabile a runtime
- Ottimizzato per performance

### Sistema di Combattimento
- Movimento fluido con physics
- Sistema di salto reattivo
- Attacco con rilevamento nemici
- Animazioni smooth
- Flip automatico sprite

### Rendering
- Universal Render Pipeline (URP) 2D
- Illuminazione 2D dinamica
- Post-processing configurabile
- Ottimizzato per mobile e desktop

## 🛠️ Customizzazione

### Cambiare la Velocità dello Scrolling
```
1. Seleziona ParallaxManager nella Hierarchy
2. Modifica "Base Scroll Speed" nell'Inspector
3. Valori consigliati: 1-10
```

### Aggiungere Layer Parallax
```
1. Duplica Background_1 e Background_2
2. Cambia lo sprite (es: cielo, montagne)
3. Imposta Z offset diverso
4. Aggiungi al ParallaxManager con velocità < 1
```

### Personalizzare il Player
```
1. Sostituisci gli sprite in Assets/Images/Player/
2. Ricrea le animation clips
3. Configura i parametri nel PlayerCombatController
```

## 🎨 Assets Inclusi

### Background
- **Foresta Magica** (1024x1024) - Pixel art con illuminazione atmosferica

### Player Sprites
- **Idle** - Animazione idle
- **Walk** - Animazione camminata
- **Jump** - Sprite di salto
- **Attack** - Animazione attacco

## ⚙️ Configurazione Unity

### Render Pipeline
- **URP 2D** configurato
- **HDR** abilitato
- **2D Lighting** attivo
- **SRP Batcher** ottimizzato

### Input System
- Supporto **Legacy Input System**
- Configurazione in `InputSystem_Actions.inputactions`
- Control schemes: Keyboard, Gamepad, Touch, XR

### Physics 2D
- Gravity: 9.81
- Layer collision matrix configurata
- Continuous collision detection

## 🐛 Troubleshooting

### Il background non scorre
- Verifica che Auto Scroll sia attivato
- Controlla che Scroll Speed > 0
- Assicurati di essere in Play Mode

### Il player non si muove
- Verifica che Rigidbody 2D sia presente
- Controlla che Move Speed > 0
- Assicurati che il component sia enabled

### Animazioni non funzionano
- Verifica che le Animation Clips siano assegnate
- Controlla che l'Animator Controller sia collegato
- Assicurati che gli sprite siano configurati correttamente

Per più troubleshooting, vedi le guide in `Assets/Scripts/`

## 📦 Build

### Preparazione per il Build
1. File → Build Settings
2. Seleziona la piattaforma target
3. Aggiungi le scene necessarie
4. Configure Player Settings
5. Build!

### Piattaforme Testate
- ✅ Windows
- ✅ macOS
- ✅ Linux
- ✅ WebGL (ottimizzazione richiesta)
- ⚠️ Mobile (testing necessario)

## 🔄 Aggiornamenti Futuri

Possibili estensioni:
- [ ] Sistema di inventory
- [ ] Multiple weapons
- [ ] Enemy AI avanzato
- [ ] Power-ups
- [ ] Checkpoints
- [ ] Menu system
- [ ] Audio manager
- [ ] Particle effects
- [ ] Weather system

## 🤝 Contribuire

Questo è un template di base. Sentiti libero di:
- Estendere le funzionalità
- Migliorare le performance
- Aggiungere nuovi sistemi
- Creare varianti

## 📄 Licenza

Progetto educativo/template. 
Sentiti libero di usarlo per i tuoi progetti!

## 📞 Support

Per domande o problemi:
1. Leggi la documentazione in `Assets/Scripts/`
2. Controlla la sezione Troubleshooting
3. Consulta le guide specifiche

## 🎓 Risorse Utili

- [Unity Manual](https://docs.unity3d.com/)
- [URP Documentation](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest)
- [2D Pixel Perfect](https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@latest)

---

**Versione**: 1.0  
**Unity**: 6.2  
**URP**: 17.x  
**Creato**: 2025

🎮 Buon game development! 🚀


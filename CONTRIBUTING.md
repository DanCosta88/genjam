# 🤝 Contributing to GenJam

Grazie per il tuo interesse nel contribuire a questo progetto!

## 📋 Come Contribuire

### 1. Fork del Repository
```bash
# Clona il tuo fork
git clone https://github.com/TUO-USERNAME/GenJam.git
cd GenJam
```

### 2. Crea un Branch
```bash
# Crea un branch per la tua feature
git checkout -b feature/nome-feature

# Oppure per un bugfix
git checkout -b bugfix/descrizione-bug
```

### 3. Fai le Tue Modifiche
- Segui le convenzioni di codice Unity
- Commenta il codice quando necessario
- Testa le modifiche

### 4. Commit
```bash
# Aggiungi i file modificati
git add .

# Commit con messaggio descrittivo
git commit -m "Add: descrizione della feature"
```

### 5. Push e Pull Request
```bash
# Push sul tuo fork
git push origin feature/nome-feature

# Crea una Pull Request su GitHub
```

## 📝 Convenzioni di Codice

### Naming Conventions

#### C# Scripts
```csharp
// PascalCase per classi e metodi pubblici
public class PlayerController : MonoBehaviour
{
    // camelCase per variabili private
    private float moveSpeed;
    
    // PascalCase per proprietà
    public float MoveSpeed { get; set; }
    
    // PascalCase per metodi
    public void MovePlayer()
    {
        // ...
    }
}
```

#### GameObjects e Assets
- **GameObjects**: PascalCase (es: `PlayerCharacter`, `EnemySpawner`)
- **Prefabs**: PascalCase (es: `Player.prefab`, `Enemy_Goblin.prefab`)
- **Scenes**: PascalCase (es: `MainMenu.unity`, `Level01.unity`)
- **Sprites**: snake_case (es: `player_idle.png`, `enemy_walk.png`)
- **Scripts**: PascalCase (es: `PlayerController.cs`)

### Struttura del Codice

```csharp
using UnityEngine;
using System.Collections;

/// <summary>
/// Descrizione breve della classe
/// </summary>
public class ExampleScript : MonoBehaviour
{
    #region Serialized Fields
    [Header("Movement Settings")]
    [Tooltip("Velocità di movimento del player")]
    [SerializeField] private float moveSpeed = 5f;
    #endregion

    #region Private Fields
    private Rigidbody2D rb;
    private bool isGrounded;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }
    #endregion

    #region Public Methods
    public void Jump()
    {
        // ...
    }
    #endregion

    #region Private Methods
    private void HandleInput()
    {
        // ...
    }
    #endregion
}
```

### Commenti e Documentazione

```csharp
/// <summary>
/// Muove il player nella direzione specificata
/// </summary>
/// <param name="direction">Direzione del movimento (-1 = sinistra, 1 = destra)</param>
/// <returns>True se il movimento è avvenuto con successo</returns>
public bool Move(float direction)
{
    // Implementazione
}
```

## 🎨 Assets Guidelines

### Sprite Import Settings
```
Texture Type: Sprite (2D and UI)
Sprite Mode: Single o Multiple
Pixels Per Unit: 100
Filter Mode: Point (per pixel art) o Bilinear
Compression: None per development, compression per build
Max Size: 2048 o meno
```

### Scene Organization
```
Scene Root
├── --- MANAGERS ---
├── GameManager
├── AudioManager
├── UIManager
│
├── --- ENVIRONMENT ---
├── Background
├── Foreground
├── Ground
│
├── --- GAMEPLAY ---
├── Player
├── Enemies
├── Collectibles
│
└── --- UI ---
    ├── Canvas
    └── EventSystem
```

### Prefab Guidelines
- Ogni prefab deve essere self-contained
- Usa nested prefabs quando appropriato
- Documenta le dipendenze esterne
- Testa i prefab in scene di test

## 🧪 Testing

### Prima di Committare
- [ ] Il codice compila senza errori
- [ ] Il codice compila senza warning
- [ ] Le scene si aprono correttamente
- [ ] Il gioco funziona in Play Mode
- [ ] Non ci sono errori nella console
- [ ] I prefab sono tutti collegati correttamente

### Testing Checklist
- [ ] Movimento del player
- [ ] Sistema di combattimento
- [ ] Scrolling del background
- [ ] Collisioni
- [ ] Animazioni
- [ ] UI responsiva

## 📦 Pull Request Guidelines

### Titolo della PR
```
[Feature] Descrizione breve
[Bugfix] Descrizione del bug risolto
[Docs] Aggiornamento documentazione
[Refactor] Refactoring del codice
```

### Descrizione della PR
```markdown
## Descrizione
Breve descrizione delle modifiche

## Tipo di Cambiamento
- [ ] Bugfix
- [ ] Nuova feature
- [ ] Breaking change
- [ ] Documentazione

## Testing
Come hai testato le modifiche?

## Screenshots
(se applicabile)

## Checklist
- [ ] Il codice segue le convenzioni del progetto
- [ ] Ho commentato il codice dove necessario
- [ ] Ho aggiornato la documentazione
- [ ] Le modifiche non generano nuovi warning
- [ ] Ho testato le modifiche
```

## 🐛 Reporting Bugs

### Template per Bug Report
```markdown
**Descrizione del Bug**
Descrizione chiara e concisa del bug

**Come Riprodurre**
1. Vai a '...'
2. Clicca su '...'
3. Vedi l'errore

**Comportamento Atteso**
Descrizione di cosa dovrebbe succedere

**Screenshots**
(se applicabile)

**Environment:**
- Unity Version: [es: 6.2]
- OS: [es: Windows 11]
- URP Version: [es: 17.0]
```

## 💡 Feature Request

### Template per Feature Request
```markdown
**Descrizione della Feature**
Descrizione chiara della feature desiderata

**Motivazione**
Perché questa feature sarebbe utile?

**Descrizione della Soluzione**
Come vorresti che fosse implementata?

**Alternative Considerate**
Altre soluzioni che hai considerato

**Informazioni Aggiuntive**
Altro contesto o screenshots
```

## 📚 Risorse Utili

### Unity Best Practices
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
- [URP Documentation](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest)

### Git Best Practices
- [Git Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows)
- [Conventional Commits](https://www.conventionalcommits.org/)

## 🎯 Areas of Contribution

Aree dove contributi sono particolarmente benvenuti:

### High Priority
- 🐛 Bugfix
- 📝 Documentazione
- ✅ Testing
- ⚡ Performance optimization

### Medium Priority
- 🎨 Nuove features
- 🎮 Nuovi sistemi di gameplay
- 🔊 Audio system
- 🌟 Particle effects

### Low Priority
- 🎨 Asset alternativi
- 📚 Tutorial
- 🌍 Localizzazione

## ❓ Domande?

Se hai domande:
1. Controlla la documentazione esistente
2. Cerca nelle Issues chiuse
3. Apri una nuova Issue con tag `question`

## 🙏 Grazie!

Ogni contributo è apprezzato, grande o piccolo!

---

**Happy Coding!** 🚀


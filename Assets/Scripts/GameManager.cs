using UnityEngine;

/// <summary>
/// Manager principale del gioco che gestisce score, vite, monete, ecc.
/// Pattern Singleton per accesso globale.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    [SerializeField] private int score = 0;
    [SerializeField] private int coins = 0;
    [SerializeField] private int lives = 3;
    [SerializeField] private float timeRemaining = 400f;
    
    [Header("Level Info")]
    [SerializeField] private string worldName = "WORLD 1-1";
    [SerializeField] private string playerName = "MARIO";

    [Header("Settings")]
    [SerializeField] private bool countdownTimer = true;
    [SerializeField] private int coinScoreValue = 100;
    [SerializeField] private int coinToLife = 100; // Monete per vita extra

    // Events per notificare UI
    public System.Action<int> OnScoreChanged;
    public System.Action<int> OnCoinsChanged;
    public System.Action<int> OnLivesChanged;
    public System.Action<float> OnTimeChanged;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Countdown timer
        if (countdownTimer && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0;
            
            OnTimeChanged?.Invoke(timeRemaining);

            // Time over
            if (timeRemaining <= 0)
            {
                TimeOver();
            }
        }
    }

    #region Score Management
    public void AddScore(int points)
    {
        score += points;
        OnScoreChanged?.Invoke(score);
    }

    public int GetScore() => score;
    #endregion

    #region Coin Management
    public void AddCoin(int amount = 1)
    {
        coins += amount;
        
        // Vita extra ogni 100 monete
        if (coins >= coinToLife)
        {
            coins -= coinToLife;
            AddLife();
        }

        // Aggiungi punti per le monete
        AddScore(amount * coinScoreValue);
        
        OnCoinsChanged?.Invoke(coins);
    }

    public int GetCoins() => coins;
    #endregion

    #region Life Management
    public void AddLife(int amount = 1)
    {
        lives += amount;
        OnLivesChanged?.Invoke(lives);
    }

    public void LoseLife()
    {
        lives--;
        OnLivesChanged?.Invoke(lives);

        if (lives <= 0)
        {
            GameOver();
        }
    }

    public int GetLives() => lives;
    #endregion

    #region Time Management
    public float GetTimeRemaining() => timeRemaining;

    public void AddTime(float seconds)
    {
        timeRemaining += seconds;
        OnTimeChanged?.Invoke(timeRemaining);
    }

    public void SetTime(float seconds)
    {
        timeRemaining = seconds;
        OnTimeChanged?.Invoke(timeRemaining);
    }
    #endregion

    #region Level Info
    public string GetWorldName() => worldName;
    public string GetPlayerName() => playerName;

    public void SetWorldName(string name)
    {
        worldName = name;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }
    #endregion

    #region Game State
    private void TimeOver()
    {
        Debug.Log("⏰ TIME OVER!");
        LoseLife();
        // Qui puoi ricaricare il livello o mostrare game over
    }

    private void GameOver()
    {
        Debug.Log("💀 GAME OVER!");
        // Qui puoi mostrare schermata game over
        Time.timeScale = 0; // Pausa il gioco
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        score = 0;
        coins = 0;
        timeRemaining = 400f;
        
        // Notifica UI
        OnScoreChanged?.Invoke(score);
        OnCoinsChanged?.Invoke(coins);
        OnTimeChanged?.Invoke(timeRemaining);
    }
    #endregion
}


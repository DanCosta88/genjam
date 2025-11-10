using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gestisce l'interfaccia HUD in stile Super Mario
/// </summary>
public class HUDManager : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI worldText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI livesText;

    [Header("Icons")]
    [SerializeField] private Image coinIcon;
    [SerializeField] private Image lifeIcon;

    private void Start()
    {
        // Subscribe to GameManager events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScore;
            GameManager.Instance.OnCoinsChanged += UpdateCoins;
            GameManager.Instance.OnLivesChanged += UpdateLives;
            GameManager.Instance.OnTimeChanged += UpdateTime;

            // Inizializza UI
            UpdatePlayerName();
            UpdateWorld();
            UpdateScore(GameManager.Instance.GetScore());
            UpdateCoins(GameManager.Instance.GetCoins());
            UpdateLives(GameManager.Instance.GetLives());
            UpdateTime(GameManager.Instance.GetTimeRemaining());
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe da events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScore;
            GameManager.Instance.OnCoinsChanged -= UpdateCoins;
            GameManager.Instance.OnLivesChanged -= UpdateLives;
            GameManager.Instance.OnTimeChanged -= UpdateTime;
        }
    }

    private void UpdatePlayerName()
    {
        if (playerNameText != null && GameManager.Instance != null)
        {
            playerNameText.text = GameManager.Instance.GetPlayerName();
        }
    }

    private void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString("D6"); // 6 cifre: 000000
        }
    }

    private void UpdateCoins(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = "×" + coins.ToString("D2"); // 2 cifre: ×00
        }
    }

    private void UpdateWorld()
    {
        if (worldText != null && GameManager.Instance != null)
        {
            worldText.text = GameManager.Instance.GetWorldName();
        }
    }

    private void UpdateTime(float time)
    {
        if (timeText != null)
        {
            int timeInt = Mathf.CeilToInt(time);
            timeText.text = timeInt.ToString();

            // Warning quando il tempo è basso
            if (timeInt <= 30 && timeInt > 0)
            {
                // Lampeggia il testo
                timeText.color = (timeInt % 2 == 0) ? Color.red : Color.white;
            }
            else
            {
                timeText.color = Color.white;
            }
        }
    }

    private void UpdateLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "×" + lives.ToString();
        }
    }

    // Metodi di utility pubblici
    public void ShowMessage(string message, float duration = 2f)
    {
        // Può essere usato per messaggi temporanei
        Debug.Log($"📢 {message}");
    }
}


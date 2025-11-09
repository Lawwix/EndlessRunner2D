using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    [Header("Game References")]
    public PlayerController player; // Ссылка на игрока

    [Header("Game Settings")]
    public float scoreMultiplier = 1f;

    private int currentScore = 0;
    private bool isGameRunning = true;
    private float distanceTraveled = 0f;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        // Если игрок не назначен в инспекторе, находим его автоматически
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        if (player != null)
        {
            lastPlayerPosition = player.transform.position;
        }

        UpdateScoreUI();
    }

    void Update()
    {
        if (!isGameRunning) return;

        CalculateScore();
    }

    void CalculateScore()
    {
        if (player != null && isGameRunning)
        {
            // Считаем очки только если игрок жив
            distanceTraveled += (player.transform.position.x - lastPlayerPosition.x);
            lastPlayerPosition = player.transform.position;

            int newScore = Mathf.FloorToInt(distanceTraveled * scoreMultiplier);
            if (newScore > currentScore)
            {
                currentScore = newScore;
                UpdateScoreUI();
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }

    // Вызывается когда игрок проигрывает
    public void GameOver()
    {
        if (!isGameRunning) return; // Уже завершена

        isGameRunning = false;
        Debug.Log($"Game Over! Final Score: {currentScore}");

        // Останавливаем игрока
        if (player != null)
        {
            player.StopRunning();
        }
    }

    public void AddScore(int points)
    {
        if (isGameRunning)
        {
            currentScore += points;
            UpdateScoreUI();
        }
    }
}

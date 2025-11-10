using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    private int currentScore = 0;
    private bool isGameRunning = true;
    private float distanceTraveled = 0f;
    private Vector3 lastPlayerPosition;
    private GameObject player;

    void Start()
    {
        // Находим игрока
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            lastPlayerPosition = player.transform.position;
        }

        UpdateScoreUI();
    }

    void Update()
    {
        if (!isGameRunning) return;

        // Проверяем не упал ли игрок
        if (player != null && player.transform.position.y < -5f)
        {
            GameOver();
            return;
        }

        CalculateScore();
    }

    void CalculateScore()
    {
        if (player != null && isGameRunning)
        {
            distanceTraveled += (player.transform.position.x - lastPlayerPosition.x);
            lastPlayerPosition = player.transform.position;

            int newScore = Mathf.FloorToInt(distanceTraveled);
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

    public void GameOver()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        Debug.Log($"=== GAME OVER ===");
        Debug.Log($"Final Score: {currentScore}");
        Debug.Log($"=== GAME OVER ===");

        // Останавливаем игрока
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.StopRunning();
        }

        // Останавливаем генерацию платформ
        PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
        if (platformSpawner != null)
        {
            platformSpawner.enabled = false;
        }

        // Можно добавить здесь вызов экрана Game Over позже
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

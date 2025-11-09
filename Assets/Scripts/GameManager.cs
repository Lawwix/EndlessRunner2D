using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;

    [Header("Game References")]
    public PlayerController player;

    [Header("Game Settings")]
    public float scoreMultiplier = 1f;

    private int currentScore = 0;
    private bool isGameRunning = true;
    private float distanceTraveled = 0f;
    private Vector3 lastPlayerPosition;

    void Start()
    {
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

    public void GameOver()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        Debug.Log($"Game Over! Final Score: {currentScore}");

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

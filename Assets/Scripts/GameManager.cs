using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalHighScoreText;

    [Header("Game References")]
    public PlayerController player;

    private int currentScore = 0;
    private int highScore = 0;
    private bool isGameRunning = true;
    private float distanceTraveled = 0f;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreUI();

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

                if (currentScore > highScore)
                {
                    highScore = currentScore;
                    UpdateHighScoreUI();
                }
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

    void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            highScoreText.text = $"Best: {highScore}";
        }
    }

    public void GameOver()
    {
        if (!isGameRunning) return;

        isGameRunning = false;

        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
        }

        // Используем Instance вместо FindObjectOfType
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSound();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = $"Score: {currentScore}";
        }

        if (finalHighScoreText != null)
        {
            finalHighScoreText.text = $"Best: {PlayerPrefs.GetInt("HighScore", 0)}";
        }

        if (player != null)
        {
            player.StopRunning();
        }

        PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
        if (platformSpawner != null)
        {
            platformSpawner.enabled = false;
        }
    }

    public void RestartGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }
        SceneManager.LoadScene("MainMenu");
    }
}

//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    [Header("UI References")]
//    public TextMeshProUGUI scoreText;
//    public TextMeshProUGUI highScoreText; // Новое поле для рекорда в игре
//    public GameObject gameOverPanel;
//    public TextMeshProUGUI finalScoreText;
//    public TextMeshProUGUI finalHighScoreText; // Новое поле для рекорда на панели Game Over

//    [Header("Game References")]
//    public PlayerController player;

//    private int currentScore = 0;
//    private int highScore = 0;
//    private bool isGameRunning = true;
//    private float distanceTraveled = 0f;
//    private Vector3 lastPlayerPosition;

//    void Start()
//    {
//        // Загружаем рекорд из памяти
//        highScore = PlayerPrefs.GetInt("HighScore", 0);
//        UpdateHighScoreUI();

//        if (player == null)
//        {
//            player = FindObjectOfType<PlayerController>();
//        }

//        if (player != null)
//        {
//            lastPlayerPosition = player.transform.position;
//        }

//        UpdateScoreUI();
//    }

//    void Update()
//    {
//        if (!isGameRunning) return;

//        // Проверяем не упал ли игрок
//        if (player != null && player.transform.position.y < -5f)
//        {
//            GameOver();
//            return;
//        }

//        CalculateScore();
//    }

//    void CalculateScore()
//    {
//        if (player != null && isGameRunning)
//        {
//            distanceTraveled += (player.transform.position.x - lastPlayerPosition.x);
//            lastPlayerPosition = player.transform.position;

//            int newScore = Mathf.FloorToInt(distanceTraveled);
//            if (newScore > currentScore)
//            {
//                currentScore = newScore;
//                UpdateScoreUI();

//                // Проверяем побит ли рекорд
//                if (currentScore > highScore)
//                {
//                    highScore = currentScore;
//                    UpdateHighScoreUI();
//                }
//            }
//        }
//    }

//    void UpdateScoreUI()
//    {
//        if (scoreText != null)
//        {
//            scoreText.text = $"Score: {currentScore}";
//        }
//    }

//    void UpdateHighScoreUI()
//    {
//        if (highScoreText != null)
//        {
//            highScoreText.text = $"Best: {highScore}";
//        }
//    }

//    public void GameOver()
//    {
//        if (!isGameRunning) return;

//        isGameRunning = false;

//        // Сохраняем рекорд если он побит
//        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
//        {
//            PlayerPrefs.SetInt("HighScore", currentScore);
//            PlayerPrefs.Save();
//            Debug.Log($"New High Score: {currentScore}");
//        }

//        // Воспроизводим звук Game Over
//        AudioManager audioManager = FindObjectOfType<AudioManager>();
//        if (audioManager != null)
//        {
//            audioManager.PlayGameOverSound();
//        }

//        // Показываем экран Game Over
//        if (gameOverPanel != null)
//        {
//            gameOverPanel.SetActive(true);
//        }

//        // Обновляем текст счета и рекорда на панели Game Over
//        if (finalScoreText != null)
//        {
//            finalScoreText.text = $"Score: {currentScore}";
//        }

//        if (finalHighScoreText != null)
//        {
//            finalHighScoreText.text = $"Best: {PlayerPrefs.GetInt("HighScore", 0)}";
//        }

//        // Останавливаем игрока
//        if (player != null)
//        {
//            player.StopRunning();
//        }

//        // Останавливаем генерацию платформ
//        PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
//        if (platformSpawner != null)
//        {
//            platformSpawner.enabled = false;
//        }
//    }

//    public void RestartGame()
//    {
//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void ReturnToMainMenu()
//    {
//        SceneManager.LoadScene("MainMenu");
//    }
//}
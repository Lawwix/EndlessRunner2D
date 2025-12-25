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
    public int CurrentScore
    {
        get { return currentScore; }
    }
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
        //if (player != null)
        //{
        //    player.StopRunning();
        //}
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

        // Сбрасываем сложность перед загрузкой сцены
        PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
        if (platformSpawner != null)
        {
            platformSpawner.ResetDifficulty();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void RestartGame()
    //{
    //    if (AudioManager.Instance != null)
    //    {
    //        AudioManager.Instance.PlayButtonClickSound();
    //    }
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
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
//using System.Collections;

//public class GameManager : MonoBehaviour
//{
//    [Header("UI References")]
//    public TextMeshProUGUI scoreText;
//    public TextMeshProUGUI highScoreText;
//    public GameObject gameOverPanel;
//    public TextMeshProUGUI finalScoreText;
//    public TextMeshProUGUI finalHighScoreText;

//    [Header("Lives UI")]
//    public GameObject[] lifeIcons; // Перетащите сюда 3 иконки сердечек
//    public TextMeshProUGUI livesText; // Опционально: текстовое отображение

//    [Header("Game References")]
//    public PlayerController player;

//    // Добавляем публичное свойство для доступа к счёту
//    public int CurrentScore => currentScore;

//    private int currentScore = 0;
//    private int highScore = 0;
//    private bool isGameRunning = true;
//    private float distanceTraveled = 0f;
//    private Vector3 lastPlayerPosition;

//    void Start()
//    {
//        highScore = PlayerPrefs.GetInt("HighScore", 0);
//        UpdateHighScoreUI();

//        if (player == null)
//        {
//            player = FindObjectOfType<PlayerController>();
//        }

//        // Создаем сердечки автоматически
//        CreateHeartIcons();

//        if (player != null)
//        {
//            lastPlayerPosition = player.transform.position;
//            UpdateLivesUI(player.currentLives);
//        }

//        UpdateScoreUI();
//    }

//    void CreateHeartIcons()
//    {
//        // Находим панель для жизней
//        GameObject livesPanel = GameObject.Find("LivesPanel");
//        if (livesPanel == null) return;

//        // Загружаем префаб сердечка
//        GameObject heartPrefab = Resources.Load<GameObject>("HeartIcon");
//        if (heartPrefab == null)
//        {
//            Debug.LogWarning("HeartIcon prefab not found in Resources folder!");
//            return;
//        }

//        // Создаем массив для иконок
//        lifeIcons = new GameObject[maxLives];

//        // Создаем 3 сердечка
//        for (int i = 0; i < maxLives; i++)
//        {
//            GameObject heart = Instantiate(heartPrefab, livesPanel.transform);
//            heart.name = $"Heart_{i}";
//            lifeIcons[i] = heart;
//        }
//    }

//    //void Start()
//    //{
//    //    highScore = PlayerPrefs.GetInt("HighScore", 0);
//    //    UpdateHighScoreUI();

//    //    if (player == null)
//    //    {
//    //        player = FindObjectOfType<PlayerController>();
//    //    }

//    //    if (player != null)
//    //    {
//    //        lastPlayerPosition = player.transform.position;
//    //        UpdateLivesUI(player.currentLives);
//    //    }

//    //    UpdateScoreUI();
//    //}

//    void Update()
//    {
//        if (!isGameRunning) return;

//        if (player != null && player.transform.position.y < -5f)
//        {
//            // При падении вызываем GameOver напрямую
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

//    // Новый метод для обновления UI жизней
//    public void UpdateLivesUI(int lives)
//    {
//        // Способ 1: через иконки сердечек
//        if (lifeIcons != null && lifeIcons.Length >= 3)
//        {
//            for (int i = 0; i < lifeIcons.Length; i++)
//            {
//                if (lifeIcons[i] != null)
//                {
//                    lifeIcons[i].SetActive(i < lives);
//                }
//            }
//        }

//        // Способ 2: через текст (опционально)
//        if (livesText != null)
//        {
//            livesText.text = $"Lives: {lives}";
//        }
//    }

//    public void GameOver()
//    {
//        if (!isGameRunning) return;

//        isGameRunning = false;

//        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
//        {
//            PlayerPrefs.SetInt("HighScore", currentScore);
//            PlayerPrefs.Save();
//        }

//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayGameOverSound();
//        }

//        if (gameOverPanel != null)
//        {
//            gameOverPanel.SetActive(true);
//        }

//        if (finalScoreText != null)
//        {
//            finalScoreText.text = $"Score: {currentScore}";
//        }

//        if (finalHighScoreText != null)
//        {
//            finalHighScoreText.text = $"Best: {PlayerPrefs.GetInt("HighScore", 0)}";
//        }

//        PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
//        if (platformSpawner != null)
//        {
//            platformSpawner.enabled = false;
//        }
//    }

//    public void RestartGame()
//    {
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayButtonClickSound();
//        }

//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void ReturnToMainMenu()
//    {
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayButtonClickSound();
//        }

//        SceneManager.LoadScene("MainMenu");
//    }
//}

//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class GameManager : MonoBehaviour
//{
//    [Header("UI References")]
//    public TextMeshProUGUI scoreText;
//    public TextMeshProUGUI highScoreText;
//    public GameObject gameOverPanel;
//    public TextMeshProUGUI finalScoreText;
//    public TextMeshProUGUI finalHighScoreText;

//    [Header("Lives UI")]
//    public GameObject[] lifeIcons; // Массив иконок жизней (3 иконки)
//    public TextMeshProUGUI livesText; // Или текстовое отображение "Lives: 3"

//    [Header("Game References")]
//    public PlayerController player;
//    private int currentScore = 0;
//    private int highScore = 0;
//    private bool isGameRunning = true;
//    private float distanceTraveled = 0f;
//    private Vector3 lastPlayerPosition;

//    //public class GameManager : MonoBehaviour
//    //{
//    //    [Header("UI References")]
//    //    public TextMeshProUGUI scoreText;
//    //    public TextMeshProUGUI highScoreText;
//    //    public GameObject gameOverPanel;
//    //    public TextMeshProUGUI finalScoreText;
//    //    public TextMeshProUGUI finalHighScoreText;

//    //    [Header("Game References")]
//    //    public PlayerController player;
//    //    public int CurrentScore
//    //    {
//    //        get { return currentScore; }
//    //    }

//    //    private int currentScore = 0;
//    //    private int highScore = 0;
//    //    private bool isGameRunning = true;
//    //    private float distanceTraveled = 0f;
//    //    private Vector3 lastPlayerPosition;

//    void Start()
//    {
//        highScore = PlayerPrefs.GetInt("HighScore", 0);
//        UpdateHighScoreUI();
//        UpdateLivesUI(); // Обновляем отображение жизней

//        if (player == null)
//        {
//            player = FindObjectOfType<PlayerController>();
//        }
//        if (player != null)
//        {
//            lastPlayerPosition = player.transform.position;
//            // Инициализируем жизни
//            UpdateLivesUI(player.currentLives);
//        }
//        UpdateScoreUI();
//    }

//    //void Start()
//    //{
//    //    highScore = PlayerPrefs.GetInt("HighScore", 0);
//    //    UpdateHighScoreUI();

//    //    if (player == null)
//    //    {
//    //        player = FindObjectOfType<PlayerController>();
//    //    }

//    //    if (player != null)
//    //    {
//    //        lastPlayerPosition = player.transform.position;
//    //    }

//    //    UpdateScoreUI();
//    //}

//    // Добавляем метод для обновления UI жизней
//    public void UpdateLivesUI(int lives)
//    {
//        // Способ 1: через иконки
//        if (lifeIcons != null && lifeIcons.Length >= 3)
//        {
//            for (int i = 0; i < lifeIcons.Length; i++)
//            {
//                lifeIcons[i].SetActive(i < lives);
//            }
//        }

//        // Способ 2: через текст
//        if (livesText != null)
//        {
//            livesText.text = $"Lives: {lives}";
//        }
//    }

//    void Update()
//    {
//        if (!isGameRunning) return;

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

//        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
//        {
//            PlayerPrefs.SetInt("HighScore", currentScore);
//            PlayerPrefs.Save();
//        }

//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayGameOverSound();
//        }

//        if (gameOverPanel != null)
//        {
//            gameOverPanel.SetActive(true);
//        }

//        if (finalScoreText != null)
//        {
//            finalScoreText.text = $"Score: {currentScore}";
//        }

//        if (finalHighScoreText != null)
//        {
//            finalHighScoreText.text = $"Best: {PlayerPrefs.GetInt("HighScore", 0)}";
//        }

//        PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
//        if (platformSpawner != null)
//        {
//            platformSpawner.enabled = false;
//        }
//    }
//}

//public void GameOver()
//{
//    if (!isGameRunning) return;

//    isGameRunning = false;

//    if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
//    {
//        PlayerPrefs.SetInt("HighScore", currentScore);
//        PlayerPrefs.Save();
//    }

//    // Используем Instance вместо FindObjectOfType
//    if (AudioManager.Instance != null)
//    {
//        AudioManager.Instance.PlayGameOverSound();
//    }

//    if (gameOverPanel != null)
//    {
//        gameOverPanel.SetActive(true);
//    }

//    if (finalScoreText != null)
//    {
//        finalScoreText.text = $"Score: {currentScore}";
//    }

//    if (finalHighScoreText != null)
//    {
//        finalHighScoreText.text = $"Best: {PlayerPrefs.GetInt("HighScore", 0)}";
//    }

//    //if (player != null)
//    //{
//    //    player.StopRunning();
//    //}

//    PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
//    if (platformSpawner != null)
//    {
//        platformSpawner.enabled = false;
//    }
//}

//public void RestartGame()
//    {
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayButtonClickSound();
//        }

//        // Сбрасываем сложность перед загрузкой сцены
//        PlatformSpawner platformSpawner = FindObjectOfType<PlatformSpawner>();
//        if (platformSpawner != null)
//        {
//            platformSpawner.ResetDifficulty();
//        }

//        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//    }

//    public void ReturnToMainMenu()
//    {
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayButtonClickSound();
//        }
//        SceneManager.LoadScene("MainMenu");
//    }
//}

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
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI highScoreText; // Поле для отображения рекорда

    private AudioManager audioManager;

    void Start()
    {
        // Находим AudioManager
        audioManager = FindObjectOfType<AudioManager>();

        // Показываем рекорд в меню
        UpdateHighScoreUI();
    }

    void UpdateHighScoreUI()
    {
        if (highScoreText != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = $"Best Score: {highScore}";
        }
    }

    public void StartGame()
    {
        // Воспроизводим звук кнопки
        if (audioManager != null)
        {
            audioManager.PlayButtonClickSound();
        }

        Debug.Log("Starting game...");
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        // Воспроизводим звук кнопки
        if (audioManager != null)
        {
            audioManager.PlayButtonClickSound();
        }

        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
using UnityEngine;
using UnityEngine.UI; // Для работы с UI

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Ссылка на панель меню паузы
    private bool isPaused = false;

    void Update()
    {
        // Проверяем нажатие клавиши ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // Останавливаем или возобновляем время в игре
        Time.timeScale = isPaused ? 0f : 1f;

        // Показываем или скрываем меню паузы
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(isPaused);
        }

        // Опционально: отключаем или включаем управление игроком
        // playerController.enabled = !isPaused;
    }

    // Метод для кнопки "Продолжить"
    public void ResumeGame()
    {
        TogglePause();
    }

    // Метод для кнопки "Выйти в меню"
    public void QuitToMenu()
    {
        Time.timeScale = 1f; // Восстанавливаем время
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Замените "MainMenu" на имя вашей сцены с меню
    }
}

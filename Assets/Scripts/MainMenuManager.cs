using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Этот метод будет вызываться при нажатии кнопки "Start Game"
    public void StartGame()
    {
        Debug.Log("Starting game...");

        // Загружаем игровую сцену по имени
        SceneManager.LoadScene("SampleScene");
    }

    // Этот метод будет вызываться при нажатии кнопки "Quit"
    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        // Завершаем приложение
        Application.Quit();

        // Для тестирования в редакторе Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
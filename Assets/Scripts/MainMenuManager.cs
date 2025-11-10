using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        // Находим AudioManager
        audioManager = FindObjectOfType<AudioManager>();
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
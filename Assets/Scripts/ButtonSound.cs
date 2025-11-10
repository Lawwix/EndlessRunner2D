using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private Button button;
    private AudioManager audioManager;

    void Start()
    {
        // Получаем компонент Button
        button = GetComponent<Button>();

        // Находим AudioManager
        audioManager = FindObjectOfType<AudioManager>();

        // Добавляем обработчик клика
        if (button != null)
        {
            button.onClick.AddListener(PlayButtonSound);
        }
    }

    void PlayButtonSound()
    {
        // Воспроизводим звук кнопки
        if (audioManager != null)
        {
            audioManager.PlayButtonClickSound();
        }
    }

    void OnDestroy()
    {
        // Убираем обработчик при уничтожении объекта
        if (button != null)
        {
            button.onClick.RemoveListener(PlayButtonSound);
        }
    }
}

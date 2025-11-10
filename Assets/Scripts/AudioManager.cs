using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip gameOverSound;
    public AudioClip buttonClickSound;

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    void Awake()
    {
        // Делаем AudioManager синглтоном (будет работать между сценами)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Настраиваем аудио источники
        SetupAudioSources();
    }

    void SetupAudioSources()
    {
        // Настраиваем фоновую музыку
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    // Метод для воспроизведения звука прыжка
    public void PlayJumpSound()
    {
        PlaySFX(jumpSound);
    }

    // Метод для воспроизведения звука Game Over
    public void PlayGameOverSound()
    {
        PlaySFX(gameOverSound);
    }

    // Метод для воспроизведения звука кнопки
    public void PlayButtonClickSound()
    {
        PlaySFX(buttonClickSound);
    }

    // Общий метод для воспроизведения звуковых эффектов
    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
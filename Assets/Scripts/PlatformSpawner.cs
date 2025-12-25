using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public int platformCount = 100;
    public float platformDistance = 6f;

    [Header("Spawn Position")]
    public float spawnX = 15f;
    public float minY = -1f;
    public float maxY = 1f;

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public float initialObstacleChance = 0.5f;
    public float maxObstacleChance = 0.8f;
    public float obstacleIncreaseRate = 0.01f;

    [Header("Difficulty Settings")]
    public int scoreThreshold = 35; // Каждые 35 очков
    public float speedIncrement = 0.5f; // Увеличение скорости игрока
    public float maxSpeedMultiplier = 2.5f; // Максимальная скорость

    private List<GameObject> platforms = new List<GameObject>();
    private Vector2 lastPlatformPosition;
    private float currentObstacleChance;
    private int nextSpeedUpScore; // При каком счёте следующее ускорение
    private float initialPlayerSpeed; // Начальная скорость игрока
    private GameManager gameManager;
    private PlayerController playerController;
    private int difficultyLevel = 0;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            initialPlayerSpeed = playerController.runSpeed;
            Debug.Log($"Initial player speed: {initialPlayerSpeed}");
        }

        currentObstacleChance = initialObstacleChance;
        nextSpeedUpScore = scoreThreshold;

        // ПЕРВАЯ платформа точно под игроком
        lastPlatformPosition = new Vector2(0, -1f);

        // Создаем первую платформу отдельно чтобы игрок на ней стоял
        CreateFirstPlatform();

        // Создаем остальные платформы
        for (int i = 1; i < platformCount; i++)
        {
            SpawnPlatform();
        }

        Debug.Log("Platform spawner started. First platform at: " + lastPlatformPosition);
    }

    void Update()
    {
        // Увеличиваем шанс появления препятствий со временем (ОРИГИНАЛЬНАЯ ЛОГИКА)
        if (currentObstacleChance < maxObstacleChance)
        {
            currentObstacleChance += obstacleIncreaseRate * Time.deltaTime;
            currentObstacleChance = Mathf.Min(currentObstacleChance, maxObstacleChance);
        }

        // ОРИГИНАЛЬНАЯ ЛОГИКА переработки платформ
        if (platforms.Count > 0 && platforms[0].transform.position.x < -spawnX)
        {
            RecyclePlatform();
            SpawnPlatform();
        }

        // Проверяем счёт для увеличения скорости
        if (gameManager != null && Time.timeScale > 0)
        {
            CheckScoreForSpeedIncrease();
        }
    }

    void CreateFirstPlatform()
    {
        Vector2 firstPlatformPosition = new Vector2(0, -1f);
        GameObject firstPlatform = Instantiate(platformPrefab, firstPlatformPosition, Quaternion.identity);
        platforms.Add(firstPlatform);
        lastPlatformPosition = firstPlatformPosition;
    }

    void SpawnPlatform()
    {
        Vector2 spawnPosition = new Vector2(
            lastPlatformPosition.x + platformDistance,
            Random.Range(minY, maxY)
        );

        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
        lastPlatformPosition = spawnPosition;

        // ВОЗВРАЩАЕМ ОРИГИНАЛЬНУЮ ЛОГИКУ СПАУНА ПРЕПЯТСТВИЙ!
        // Препятствия только после 3-й платформы (как в вашем коде)
        if (platforms.Count > 3)
        {
            TrySpawnObstacle(spawnPosition);
        }
    }

    void RecyclePlatform()
    {
        if (platforms.Count > 0)
        {
            GameObject platformToRemove = platforms[0];
            platforms.RemoveAt(0);
            Destroy(platformToRemove);
        }
    }

    // ОРИГИНАЛЬНЫЙ МЕТОД спауна препятствий (как у вас было)
    void TrySpawnObstacle(Vector2 platformPosition)
    {
        // Проверяем, что игра не на паузе
        if (Time.timeScale == 0f) return;

        if (Random.Range(0f, 1f) < currentObstacleChance)
        {
            float platformWidth = 3f;
            float randomX = Random.Range(-platformWidth / 2 + 0.5f, platformWidth / 2 - 0.5f);
            Vector2 obstaclePosition = platformPosition + new Vector2(randomX, 0.7f);
            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);

            Debug.Log($"Obstacle spawned (Chance: {currentObstacleChance:F2})");
        }
    }

    void CheckScoreForSpeedIncrease()
    {
        // Получаем текущий счёт через GameManager
        int currentScore = GetCurrentScoreFromGameManager();

        if (currentScore >= nextSpeedUpScore)
        {
            difficultyLevel++;
            IncreasePlayerSpeed();

            // Также немного увеличиваем шанс препятствий с каждым уровнем
            currentObstacleChance = Mathf.Min(currentObstacleChance + 0.05f, maxObstacleChance);

            nextSpeedUpScore += scoreThreshold;

            Debug.Log($"<color=yellow>Difficulty Level {difficultyLevel}!</color> " +
                     $"Speed: {playerController.runSpeed}, " +
                     $"Obstacle chance: {currentObstacleChance:F2}, " +
                     $"Next at {nextSpeedUpScore} points");
        }
    }

    void IncreasePlayerSpeed()
    {
        if (playerController != null)
        {
            float maxSpeed = initialPlayerSpeed * maxSpeedMultiplier;

            if (playerController.runSpeed < maxSpeed)
            {
                playerController.runSpeed += speedIncrement;
                playerController.runSpeed = Mathf.Min(playerController.runSpeed, maxSpeed);

                Debug.Log($"Player speed increased to: {playerController.runSpeed}");

                // Визуальный эффект (опционально)
                StartCoroutine(SpeedBoostEffect());
            }
        }
    }

    // Простой визуальный эффект при ускорении (опционально)
    System.Collections.IEnumerator SpeedBoostEffect()
    {
        // Можно добавить изменение цвета игрока или частицы
        SpriteRenderer playerSprite = playerController.GetComponent<SpriteRenderer>();
        Color originalColor = playerSprite != null ? playerSprite.color : Color.white;

        if (playerSprite != null)
        {
            playerSprite.color = Color.yellow;
            yield return new WaitForSeconds(0.3f);
            playerSprite.color = originalColor;
        }
    }

    int GetCurrentScoreFromGameManager()
    {
        if (gameManager != null)
        {
            // Способ 1: если добавили свойство CurrentScore
            return gameManager.CurrentScore;

            // Способ 2: если добавили метод GetCurrentScore()
            // return gameManager.GetCurrentScore();
        }
        return 0;
    }

    //int GetCurrentScoreFromGameManager()
    //{
    //    if (gameManager != null)
    //    {
    //        // ВАЖНО: Добавьте в GameManager публичное свойство или метод для доступа к currentScore
    //        // Пока использую рефлексию как временное решение

    //        System.Reflection.FieldInfo field = typeof(GameManager).GetField("currentScore",
    //            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
    //        if (field != null)
    //        {
    //            return (int)field.GetValue(gameManager);
    //        }
    //    }

    //    return 0;
    //}

    // Метод для отладки: принудительно увеличить сложность
    public void ForceSpeedIncrease()
    {
        IncreasePlayerSpeed();
        Debug.Log($"Speed forced to: {playerController.runSpeed}");
    }

    // Метод для сброса сложности (вызывается из GameManager при рестарте)
    public void ResetDifficulty()
    {
        // Сбрасываем скорость игрока
        if (playerController != null)
        {
            playerController.runSpeed = initialPlayerSpeed;
        }

        // Сбрасываем параметры сложности
        currentObstacleChance = initialObstacleChance;
        difficultyLevel = 0;
        nextSpeedUpScore = scoreThreshold;

        Debug.Log("Difficulty reset to initial values");
    }
}



//using UnityEngine;
//using System.Collections.Generic;

//public class PlatformSpawner : MonoBehaviour
//{
//    [Header("Platform Settings")]
//    public GameObject platformPrefab;
//    public int platformCount = 100;
//    public float platformDistance = 6f;

//    [Header("Spawn Position")]
//    public float spawnX = 15f;
//    public float minY = -1f;
//    public float maxY = 1f;

//    [Header("Obstacle Settings")]
//    public GameObject obstaclePrefab;
//    public float initialObstacleChance = 0.5f;
//    public float maxObstacleChance = 0.8f;
//    public float obstacleIncreaseRate = 0.01f;

//    private List<GameObject> platforms = new List<GameObject>();
//    private Vector2 lastPlatformPosition;
//    private float currentObstacleChance;

//    void Start()
//    {
//        currentObstacleChance = initialObstacleChance;

//        // ПЕРВАЯ платформа точно под игроком
//        lastPlatformPosition = new Vector2(0, -1f);

//        // Создаем первую платформу отдельно чтобы игрок на ней стоял
//        CreateFirstPlatform();

//        // Создаем остальные платформы
//        for (int i = 1; i < platformCount; i++)
//        {
//            SpawnPlatform();
//        }

//        Debug.Log("Platform spawner started. First platform created at: " + lastPlatformPosition);
//    }

//    void CreateFirstPlatform()
//    {
//        Vector2 firstPlatformPosition = new Vector2(0, -1f);
//        GameObject firstPlatform = Instantiate(platformPrefab, firstPlatformPosition, Quaternion.identity);
//        platforms.Add(firstPlatform);
//        lastPlatformPosition = firstPlatformPosition;
//    }

//    void Update()
//    {
//        if (currentObstacleChance < maxObstacleChance)
//        {
//            currentObstacleChance += obstacleIncreaseRate * Time.deltaTime;
//            currentObstacleChance = Mathf.Min(currentObstacleChance, maxObstacleChance);
//        }

//        if (platforms.Count > 0 && platforms[0].transform.position.x < -spawnX)
//        {
//            RecyclePlatform();
//            SpawnPlatform();
//        }
//    }

//    void SpawnPlatform()
//    {
//        Vector2 spawnPosition = new Vector2(
//            lastPlatformPosition.x + platformDistance,
//            Random.Range(minY, maxY)
//        );

//        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
//        platforms.Add(platform);
//        lastPlatformPosition = spawnPosition;

//        // Препятствия только после 3-й платформы
//        if (platforms.Count > 3)
//        {
//            TrySpawnObstacle(spawnPosition);
//        }
//    }

//    void RecyclePlatform()
//    {
//        if (platforms.Count > 0)
//        {
//            GameObject platformToRemove = platforms[0];
//            platforms.RemoveAt(0);
//            Destroy(platformToRemove);
//        }
//    }

//    void TrySpawnObstacle(Vector2 platformPosition)
//    {
//        if (Random.Range(0f, 1f) < currentObstacleChance)
//        {
//            float platformWidth = 3f;
//            float randomX = Random.Range(-platformWidth / 2 + 0.5f, platformWidth / 2 - 0.5f);
//            Vector2 obstaclePosition = platformPosition + new Vector2(randomX, 0.7f);
//            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
//        }
//    }
//}



//using UnityEngine;
//using System.Collections.Generic;

//public class PlatformSpawner : MonoBehaviour
//{
//    [Header("Platform Settings")]
//    public GameObject platformPrefab;
//    public int platformCount = 12; // БЫЛО: 8, СТАЛО: 12
//    public float platformDistance = 6f;

//    [Header("Spawn Position")]
//    public float spawnX = 15f;
//    public float minY = -0.5f;  // БЫЛО: -1f, СТАЛО: -0.5f (не так низко)
//    public float maxY = 1.5f;   // БЫЛО: 1f, СТАЛО: 1.5f (можно выше)

//    [Header("Obstacle Settings")]
//    public GameObject obstaclePrefab;
//    public float obstacleSpawnChance = 0.8f; // БЫЛО: 0.6f, СТАЛО: 0.8f (80% шанс)

//    private List<GameObject> platforms = new List<GameObject>();
//    private Vector2 lastPlatformPosition;

//    void Start()
//    {
//        // Создаем первую платформу точно под игроком
//        Vector2 firstPlatformPosition = new Vector2(0, -3); // Ниже игрока
//        GameObject firstPlatform = Instantiate(platformPrefab, firstPlatformPosition, Quaternion.identity);
//        platforms.Add(firstPlatform);
//        lastPlatformPosition = firstPlatformPosition;

//        // Создаем остальные платформы
//        for (int i = 1; i < platformCount; i++)
//        {
//            SpawnPlatform();
//        }
//    }

//    void Update()
//    {
//        // Проверяем, нужно ли генерировать новые платформы
//        if (platforms.Count > 0 && platforms[0].transform.position.x < -spawnX)
//        {
//            RecyclePlatform();
//            SpawnPlatform();
//        }
//    }

//    void SpawnPlatform()
//    {
//        // Ограничиваем максимальное изменение высоты от предыдущей платформы
//        float maxHeightChange = 1f;

//        float targetY = Random.Range(minY, maxY);

//        // Плавное изменение высоты (не более maxHeightChange от предыдущей)
//        float newY = Mathf.Clamp(
//            targetY,
//            lastPlatformPosition.y - maxHeightChange,
//            lastPlatformPosition.y + maxHeightChange
//        );

//        // Защита от слишком низких платформ (не ниже -1)
//        newY = Mathf.Max(newY, -1f);

//        Vector2 spawnPosition = new Vector2(
//            lastPlatformPosition.x + platformDistance,
//            newY
//        );

//        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
//        platforms.Add(platform);
//        lastPlatformPosition = spawnPosition;

//        if (platforms.Count > 2)
//        {
//            TrySpawnObstacle(spawnPosition);
//        }
//    }

//    void RecyclePlatform()
//    {
//        // Удаляем самую левую платформу и создаем новую справа
//        GameObject platformToRemove = platforms[0];
//        platforms.RemoveAt(0);
//        Destroy(platformToRemove);
//    }

//    void TrySpawnObstacle(Vector2 platformPosition)
//    {
//        // Увеличиваем шанс до 80%
//        if (Random.Range(0f, 1f) < obstacleSpawnChance)
//        {
//            // Случайная позиция на платформе (не слишком близко к краям)
//            float platformWidth = 3f; // Длина платформы
//            float randomX = Random.Range(-platformWidth / 2 + 0.5f, platformWidth / 2 - 0.5f);

//            Vector2 obstaclePosition = platformPosition + new Vector2(randomX, 0.7f);
//            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
//        }
//    }
//}
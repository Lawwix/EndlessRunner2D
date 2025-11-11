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

    private List<GameObject> platforms = new List<GameObject>();
    private Vector2 lastPlatformPosition;
    private float currentObstacleChance;

    void Start()
    {
        currentObstacleChance = initialObstacleChance;

        // ПЕРВАЯ платформа точно под игроком
        lastPlatformPosition = new Vector2(0, -1f);

        // Создаем первую платформу отдельно чтобы игрок на ней стоял
        CreateFirstPlatform();

        // Создаем остальные платформы
        for (int i = 1; i < platformCount; i++)
        {
            SpawnPlatform();
        }

        Debug.Log("Platform spawner started. First platform created at: " + lastPlatformPosition);
    }

    void CreateFirstPlatform()
    {
        Vector2 firstPlatformPosition = new Vector2(0, -1f);
        GameObject firstPlatform = Instantiate(platformPrefab, firstPlatformPosition, Quaternion.identity);
        platforms.Add(firstPlatform);
        lastPlatformPosition = firstPlatformPosition;
    }

    void Update()
    {
        if (currentObstacleChance < maxObstacleChance)
        {
            currentObstacleChance += obstacleIncreaseRate * Time.deltaTime;
            currentObstacleChance = Mathf.Min(currentObstacleChance, maxObstacleChance);
        }

        if (platforms.Count > 0 && platforms[0].transform.position.x < -spawnX)
        {
            RecyclePlatform();
            SpawnPlatform();
        }
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

        // Препятствия только после 3-й платформы
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

    void TrySpawnObstacle(Vector2 platformPosition)
    {
        if (Random.Range(0f, 1f) < currentObstacleChance)
        {
            float platformWidth = 3f;
            float randomX = Random.Range(-platformWidth / 2 + 0.5f, platformWidth / 2 - 0.5f);
            Vector2 obstaclePosition = platformPosition + new Vector2(randomX, 0.7f);
            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
        }
    }
}

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
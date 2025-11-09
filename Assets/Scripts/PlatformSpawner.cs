using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public int platformCount = 8;
    public float platformDistance = 8f; // Должно работать с Scale X = 6

    [Header("Spawn Position")]
    public float spawnX = 20f;
    public float minY = -0.5f;    // БЫЛО: -1.5f (меньший разброс вниз)
    public float maxY = 0.5f;     // БЫЛО: 1.5f (меньший разброс вверх)

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public float obstacleSpawnChance = 0.6f;

    private List<GameObject> platforms = new List<GameObject>();
    private Vector2 lastPlatformPosition;
    private float platformWidth = 6f; // Должно совпадать с Scale X платформы

    void Start()
    {
        // Создаем первую платформу точно под игроком
        Vector2 firstPlatformPosition = new Vector2(0, -2f);
        GameObject firstPlatform = Instantiate(platformPrefab, firstPlatformPosition, Quaternion.identity);
        platforms.Add(firstPlatform);
        lastPlatformPosition = firstPlatformPosition;

        // Создаем остальные платформы
        for (int i = 1; i < platformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        // Проверяем самую левую платформу
        if (platforms.Count > 0 && platforms[0].transform.position.x < -spawnX)
        {
            RecyclePlatform();
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        // Ограничиваем максимальное изменение высоты между платформами
        float maxHeightChange = 1f; // Максимальное изменение высоты от предыдущей платформы

        float targetY = Random.Range(minY, maxY);

        // Плавно изменяем высоту от предыдущей платформы
        float newY = Mathf.Clamp(
            targetY,
            lastPlatformPosition.y - maxHeightChange,
            lastPlatformPosition.y + maxHeightChange
        );

        // Правильно вычисляем позицию следующей платформы
        Vector2 spawnPosition = new Vector2(
            lastPlatformPosition.x + platformDistance,
            newY
        );

        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
        lastPlatformPosition = spawnPosition;

        // Пытаемся создать препятствие на этой платформе (кроме первых двух)
        if (platforms.Count > 2)
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
        if (Random.Range(0f, 1f) < obstacleSpawnChance)
        {
            // Случайная позиция на платформе (не слишком близко к краям)
            float randomX = Random.Range(-platformWidth / 2 + 1f, platformWidth / 2 - 1f);
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
//    public int platformCount = 5;
//    public float platformDistance = 4f;

//    [Header("Spawn Position")]
//    public float spawnX = 15f;
//    public float minY = -1f;
//    public float maxY = 2f;

//    [Header("Obstacle Settings")]
//    public GameObject obstaclePrefab;
//    public float obstacleSpawnChance = 0.3f; // 30% шанс появления препятствия

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
//        // Создаем новую платформу
//        Vector2 spawnPosition = lastPlatformPosition + new Vector2(platformDistance, Random.Range(minY, maxY));
//        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

//        platforms.Add(platform);
//        lastPlatformPosition = spawnPosition;

//        // Пытаемся создать препятствие на этой платформе
//        TrySpawnObstacle(spawnPosition);
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
//        // Случайно решаем, создавать ли препятствие (30% шанс)
//        if (Random.Range(0f, 1f) < obstacleSpawnChance)
//        {
//            // Случайная позиция на платформе (не в самом начале)
//            float platformWidth = 5f; // Длина платформы
//            float randomX = Random.Range(-platformWidth / 2 + 1f, platformWidth / 2 - 1f);

//            // Создаем препятствие на платформе
//            Vector2 obstaclePosition = platformPosition + new Vector2(randomX, 0.7f);
//            Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
//        }
//    }
//}
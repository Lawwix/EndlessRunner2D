using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public int platformCount = 8;
    public float platformDistance = 6f;

    [Header("Spawn Position")]
    public float spawnX = 15f;
    public float minY = -1f;
    public float maxY = 1f;

    [Header("Obstacle Settings")]
    public GameObject obstaclePrefab;
    public float obstacleSpawnChance = 0.6f;

    private List<GameObject> platforms = new List<GameObject>();
    private Vector2 lastPlatformPosition;

    void Start()
    {
        // ПЕРВАЯ платформа создается точно под игроком
        lastPlatformPosition = new Vector2(0, -1f); // Y=-1 чтобы игрок стоял на ней

        for (int i = 0; i < platformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (platforms.Count > 0 && platforms[0].transform.position.x < -spawnX)
        {
            RecyclePlatform();
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        Vector2 spawnPosition = lastPlatformPosition + new Vector2(platformDistance, Random.Range(minY, maxY));
        GameObject platform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
        lastPlatformPosition = spawnPosition;

        if (platforms.Count > 2)
        {
            TrySpawnObstacle(spawnPosition);
        }
    }

    void RecyclePlatform()
    {
        GameObject platformToRemove = platforms[0];
        platforms.RemoveAt(0);
        Destroy(platformToRemove);
    }

    void TrySpawnObstacle(Vector2 platformPosition)
    {
        if (Random.Range(0f, 1f) < obstacleSpawnChance)
        {
            Vector2 obstaclePosition = platformPosition + new Vector2(0, 0.7f);
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
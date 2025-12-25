using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform cameraTransform; // Ссылка на трансформ камеры
    public float parallaxSpeed = 0.5f; // 1 = движется как камера, 0.5 = в 2 раза медленнее (параллакс)
    private Vector3 previousCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // 1. Насколько камера сместилась с прошлого кадра?
        float deltaX = cameraTransform.position.x - previousCameraPosition.x;

        // 2. Двигаем фон на долю от этого смещения
        // Округляем до целых пикселей для устранения дёргания
        float pixelSize = 1f / 100f; // Pixels Per Unit = 100
        float parallaxMovement = Mathf.Round((deltaX * parallaxSpeed) / pixelSize) * pixelSize;

        transform.Translate(new Vector3(parallaxMovement, 0, 0));

        // 3. Обновляем позицию камеры для следующего кадра
        previousCameraPosition = cameraTransform.position;
    }
}
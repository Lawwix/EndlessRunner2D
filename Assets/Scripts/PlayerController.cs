using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 10f;
    public float runSpeed = 5f; // Новая переменная для скорости бега

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGameRunning = true; // Контроль игры

    void Start()
    {
        // Получаем компонент Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isGameRunning) return;

        // Проверяем, стоит ли игрок на земле
        CheckGrounded();

        // Обрабатываем прыжок при нажатии пробела или клике мыши
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (!isGameRunning) return;

        // Постоянное движение вперед
        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    void CheckGrounded()
    {
        // Проверяем, касается ли игрок земли
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void Jump()
    {
        // Применяем силу для прыжка
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // Метод для остановки игрока (при столкновении с препятствием)
    public void StopRunning()
    {
        isGameRunning = false;
        rb.velocity = Vector2.zero;
    }
}

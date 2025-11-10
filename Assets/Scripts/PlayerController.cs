using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 16f; // УВЕЛИЧЕНО!
    public float runSpeed = 3f;   // нормальная скорость

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGameRunning = true; // Добавляем контроль игры

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isGameRunning) return; // Игра остановлена - выходим

        CheckGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (!isGameRunning) return; // Игра остановлена - не двигаемся

        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StopRunning();

            // Находим GameManager и сообщаем о Game Over
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
    }

    public void StopRunning()
    {
        isGameRunning = false;
        rb.velocity = Vector2.zero; // Полная остановка
    }

    void CheckFallDeath()
    {
        if (transform.position.y < -5f)
        {
            StopRunning();

            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
    }
}
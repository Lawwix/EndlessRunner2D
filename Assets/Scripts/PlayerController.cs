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

    [Header("Audio")]
    public AudioManager audioManager;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGameRunning = true; // Добавляем контроль игры

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Находим AudioManager если не назначен
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
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

        // Воспроизводим звук прыжка
        if (audioManager != null)
        {
            audioManager.PlayJumpSound();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Воспроизводим звук Game Over
            if (audioManager != null)
            {
                audioManager.PlayGameOverSound();
            }

            StopRunning();
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
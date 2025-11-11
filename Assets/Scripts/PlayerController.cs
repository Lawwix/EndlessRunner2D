using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 16f;
    public float runSpeed = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGameRunning = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    void Update()
    {
        if (!isGameRunning) return;

        CheckGrounded();
        CheckFallDeath();

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void CheckFallDeath()
    {
        if (transform.position.y < -10f)
        {
            GameOver();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayJumpSound();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGameRunning) return;

        // ЛЮБОЕ столкновение с Obstacle = Game Over
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
            return;
        }

        // Столкновение с платформой сбоку = Game Over
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // Если нормаль указывает в сторону (столкновение сбоку)
                if (Mathf.Abs(contact.normal.x) > 0.7f)
                {
                    GameOver();
                    return;
                }
            }
        }
    }

    void GameOver()
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        rb.velocity = Vector2.zero;

        // Звук Game Over
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSound();
        }

        // Вызов Game Over в GameManager
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}

//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    public float jumpForce = 16f;
//    public float runSpeed = 5f;
//    public Transform groundCheck;
//    public float checkRadius = 0.3f;
//    public LayerMask groundLayer;

//    private Rigidbody2D rb;
//    private bool isGrounded;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        if (rb != null) rb.freezeRotation = true;
//    }

//    void Update()
//    {
//        // Проверяем землю под ногами
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

//        // Прыжок
//        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
//        {
//            Jump();
//        }

//        // Проверка падения
//        if (transform.position.y < -10f)
//        {
//            FindObjectOfType<GameManager>()?.GameOver();
//        }
//    }

//    void FixedUpdate()
//    {
//        // Постоянное движение вперед
//        if (rb != null)
//        {
//            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
//        }
//    }

//    void Jump()
//    {
//        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//        AudioManager.Instance?.PlayJumpSound();
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            AudioManager.Instance?.PlayGameOverSound();
//            FindObjectOfType<GameManager>()?.GameOver();
//        }
//    }

//    public void StopRunning()
//    {
//        if (rb != null) rb.velocity = Vector2.zero;
//    }
//}

//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float jumpForce = 16f; // УВЕЛИЧЕНО!
//    public float runSpeed = 3f;   // нормальная скорость

//    [Header("Ground Check")]
//    public Transform groundCheck;
//    public float checkRadius = 0.2f;
//    public LayerMask groundLayer;

//    [Header("Audio")]
//    public AudioManager audioManager;

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isGameRunning = true; // Добавляем контроль игры

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();

//        // Находим AudioManager если не назначен
//        if (audioManager == null)
//        {
//            audioManager = FindObjectOfType<AudioManager>();
//        }
//    }

//    void Update()
//    {
//        if (!isGameRunning) return; // Игра остановлена - выходим

//        CheckGrounded();

//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            Jump();
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!isGameRunning) return; // Игра остановлена - не двигаемся

//        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
//    }

//    void CheckGrounded()
//    {
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
//    }

//    void Jump()
//    {
//        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

//        // Воспроизводим звук прыжка
//        if (audioManager != null)
//        {
//            audioManager.PlayJumpSound();
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            // Воспроизводим звук Game Over
//            if (audioManager != null)
//            {
//                audioManager.PlayGameOverSound();
//            }

//            StopRunning();
//            GameManager gameManager = FindObjectOfType<GameManager>();
//            if (gameManager != null)
//            {
//                gameManager.GameOver();
//            }
//        }
//    }

//    public void StopRunning()
//    {
//        isGameRunning = false;
//        rb.velocity = Vector2.zero; // Полная остановка
//    }

//    void CheckFallDeath()
//    {
//        if (transform.position.y < -5f)
//        {
//            StopRunning();

//            GameManager gameManager = FindObjectOfType<GameManager>();
//            if (gameManager != null)
//            {
//                gameManager.GameOver();
//            }
//        }
//    }
//}
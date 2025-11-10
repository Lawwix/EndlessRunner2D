using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 16f;
    public float initialRunSpeed = 5f;
    public float maxRunSpeed = 10f;
    public float speedIncreaseRate = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Game Over Settings")]
    public float fallThreshold = -10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGameRunning = true;
    private float currentRunSpeed;
    private float gameTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentRunSpeed = initialRunSpeed;
        gameTime = 0f;

        if (rb != null)
        {
            rb.freezeRotation = true;
        }
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (!isGameRunning) return;

        gameTime += Time.deltaTime;

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

        IncreaseSpeedOverTime();
        rb.velocity = new Vector2(currentRunSpeed, rb.velocity.y);
    }

    void IncreaseSpeedOverTime()
    {
        if (currentRunSpeed < maxRunSpeed)
        {
            currentRunSpeed = initialRunSpeed + (gameTime * speedIncreaseRate);
            currentRunSpeed = Mathf.Min(currentRunSpeed, maxRunSpeed);
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void CheckFallDeath()
    {
        if (transform.position.y < fallThreshold)
        {
            StopRunning();
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // Находим AudioManager автоматически
        AudioManager audioManager = AudioManager.Instance;
        if (audioManager != null)
        {
            audioManager.PlayJumpSound();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Столкновение с препятствием - Game Over
            AudioManager audioManager = AudioManager.Instance;
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
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Столкновение с платформой сбоку - отталкиваем игрока
            HandlePlatformSideCollision(collision);
        }
    }

    void HandlePlatformSideCollision(Collision2D collision)
    {
        // Проверяем что столкновение сбоку (не сверху)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > 0.5f) // Столкновение сбоку
            {
                // Слегка отталкиваем игрока от платформы используя currentRunSpeed
                rb.velocity = new Vector2(currentRunSpeed, rb.velocity.y);

                // Можно добавить небольшой толчок вверх чтобы помочь игроку
                if (isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.3f);
                }
                break;
            }
        }
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Obstacle"))
    //    {
    //        AudioManager audioManager = AudioManager.Instance;
    //        if (audioManager != null)
    //        {
    //            audioManager.PlayGameOverSound();
    //        }

    //        StopRunning();
    //        GameManager gameManager = FindObjectOfType<GameManager>();
    //        if (gameManager != null)
    //        {
    //            gameManager.GameOver();
    //        }
    //    }
    //}

    public void StopRunning()
    {
        isGameRunning = false;
        rb.velocity = Vector2.zero;
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
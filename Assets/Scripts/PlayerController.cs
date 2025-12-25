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
        if (Time.timeScale == 0f) // Если игра на паузе
            return;

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
                // Если нормал указывает в сторону (столкновение сбоку)
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

//using UnityEngine;      пристолкновении лажа и препятствия улетают
//using System.Collections;

//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float jumpForce = 16f;
//    public float runSpeed = 5f;

//    [Header("Life Settings")]
//    public int maxLives = 3;
//    public int currentLives = 3;
//    public float invincibilityTime = 1.5f;

//    [Header("Ground Check")]
//    public Transform groundCheck;
//    public float checkRadius = 0.3f;
//    public LayerMask groundLayer;

//    [Header("Collision Settings")]
//    public float sideBounceForce = 3f; // Сила отскока от боков
//    public float obstacleBounceForce = 4f; // Сила отскока от препятствий

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isGameRunning = true;
//    private bool isInvincible = false;
//    private SpriteRenderer spriteRenderer;
//    private float lastDamageTime = 0f;
//    private bool canTakeDamage = true;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        spriteRenderer = GetComponent<SpriteRenderer>();

//        if (rb != null)
//        {
//            rb.freezeRotation = true;
//            // Важные настройки физики
//            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
//            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
//        }

//        currentLives = maxLives;
//        UpdateLivesUI();
//    }

//    void Update()
//    {
//        if (Time.timeScale == 0f) return;
//        if (!isGameRunning) return;

//        CheckGrounded();
//        CheckFallDeath();

//        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
//        {
//            Jump();
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!isGameRunning) return;
//        // Плавное движение вперед
//        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
//    }

//    void CheckGrounded()
//    {
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
//    }

//    void CheckFallDeath()
//    {
//        if (transform.position.y < -10f)
//        {
//            currentLives = 0;
//            UpdateLivesUI();
//            GameOver();
//        }
//    }

//    void Jump()
//    {
//        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayJumpSound();
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (!isGameRunning || isInvincible || !canTakeDamage) return;

//        // Полная защита от множественных срабатываний
//        canTakeDamage = false;
//        StartCoroutine(ResetDamageCooldown());

//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            HandleObstacleCollision(collision);
//            return;
//        }

//        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//        {
//            HandleGroundSideCollision(collision);
//            return;
//        }
//    }

//    void HandleObstacleCollision(Collision2D collision)
//    {
//        // Отскок от препятствия
//        Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
//        bounceDirection.y = Mathf.Abs(bounceDirection.y); // Всегда вверх

//        rb.velocity = new Vector2(-obstacleBounceForce, obstacleBounceForce);

//        TakeDamage();
//    }

//    void HandleGroundSideCollision(Collision2D collision)
//    {
//        bool hitSide = false;
//        Vector2 averageNormal = Vector2.zero;

//        foreach (ContactPoint2D contact in collision.contacts)
//        {
//            if (Mathf.Abs(contact.normal.x) > 0.5f)
//            {
//                hitSide = true;
//                averageNormal += contact.normal;
//            }
//        }

//        if (hitSide)
//        {
//            averageNormal.Normalize();
//            // Отскок от стороны платформы
//            rb.velocity = new Vector2(-sideBounceForce * Mathf.Sign(averageNormal.x), sideBounceForce);

//            TakeDamage();
//        }
//    }

//    IEnumerator ResetDamageCooldown()
//    {
//        yield return new WaitForSeconds(0.3f); // Задержка между уронами
//        canTakeDamage = true;
//    }

//    void TakeDamage()
//    {
//        if (isInvincible) return;

//        currentLives--;
//        UpdateLivesUI();

//        // Убедимся, что персонаж продолжает движение после урона
//        StartCoroutine(ResumeMovementAfterHit());

//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayGameOverSound(); // Или создайте отдельный звук урона
//        }

//        StartCoroutine(DamageEffect());

//        if (currentLives <= 0)
//        {
//            StartCoroutine(DelayedGameOver(0.5f));
//        }
//        else
//        {
//            StartCoroutine(ActivateInvincibility());
//        }
//    }

//    IEnumerator ResumeMovementAfterHit()
//    {
//        yield return new WaitForSeconds(0.2f);

//        // Восстанавливаем нормальное движение
//        if (isGameRunning && rb != null)
//        {
//            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
//        }
//    }

//    IEnumerator DelayedGameOver(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        GameOver();
//    }

//    IEnumerator DamageEffect()
//    {
//        if (spriteRenderer != null)
//        {
//            for (int i = 0; i < 5; i++)
//            {
//                spriteRenderer.color = Color.red;
//                yield return new WaitForSeconds(0.08f);
//                spriteRenderer.color = Color.white;
//                yield return new WaitForSeconds(0.08f);
//            }
//        }
//    }

//    IEnumerator ActivateInvincibility()
//    {
//        isInvincible = true;

//        if (spriteRenderer != null)
//        {
//            float timer = 0f;
//            while (timer < invincibilityTime)
//            {
//                spriteRenderer.enabled = !spriteRenderer.enabled;
//                yield return new WaitForSeconds(0.08f);
//                timer += 0.16f;
//            }
//            spriteRenderer.enabled = true;
//        }
//        else
//        {
//            yield return new WaitForSeconds(invincibilityTime);
//        }

//        isInvincible = false;
//    }

//    void UpdateLivesUI()
//    {
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.UpdateLivesUI(currentLives);
//        }
//    }

//    void GameOver()
//    {
//        if (!isGameRunning) return;

//        isGameRunning = false;
//        if (rb != null) rb.velocity = Vector2.zero;

//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.GameOver();
//        }
//    }

//    void OnDrawGizmosSelected()
//    {
//        if (groundCheck != null)
//        {
//            Gizmos.color = isGrounded ? Color.green : Color.red;
//            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
//        }
//    }
//}

//using UnityEngine;   при столкновении лажа
//using System.Collections;

//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float jumpForce = 16f;
//    public float runSpeed = 5f;

//    [Header("Life Settings")]
//    public int maxLives = 3;
//    public int currentLives = 3;
//    public float invincibilityTime = 1.5f;

//    [Header("Ground Check")]
//    public Transform groundCheck;
//    public float checkRadius = 0.3f;
//    public LayerMask groundLayer;

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isGameRunning = true;
//    private bool isInvincible = false;
//    private SpriteRenderer spriteRenderer;
//    private float lastDamageTime = 0f; // Защита от двойного урона

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        spriteRenderer = GetComponent<SpriteRenderer>();

//        if (rb != null)
//        {
//            rb.freezeRotation = true;
//        }

//        currentLives = maxLives;
//        UpdateLivesUI();
//    }

//    void Update()
//    {
//        if (Time.timeScale == 0f) // Если игра на паузе
//            return;

//        if (!isGameRunning) return;

//        CheckGrounded();
//        CheckFallDeath();

//        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
//        {
//            Jump();
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!isGameRunning) return;
//        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
//    }

//    void CheckGrounded()
//    {
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
//    }

//    void CheckFallDeath()
//    {
//        if (transform.position.y < -10f)
//        {
//            // При падении отнимаем все жизни
//            currentLives = 0;
//            UpdateLivesUI();
//            GameOver();
//        }
//    }

//    void Jump()
//    {
//        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayJumpSound();
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (!isGameRunning || isInvincible) return;

//        // ЗАЩИТА: ждем 0.1 секунды перед следующим уроном
//        if (Time.time - lastDamageTime < 0.1f) return;

//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            lastDamageTime = Time.time;
//            TakeDamage();
//            return;
//        }

//        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//        {
//            bool hitSide = false;
//            foreach (ContactPoint2D contact in collision.contacts)
//            {
//                if (Mathf.Abs(contact.normal.x) > 0.7f)
//                {
//                    hitSide = true;
//                    break;
//                }
//            }

//            if (hitSide)
//            {
//                lastDamageTime = Time.time;
//                TakeDamage();
//                return;
//            }
//        }
//    }

//    //void OnCollisionEnter2D(Collision2D collision)
//    //{
//    //    if (!isGameRunning || isInvincible) return;

//    //    if (collision.gameObject.CompareTag("Obstacle"))
//    //    {
//    //        TakeDamage();
//    //        return;
//    //    }

//    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//    //    {
//    //        foreach (ContactPoint2D contact in collision.contacts)
//    //        {
//    //            if (Mathf.Abs(contact.normal.x) > 0.7f)
//    //            {
//    //                TakeDamage();
//    //                return;
//    //            }
//    //        }
//    //    }
//    //}

//    void TakeDamage()
//    {
//        if (isInvincible) return;

//        currentLives--;
//        UpdateLivesUI();

//        // ДОБАВЬТЕ ЭТО: небольшой отскок при получении урона
//        if (rb != null)
//        {
//            // Отталкиваем немного вверх и в противоположную сторону
//            rb.velocity = new Vector2(-2f, 5f);
//        }

//        StartCoroutine(DamageEffect());

//        if (currentLives <= 0)
//        {
//            GameOver();
//        }
//        else
//        {
//            StartCoroutine(ActivateInvincibility());
//        }
//    }

//    //void TakeDamage()
//    //{
//    //    if (isInvincible) return;

//    //    currentLives--;
//    //    UpdateLivesUI();

//    //    if (AudioManager.Instance != null)
//    //    {
//    //        // Если нет отдельного звука урона, используем звук GameOver
//    //        AudioManager.Instance.PlayGameOverSound();
//    //    }

//    //    StartCoroutine(DamageEffect());

//    //    if (currentLives <= 0)
//    //    {
//    //        GameOver();
//    //    }
//    //    else
//    //    {
//    //        StartCoroutine(ActivateInvincibility());
//    //    }
//    //}

//    IEnumerator DamageEffect()
//    {
//        if (spriteRenderer != null)
//        {
//            for (int i = 0; i < 3; i++)
//            {
//                spriteRenderer.color = Color.red;
//                yield return new WaitForSeconds(0.1f);
//                spriteRenderer.color = Color.white;
//                yield return new WaitForSeconds(0.1f);
//            }
//        }
//    }

//    IEnumerator ActivateInvincibility()
//    {
//        isInvincible = true;

//        if (spriteRenderer != null)
//        {
//            float timer = 0f;
//            while (timer < invincibilityTime)
//            {
//                spriteRenderer.enabled = !spriteRenderer.enabled;
//                yield return new WaitForSeconds(0.1f);
//                timer += 0.2f;
//            }
//            spriteRenderer.enabled = true;
//        }
//        else
//        {
//            yield return new WaitForSeconds(invincibilityTime);
//        }

//        isInvincible = false;
//    }

//    void UpdateLivesUI()
//    {
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.UpdateLivesUI(currentLives);
//        }
//    }

//    void GameOver()
//    {
//        if (!isGameRunning) return;

//        isGameRunning = false;
//        rb.velocity = Vector2.zero;

//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayGameOverSound();
//        }

//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.GameOver();
//        }
//    }

//    void OnDrawGizmosSelected()
//    {
//        if (groundCheck != null)
//        {
//            Gizmos.color = isGrounded ? Color.green : Color.red;
//            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
//        }
//    }
//}

//using UnityEngine;   мб норм

//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float jumpForce = 16f;
//    public float runSpeed = 5f;

//    [Header("Life Settings")]
//    public int maxLives = 3;
//    public int currentLives = 3;
//    public float invincibilityTime = 1.5f; // Время неуязвимости после получения урона

//    [Header("Ground Check")]
//    public Transform groundCheck;
//    public float checkRadius = 0.3f;
//    public LayerMask groundLayer;

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isGameRunning = true;
//    private bool isInvincible = false;
//    private SpriteRenderer spriteRenderer;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        spriteRenderer = GetComponent<SpriteRenderer>();

//        if (rb != null)
//        {
//            rb.freezeRotation = true;
//        }

//        currentLives = maxLives;

//        // Обновляем UI жизней
//        UpdateLivesUI();
//    }

//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float jumpForce = 16f;
//    public float runSpeed = 5f;

//    [Header("Ground Check")]
//    public Transform groundCheck;
//    public float checkRadius = 0.3f;
//    public LayerMask groundLayer;

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isGameRunning = true;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        if (rb != null)
//        {
//            rb.freezeRotation = true;
//        }
//    }

//    void Update()
//    {
//        if (Time.timeScale == 0f) // Если игра на паузе
//            return;

//        if (!isGameRunning) return;

//        CheckGrounded();
//        CheckFallDeath();

//        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
//        {
//            Jump();
//        }
//    }

//    void FixedUpdate()
//    {
//        if (!isGameRunning) return;

//        // Постоянное движение вперед
//        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
//    }

//    void CheckGrounded()
//    {
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
//    }

//    void CheckFallDeath()
//    {
//        if (transform.position.y < -10f)
//        {
//            GameOver();
//        }
//    }

//    void Jump()
//    {
//        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayJumpSound();
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (!isGameRunning || isInvincible) return;

//        // Столкновение с Obstacle = получение урона
//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            TakeDamage();
//            return;
//        }

//        // Столкновение с платформой сбоку = получение урона
//        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//        {
//            foreach (ContactPoint2D contact in collision.contacts)
//            {
//                if (Mathf.Abs(contact.normal.x) > 0.7f)
//                {
//                    TakeDamage();
//                    return;
//                }
//            }
//        }
//    }

//    void TakeDamage()
//    {
//        if (isInvincible) return;

//        currentLives--;

//        // Обновляем UI жизней
//        UpdateLivesUI();

//        // Звук получения урона
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayDamageSound(); // Нужно добавить этот метод в AudioManager
//        }

//        // Визуальный эффект повреждения
//        StartCoroutine(DamageEffect());

//        if (currentLives <= 0)
//        {
//            GameOver();
//        }
//        else
//        {
//            // Активируем неуязвимость
//            StartCoroutine(ActivateInvincibility());
//        }
//    }

//    IEnumerator DamageEffect()
//    {
//        // Мигание спрайта
//        if (spriteRenderer != null)
//        {
//            for (int i = 0; i < 3; i++)
//            {
//                spriteRenderer.color = Color.red;
//                yield return new WaitForSeconds(0.1f);
//                spriteRenderer.color = Color.white;
//                yield return new WaitForSeconds(0.1f);
//            }
//        }
//    }

//    IEnumerator ActivateInvincibility()
//    {
//        isInvincible = true;

//        // Мигание во время неуязвимости
//        if (spriteRenderer != null)
//        {
//            float timer = 0f;
//            while (timer < invincibilityTime)
//            {
//                spriteRenderer.enabled = !spriteRenderer.enabled;
//                yield return new WaitForSeconds(0.1f);
//                timer += 0.2f;
//            }
//            spriteRenderer.enabled = true;
//        }
//        else
//        {
//            yield return new WaitForSeconds(invincibilityTime);
//        }

//        isInvincible = false;
//    }

//    void UpdateLivesUI()
//    {
//        // Вызываем метод в UIManager или GameManager для обновления UI
//        UIManager uiManager = FindObjectOfType<UIManager>();
//        if (uiManager != null)
//        {
//            uiManager.UpdateLivesDisplay(currentLives);
//        }

//        // Или через GameManager
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.UpdateLivesUI(currentLives);
//        }
//    }

//    // Изменяем метод GameOver чтобы он вызывался только при смерти
//    void GameOver()
//    {
//        if (!isGameRunning) return;

//        isGameRunning = false;
//        rb.velocity = Vector2.zero;

//        // Звук Game Over
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayGameOverSound();
//        }

//        // Вызов Game Over в GameManager
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.GameOver();
//        }
//    }
//}

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (!isGameRunning) return;

//        // ЛЮБОЕ столкновение с Obstacle = Game Over
//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            GameOver();
//            return;
//        }

//        // Столкновение с платформой сбоку = Game Over
//        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//        {
//            foreach (ContactPoint2D contact in collision.contacts)
//            {
//                // Если нормал указывает в сторону (столкновение сбоку)
//                if (Mathf.Abs(contact.normal.x) > 0.7f)
//                {
//                    GameOver();
//                    return;
//                }
//            }
//        }
//    }

//    void GameOver()
//    {
//        if (!isGameRunning) return;

//        isGameRunning = false;
//        rb.velocity = Vector2.zero;

//        // Звук Game Over
//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.PlayGameOverSound();
//        }

//        // Вызов Game Over в GameManager
//        GameManager gameManager = FindObjectOfType<GameManager>();
//        if (gameManager != null)
//        {
//            gameManager.GameOver();
//        }
//    }

//    void OnDrawGizmosSelected()
//    {
//        if (groundCheck != null)
//        {
//            Gizmos.color = isGrounded ? Color.green : Color.red;
//            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
//        }
//    }
//}
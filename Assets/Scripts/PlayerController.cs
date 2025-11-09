using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 16f;
    public float runSpeed = 5f;
    public float jumpTime = 0.3f; // Время удержания прыжка

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isGameRunning = true;
    private bool isJumping = false;
    private float jumpTimeCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isGameRunning) return;

        CheckGrounded();

        // Начало прыжка
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            Jump();
        }

        // Удержание прыжка
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                Jump();
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // Конец прыжка
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        if (!isGameRunning) return;

        rb.velocity = new Vector2(runSpeed, rb.velocity.y);
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Сбрасываем прыжок при приземлении
        if (isGrounded)
        {
            isJumping = false;
        }
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
        }
    }

    public void StopRunning()
    {
        isGameRunning = false;
        rb.velocity = Vector2.zero;
    }
}

//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float jumpForce = 10f;
//    public float runSpeed = 5f; // Новая переменная для скорости бега

//    [Header("Ground Check")]
//    public Transform groundCheck;
//    public float checkRadius = 0.2f;
//    public LayerMask groundLayer;

//    private Rigidbody2D rb;
//    private bool isGrounded;
//    private bool isGameRunning = true; // Контроль игры

//    void Start()
//    {
//        // Получаем компонент Rigidbody2D
//        rb = GetComponent<Rigidbody2D>();
//    }

//    void Update()
//    {
//        if (!isGameRunning) return;

//        // Проверяем, стоит ли игрок на земле
//        CheckGrounded();

//        // Обрабатываем прыжок при нажатии пробела или клике мыши
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
//        // Проверяем, касается ли игрок земли
//        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
//    }

//    void Jump()
//    {
//        // Применяем силу для прыжка
//        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//    }

//    // Метод для остановки игрока (при столкновении с препятствием)
//    public void StopRunning()
//    {
//        isGameRunning = false;
//        rb.velocity = Vector2.zero;
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        // Проверяем столкновение с препятствием
//        if (collision.gameObject.CompareTag("Obstacle"))
//        {
//            StopRunning();
//            Debug.Log("Game Over! Hit obstacle");
//        }
//    }
//}

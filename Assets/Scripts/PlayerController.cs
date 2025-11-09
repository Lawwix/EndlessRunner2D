using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        // Получаем компонент Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Проверяем, стоит ли игрок на земле
        CheckGrounded();

        // Обрабатываем прыжок при нажатии пробела или клике мыши
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
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
}

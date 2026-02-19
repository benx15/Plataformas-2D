using UnityEngine;

public class Jugador : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.4f;
    public LayerMask groundLayer;

    public Transform spawnPoint;   //  AÑADIDO

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    //  AÑADIDO
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FallZone") || other.CompareTag("Pincho"))
        {
            rb.velocity = Vector2.zero;                 // evita bugs raros
            transform.position = spawnPoint.position;   // respawn
        }
    }
}

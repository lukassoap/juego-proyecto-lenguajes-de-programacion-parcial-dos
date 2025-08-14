using UnityEngine;

public class PlatformEnemy : MonoBehaviour
{
    // La velocidad del enemigo
    public float moveSpeed = 2f; 

    // El punto de control para detectar el borde de la plataforma
    public Transform groundDetection; 

    // El tag que identifica la plataforma
    public string platformTag = "plataforma"; 

    // La dirección actual del movimiento (true = derecha, false = izquierda)
    private bool movingRight = true; 

    // Referencia al componente Rigidbody2D del enemigo
    private Rigidbody2D rb;

    void Start()
    {
        // Obtiene la referencia al Rigidbody2D en el objeto actual
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Mueve al enemigo en la dirección actual
        Move();

        // Detecta si hay un borde o un vacío
        DetectEdge();
    }

    private void Move()
    {
        // Define la velocidad en X
        float xVelocity = movingRight ? moveSpeed : -moveSpeed;

        // Aplica la velocidad al Rigidbody2D
        rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
    }

    private void DetectEdge()
    {
        // Lanza un rayo hacia abajo desde el punto de detección.
        // La longitud del rayo debe ser suficiente para detectar la plataforma.
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);

        // Si el rayo no golpea nada o el objeto que golpea no tiene el tag "plataforma"...
        if (groundInfo.collider == null || !groundInfo.collider.CompareTag(platformTag))
        {
            // Cambia la dirección del enemigo
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        // Invierte la dirección
        movingRight = !movingRight;

        // Voltea el sprite del enemigo para que mire en la nueva dirección
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
}
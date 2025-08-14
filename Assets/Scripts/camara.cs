using UnityEngine;

public class camara: MonoBehaviour
{
    // La variable que almacenará la referencia al jugador
    public Transform player; 

    // Ajusta la velocidad de seguimiento. Un valor más alto significa que la cámara sigue al jugador más rápidamente.
    public float smoothSpeed = 0.125f; 

    // Almacena el desplazamiento inicial entre la cámara y el jugador
    private Vector3 offset; 

    // Start se llama antes del primer frame
    void Start()
    {
        // Calcula el desplazamiento inicial basándose en las posiciones iniciales de la cámara y el jugador
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    // LateUpdate se llama después de que todos los objetos han sido actualizados
    void LateUpdate()
    {
        // Asegúrate de que la referencia al jugador no es nula
        if (player != null)
        {
            // Calcula la nueva posición de la cámara
            Vector3 desiredPosition = player.position + offset;
            
            // Usa Vector3.Lerp para suavizar el movimiento de la cámara
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // Asigna la nueva posición suavizada a la cámara
            transform.position = smoothedPosition;
        }
    }
}
using UnityEngine;

public class Collectible : MonoBehaviour
{
      public GameManager myGameManager;
      
      void Start()
    {
        myGameManager = FindObjectOfType<GameManager>();
    }

    // Este método se activa cuando un Collider2D entra en el Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entró en el trigger tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Busca el componente de puntuación en el jugador
            myGameManager.AddScore();
            Destroy(gameObject);
        }
    }
}
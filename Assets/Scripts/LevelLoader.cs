using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // El nombre de la siguiente escena a la que quieres ir
    public string nextSceneName;

    // Este método se llama cuando un Collider2D entra en el Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Puedes usar el tag del jugador para asegurarte de que solo él active el cambio
        if (other.CompareTag("Player"))
        {
            // aqui sonido
            SoundEffectManager.Play("Victory"); // deberia funcionar 
            // Carga la siguiente escena usando el nombre que especificaste
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
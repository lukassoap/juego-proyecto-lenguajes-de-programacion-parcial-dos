using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // El prefab del objeto que puede soltar el enemigo
    public GameObject itemToDrop; 

    // La probabilidad (en porcentaje) de soltar el objeto
    // 10f significa 10%
    public float dropChance = 10f; 

    // Función que se llama cuando el enemigo muere
    public void Die()

    {

        // aqui llamo sonido

        SoundEffectManager.Play("EnemyDeath"); // deberia funcionar 
        // Llamada a la función que maneja el "loot"
        DropItem();

        // Destruye el objeto del enemigo
        Destroy(gameObject);
    }

    private void DropItem()
    {
        // Genera un número aleatorio entre 0 y 100
        float randomNumber = Random.Range(0f, 100f);

        // Si el número aleatorio es menor o igual a la probabilidad de soltar...
        if (randomNumber <= dropChance)
        {
            // Genera una instancia del objeto en la posición del enemigo
            if (itemToDrop != null)
            {
                Instantiate(itemToDrop, transform.position, Quaternion.identity);
            }
        }
    }
}
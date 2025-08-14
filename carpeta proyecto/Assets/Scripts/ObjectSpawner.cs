using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // El objeto que queremos generar (arrastra el prefab aquí en el Inspector)
    public GameObject objectToSpawn; 

    // Referencia al transform del jugador
    public Transform playerTransform; 

    // Distancia a la que el objeto debe aparecer
    public float spawnDistance = 10f; 

    // Bandera para asegurarnos de que el objeto solo se genere una vez
    private bool hasSpawned = false; 

    void Update()
    {
        // Verifica si el objeto ya ha sido generado
        if (!hasSpawned)
        {
            // Calcula la distancia entre el generador y el jugador
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            // Si la distancia es menor o igual a la distancia de generación...
            if (distance <= spawnDistance)
            {
                // Genera el objeto en la posición del generador
                Instantiate(objectToSpawn, transform.position, Quaternion.identity);

                // Marca la bandera como verdadera para que no se genere de nuevo
                hasSpawned = true; 
            }
        }
    }
}
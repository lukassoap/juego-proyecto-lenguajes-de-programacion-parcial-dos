using UnityEngine;

public class GarbadeCollector : MonoBehaviour
{
    // Esta función es llamada automáticamente por Unity cuando otro
    // Collider entra en el trigger de este obje
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que entró tiene la etiqueta "ItemGood" O "Enemy".
        // CompareTag es más eficiente que usar other.gameObject.tag == "tag".
        if (collision.CompareTag("Enemy"))
        {
            // Si tiene alguna de esas etiquetas, destruimos el GameObject completo
            // al que pertenece el collider que entró.
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("ItemGood"))
        {
            Destroy(collision.gameObject);
        }
    }
}
using UnityEngine;

public class GarbadeCollector : MonoBehaviour
{
    // Esta funci�n es llamada autom�ticamente por Unity cuando otro
    // Collider entra en el trigger de este obje
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que entr� tiene la etiqueta "ItemGood" O "Enemy".
        // CompareTag es m�s eficiente que usar other.gameObject.tag == "tag".
        if (collision.CompareTag("Enemy"))
        {
            // Si tiene alguna de esas etiquetas, destruimos el GameObject completo
            // al que pertenece el collider que entr�.
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("ItemGood"))
        {
            Destroy(collision.gameObject);
        }
    }
}
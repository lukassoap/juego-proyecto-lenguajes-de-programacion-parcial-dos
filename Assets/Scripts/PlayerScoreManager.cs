using UnityEngine;
using TMPro; // Asegúrate de tener este 'using' si usas TextMeshPro

public class myplayerScoreManager : MonoBehaviour
{
public int score;
    public TextMeshProUGUI textScore; // ¡Cambio aquí!
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AddScore()
    {
        score++;
        textScore.text = "Puntos: " + score.ToString();
    }
}
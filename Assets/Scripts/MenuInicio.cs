using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string nextSceneName;
    public void Jogar()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    // Update is called once per frame
    public void Salir()
    {
        Application.Quit();
    }
}

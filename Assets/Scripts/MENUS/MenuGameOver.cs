using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    public string Menu;
    private int menuSceneIndex; 

    void Start()
    {
        menuSceneIndex = SceneManager.GetSceneByName(Menu).buildIndex;
        Invoke("CambiarEscena", 6f);
    }

    void CambiarEscena()
    {
        SceneManager.LoadScene(Menu);
    }

    public void Salir()
    {
        Application.Quit();
    }
}

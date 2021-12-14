using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Debug.Log("Saliendo");
        Application.Quit();
    }

    public void LevelSelect(int _n)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + _n);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    float changeSceneDelay = 0.7f;

    public void Play()
    {
        StartCoroutine(ChangeSceneCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void Exit()
    {
        Debug.Log("Saliendo");
        StartCoroutine(QuitGameCoroutine());
    }

    public void LevelSelect(int _n)
    {
        StartCoroutine(ChangeSceneCoroutine(SceneManager.GetActiveScene().buildIndex + _n));
    }

    public void Retry()
    {
        StartCoroutine(ChangeSceneCoroutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void GoLevelSelector()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(ChangeSceneCoroutine("Level_Selector"));
    }

    public void GoBack()
    {
        StartCoroutine(ChangeSceneCoroutine(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void ShowInGameMenu(GameObject _inGameMenu)
    {
        _inGameMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void CloseInGameMenu(GameObject _inGameMenu)
    {
        Time.timeScale = 1.0f;
        _inGameMenu.SetActive(false);
    }


    IEnumerator ChangeSceneCoroutine(int _sceneIdx)
    {
        yield return new WaitForSeconds(changeSceneDelay);
        SceneManager.LoadScene(_sceneIdx);
    }
    IEnumerator ChangeSceneCoroutine(string _sceneName)
    {
        yield return new WaitForSeconds(changeSceneDelay);
        SceneManager.LoadScene(_sceneName);
    }
    IEnumerator QuitGameCoroutine()
    {
        yield return new WaitForSeconds(changeSceneDelay);
        Application.Quit();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtns : MonoBehaviour
{
    public GameObject loadingScreen;
    public void Play()
    {
        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<Animator>().SetTrigger("Triggered");
        StartCoroutine(PlayGame());
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
    }
}

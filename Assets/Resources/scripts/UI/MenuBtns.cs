using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtns : MonoBehaviour
{
    public GameObject panel;
    public void Play()
    {
        panel.GetComponent<Animator>().SetTrigger("Triggered");
        StartCoroutine(PlayGame());
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}

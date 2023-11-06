using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            LoadScene("End");
        }
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(sceneName);
    }
    public void doExitGame()
    {
        Application.Quit();
    }
}

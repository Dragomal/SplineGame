using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private Transform _startCoos;
    public void Die(){
        //Désactive les mouvements du joueur et le freeze
        PlayerMovements playerMovements = GetComponent<PlayerMovements>();
        playerMovements.enabled = false;
        
        Rigidbody2D playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerRigidbody2D.velocity = Vector2.zero;
        playerRigidbody2D.gravityScale = 0;
        LoadScene("Menu");
    }
    public void Reset(){
        //Réactive les mouvements du joueur et l'unfreeze
        PlayerMovements playerMovements = GetComponent<PlayerMovements>();
        playerMovements.enabled = true;

        Rigidbody2D playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerRigidbody2D.gravityScale = 1;

        transform.position = _startCoos.position;
    }
    public void LoadScene(string sceneName){
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public IEnumerator LoadSceneCoroutine(string sceneName){
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(sceneName);
    }
}

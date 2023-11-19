using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private Transform _startCoos;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _cameraOnDeath;
    private Rigidbody2D _rb2D;
    private CinemachineImpulseSource _impulseSource;
    public SpriteRenderer _playerSpriteRenderer, _jetpackSpriteRenderer;
    void Start(){
        _rb2D = GetComponent<Rigidbody2D>();
        _playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _impulseSource = GetComponentInChildren<CinemachineImpulseSource>();
    }
    public void Die(){
        //Désactive les mouvements du joueur et le freeze
        PlayerMovements playerMovements = GetComponent<PlayerMovements>();
        playerMovements.enabled = false;
        
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        FollowSpline cameraFollow = camera.GetComponent<FollowSpline>();
        cameraFollow.enabled = false;
        _cameraOnDeath.SetActive(true);
        
        CameraShakeManager.instance.CameraShake(_impulseSource);

        _playerSpriteRenderer.enabled = false;
        _jetpackSpriteRenderer.enabled = false;
        Instantiate(_particleSystem, transform.position, Quaternion.identity);

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
    public void ChangeJetpack(bool isJetpack){
        if(isJetpack){
            _jetpackSpriteRenderer.color = new Color32(197, 219, 90, 86);
        }
        else{
            _jetpackSpriteRenderer.color = new Color32(219, 102, 90, 86);
        }
    }
    void Update(){
        if(_rb2D.velocity.x != 0){
            if(_rb2D.velocity.x < 0){
                transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else{
                transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }
    }
    public void LoadScene(string sceneName){
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public IEnumerator LoadSceneCoroutine(string sceneName){
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(sceneName);
    }
}

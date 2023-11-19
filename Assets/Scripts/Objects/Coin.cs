using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class Coin : MonoBehaviour
{
    [SerializeField] private float _range = 1;
    [SerializeField] private GameObject particles;
    private float _xPos;
    private float _yPos;



    void Awake(){
        _xPos = transform.position.x;
        _yPos = transform.position.y;
    }
    void Update(){
        //Mouvement de haut en bas de la pièce
        transform.localPosition = new Vector3(_xPos, _yPos + _range * Mathf.Sin(Time.time * Mathf.PI), 0);
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            GameObject player = other.gameObject;
            StartCoroutine(DestroyCoin(player));       
        }
    }
    IEnumerator DestroyCoin(GameObject player){

        //Désactive les visuels et Hitbox de la pièce
        ParticleSystem coinParticles = GetComponent<ParticleSystem>();
        coinParticles.Stop();
        SpriteRenderer coinRenderer = GetComponent<SpriteRenderer>();
        coinRenderer.enabled = false;
        CircleCollider2D hitboxCircle = GetComponent<CircleCollider2D>();
        hitboxCircle.enabled = false;

        //Ajoute 1 au compteur UI
        CoinManager playerCoinManager= player.GetComponent<CoinManager>();
        Debug.Log( playerCoinManager);
        playerCoinManager.AddCoin();


        //Effets de pickup (Particules / Son)
        Instantiate(particles, transform.position, Quaternion.identity);

        //Attend 1 seconde
        yield return new WaitForSeconds(1.25f);

        //Destroy la pièce
        Destroy(this.gameObject);
    }
}

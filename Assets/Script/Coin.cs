using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _range = 1;
    private float _xPos;
    private float _yPos;
    void Awake(){
        _xPos = transform.position.x;
        _yPos = transform.position.y;
    }
    void Update(){
        //Mouvement de haut en bas de la pièce
        transform.localPosition = new Vector3(_xPos, _yPos + _range * Mathf.Sin(Time.time * Mathf.PI), 0);

        //Particule brillant pour les pièces ?

    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            //Désactive les visuels de la pièce
            ParticleSystem coinParticles = GetComponent<ParticleSystem>();
            coinParticles.Stop();
            SpriteRenderer coinRenderer = GetComponent<SpriteRenderer>();
            coinRenderer.enabled = false;

            //Ajoute 1 au compteur UI

            //Effets de pickup (Particules / Son)

            //Destroy la pièce
        }
    }
}

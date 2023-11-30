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
            CoinManager playerCoinManager= other.gameObject.GetComponent<CoinManager>();
            playerCoinManager.AddCoin();
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);   
        } 
    }
}

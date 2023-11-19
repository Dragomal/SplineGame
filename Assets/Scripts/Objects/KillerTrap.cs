using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerTrap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            PlayerState playerState = other.GetComponent<PlayerState>();
            playerState.Die();
        }
    }
}

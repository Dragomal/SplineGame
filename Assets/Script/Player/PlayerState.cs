using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public void Reset(){
        //Réactive les mouvements du joueur et l'unfreeze
        PlayerMovements playerMovements = GetComponent<PlayerMovements>();
        playerMovements.enabled = true;

        Rigidbody2D playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerRigidbody2D.gravityScale = 1;

        transform.position = _startCoos.position;
    }
}

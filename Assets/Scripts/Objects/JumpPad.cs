using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private bool isUsed;
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OnCollisionEnter2D(Collision2D collision){
        PlayerMovements playerMovements = collision.gameObject.GetComponent<PlayerMovements>();
        if(collision.gameObject.CompareTag("Player")){
            if(playerMovements._CanDoubleJump || isUsed) return;
            playerMovements._CanDoubleJump = true;

            spriteRenderer.color = Color.white;
            isUsed = true;
        }
    }
}

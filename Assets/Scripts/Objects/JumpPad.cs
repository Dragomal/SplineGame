using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    private SpriteRenderer spriteRenderer;
    private bool isUsed;
    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D collision){
        PlayerMovements playerMovements = collision.gameObject.GetComponent<PlayerMovements>();
        if(collision.gameObject.CompareTag("Player")){
            if(playerMovements._CanDoubleJump || isUsed) return;
            playerMovements._CanDoubleJump = true;

            spriteRenderer.color = Color.white;
            isUsed = true;

            Instantiate(particles, transform.position, Quaternion.identity, collision.gameObject.transform);

            Destroy(this.gameObject);
        }
    }
}

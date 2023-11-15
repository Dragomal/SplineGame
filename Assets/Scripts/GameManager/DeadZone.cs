using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            //Désactive le suivi de la spline
            FollowSpline cameraFollow = GetComponentInParent<FollowSpline>();
            cameraFollow.enabled = false;

            //Désactive le player
            PlayerState playerState = other.GetComponent<PlayerState>();
            playerState.Die();

            //Effets de mort (ScreenShake / Explosion / Son)

            //Affiche Overlay de mort
        }
    }
}

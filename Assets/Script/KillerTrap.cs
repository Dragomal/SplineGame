using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerTrap : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            //Cherche la caméra et désactive le suivi de la spline
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            FollowSpline cameraFollow = camera.GetComponent<FollowSpline>();
            cameraFollow.enabled = false;

            //Désactive le player
            PlayerState playerState = other.GetComponent<PlayerState>();
            playerState.Die();

            //Effets de mort (ScreenShake / Explosion / Son)

            //Affiche Overlay de mort
        }
    }
}

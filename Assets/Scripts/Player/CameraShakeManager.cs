using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    void Awake(){
        if(instance == null) instance = this;
    }
    public void CameraShake(CinemachineImpulseSource impulseSource){
        // impulseSource.Do
    }
}

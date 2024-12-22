using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
 

public class DeactivateCameraIfNotMain : MonoBehaviourPunCallbacks
{
    
    void Start()
    {
        if (!photonView.IsMine){
            GetComponent<Camera>().gameObject.SetActive(false);
        }
    }

    public override void OnEnable(){
        if (!photonView.IsMine){
            GetComponent<Camera>().gameObject.SetActive(false);
        }

    }

    //this code works for allowing the camera to be disabled in case youre not the main player, in order to have every player has his camera
        
    
}

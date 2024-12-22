using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DeactivateIfNotMain : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject objectToDeactivate;
    void Update()
    {
        if (PhotonNetwork.IsMasterClient){
            objectToDeactivate.SetActive(false);
        }
        else{
            objectToDeactivate.SetActive(true);
        }
    }
}

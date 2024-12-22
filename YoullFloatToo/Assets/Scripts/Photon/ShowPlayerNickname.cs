using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;
using NUnit.Framework.Internal;

public class ShowPlayerNickname : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI playerText;
    string playerName = "Test";
    private void Start() {

        
        string playerName = PlayerNecesaryDataHolder.instance.GetPlayerName();
        
        PhotonNetwork.NickName = playerName;
        Debug.Log (PhotonNetwork.NickName);

        if (photonView.IsMine){
            playerText.text = PhotonNetwork.NickName;
        }
        
    }

    //It creates and moddifies a small text under the player showing who it is in case that the PlayerNeccesaryDataHolder is avalible

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI text;

    private Player _player;



    public void SetPlayerInfo (Player player){
        _player = player;
        //text.text = PlayerNecesaryDataHolder.instance.GetPlayerName();
    }

    public Player GetPlayer(){
        return _player;
    }

    //This player fills the information of the players in the game to player listingMenu
}

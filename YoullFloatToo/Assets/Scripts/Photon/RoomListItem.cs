using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    RoomInfo info;

    public void SetUp(RoomInfo _info){
        info = _info;
        text.text = _info.Name;
    }

    public void OnClick(){
        RoomListingMenu.Instance.JoinRoom(info);
        Debug.Log("Joining room" + text);
    }

    //when you click on the room button, you join the text attach to it
}

using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListing : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public RoomInfo RoomInfo { get; private set; }

    public void SetRoomInfo (RoomInfo roomInfo){
        RoomInfo = roomInfo;

        text.text = roomInfo.Name;
    }

    //This code gets every room created and addapts the text mesh pro from a botton to fill its method, so when you click on the button you
    //will  join that room;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using NaughtyAttributes;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{

    public static RoomListingMenu Instance;
    [SerializeField] RoomListItem roomListingPrefab;
    [SerializeField] Transform content;
    private void Awake() {
        Instance = this;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in content){
            Destroy(trans.gameObject);
        }
        

        for (int i = 0; i < roomList.Count; i++){
                    Instantiate(roomListingPrefab, content).GetComponent<RoomListItem>().SetUp(roomList[i]);
                
            }
    }

    public void JoinRoom(RoomInfo info){
        PhotonNetwork.JoinRoom(info.Name);
        Debug.Log("Joining room " + info.Name);
    }

    //This code will create the different buttons with their information and allows you to join the related to the text they have
}

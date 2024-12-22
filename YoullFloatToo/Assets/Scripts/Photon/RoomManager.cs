using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set;}
   private int maxNumberOfPlayer = 3;

   public string roomNameToJoin = "test";

   [SerializeField] GameObject multiplayerUI;

   public Action onJoinedRoom;

   private void Awake() {
    Instance = this;
   }
    
    public void JoinButtonPressed(){
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxNumberOfPlayer;

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin,roomOptions, null);
        onJoinedRoom?.Invoke();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
         
         Debug.Log("Were connected and in a room");
         RoomList.Instance.UpdateUI() ;
         multiplayerUI.SetActive(true);
         
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Left the room. Returning to lobby...");
        PhotonNetwork.JoinLobby(); 
        multiplayerUI.SetActive(false);
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public void GoNextSceneMultiplayer()
{
    if (PhotonNetwork.IsMasterClient)
    {
        
        photonView.RPC("LoadSceneForAllPlayers", RpcTarget.All);
        
        
    }
}

[PunRPC]
private void LoadSceneForAllPlayers()
{
    if (PlayerPrefs.GetInt("MultiplayerMode")== 1){
            SceneManager.LoadScene("RaceMode");
        } 
    
        if (PlayerPrefs.GetInt("MultiplayerMode")== 0){
            SceneManager.LoadScene("LastOneStanding");
        } 
    
}


    public void GoMainMenu(){
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(1);
    }
}

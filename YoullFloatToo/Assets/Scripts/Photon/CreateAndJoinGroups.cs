using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class CreateAndJoinGroups : MonoBehaviourPunCallbacks
{

    private const string LASTONE_STANDING_SCENE = "LastOneStanding"; // Scene name for the lobby for multiplayer
    private const string RACEMODE_SCENE = "RaceMode"; // Scene name for the lobby for multiplayer
    public TMP_InputField createInput;

    private int maxNumberOfPlayer = 3;
    private int goLastStandingIndex;

    [SerializeField] bool goLastOneStanding;

    IEnumerator Start()
    {
        goLastStandingIndex = PlayerPrefs.GetInt("MultiplayerMode");

        //Checks if the player is connected to the multiplayer server, if it isnt, it connects

        if (goLastStandingIndex == 0){
            goLastOneStanding = true;
        }

        if (goLastStandingIndex == 1){
            goLastOneStanding = false;
        }
        

        if (PhotonNetwork.InRoom){
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {

        //Once it is connected to the server, the game joins the Lobby
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom()
    {

        //Once the player it is connected to the Lobby it has different options
        //This one is to create a room to play in multiplayer, with a room configuration
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxNumberOfPlayer;
        PhotonNetwork.CreateRoom(createInput.text,roomOptions);
    }

    public void JoinRandomRoom(){
        //Once the player it is connected to the Lobby it has different options
        //In case the player just wants to play directly it connects to a random room or creates a new one  with a room configuration
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxNumberOfPlayer;
        PhotonNetwork.JoinRandomOrCreateRoom(null,0,MatchmakingMode.FillRoom, null, null, null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        //Once it knoews if it joins or creates a room, it will check if the bools "goLastOneStanding" is true or false, allowing it to go to a particular GameMode

        if (goLastOneStanding == true){
            PhotonNetwork.LoadLevel(LASTONE_STANDING_SCENE);
        }
        else{
            PhotonNetwork.LoadLevel(RACEMODE_SCENE);
        }
        
    }

    

    //This script is related to the different buttons in the LOBBY_SCENE,  allowing it to connect to different scenes, to be correctly connected
    //Or to be disconnected from the multiplayer server if needed, it is applied to the different buttons, and setting the room Configuration


}

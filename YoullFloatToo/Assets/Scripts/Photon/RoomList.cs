using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance;

    public GameObject roomManagerGameObject;
    [SerializeField] Transform roomListParent;
    [SerializeField] GameObject roomListItemPrefab;

    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] GameObject connectingUI;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();


    private void Awake() {
        Instance = this;
    }
    IEnumerator Start()
    {
        stateText.text = "Connecting";
        Debug.Log("Connecting");
        connectingUI.SetActive(true);

        
        //Precautions
        if (PhotonNetwork.InRoom){
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        stateText.text = "Connected to server";

        Debug.Log("Connected to server");

        PhotonNetwork.JoinLobby();
        UpdateUI() ;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        stateText.text = "Connected to the Lobby";

        Debug.Log("Connected to the Lobby");
        connectingUI.SetActive(false);
        UpdateUI() ;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        connectingUI.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ChangeRoomToCreateName(string roomName){
        RoomManager.Instance.roomNameToJoin = roomName;
    }

    

    public override void OnRoomListUpdate (List<RoomInfo> roomInfos){

        Debug.Log("Room list updated. Number of rooms: " + roomInfos.Count);
        cachedRoomList.Clear(); // Clear the list to refresh it

        if (cachedRoomList.Count <= 0){
            cachedRoomList = roomInfos;
        }
        else{
            foreach(var room in roomInfos){
                for (int i = 0; i < cachedRoomList.Count; i++){
                    if (cachedRoomList[i].Name == room.Name){

                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList){
                            newList.Remove(newList[i]);
                        }
                        else{
                            newList[i] = room;
                        }
                        Debug.Log("Cached Room List Count: " + cachedRoomList.Count);
                        cachedRoomList = newList;
                    }
                }
            }
        }

        UpdateUI() ;
    }

    public void UpdateUI(){

        foreach(Transform roomIten in roomListParent){
            Destroy(roomIten.gameObject);


        }

        foreach(var room in cachedRoomList){
           Debug.Log("Creating button for room: " + room.Name);
           GameObject roomItem =  Instantiate(roomListItemPrefab, roomListParent);

           roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
           roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + ("/ 3");

           roomItem.GetComponent<RoomItemButton>().roomName = room.Name;
        }
    }

    public void JoinRoomByName(string name){
        RoomManager.Instance.roomNameToJoin = name;
        RoomManager.Instance.JoinButtonPressed();
    } 


}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform content;
    [SerializeField] PlayerListing playerListing;

    [SerializeField] List <PlayerListing> listings = new List <PlayerListing>();


    private void Start() {
        GetCurrentRoomPlayers();
    }

    private void GetCurrentRoomPlayers(){
        foreach(KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players){
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Player player){
        PlayerListing playerListings = Instantiate (playerListing, content);
        if (playerListings != null){
            playerListings.SetPlayerInfo (player);
            listings.Add (playerListings);
        }
        Debug.Log(listings);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = listings.FindIndex(x => x.GetPlayer() == otherPlayer);
        if (index != -1){
            Destroy(listings[index].gameObject);
            listings.RemoveAt(index);
        }
    }

    //This method adds any player to the menu as long as they get connected on it, it has some utilities related to
    //clean and emove in case anything wrong happens
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MultiplayerObstacleSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject [] obstacleList;
    private void Start() {
        StartCoroutine(ObstacleCreation());
    }

    IEnumerator ObstacleCreation()
    {
        //This method creates an obstacle in the photon way in a randome period of times
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        PhotonNetwork.Instantiate(Path.Combine("Obstacles", "Symbols", obstacleList[UnityEngine.Random.Range(0, obstacleList.Length)].name ) , 
        new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y -15, gameObject.transform.position.z)
        , Quaternion.identity);
        Debug.Log("obstacle instantiated");
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        StartCoroutine(ObstacleCreation());
    }

    //This script allows the IA from the multiplayer to start spawning obstacles in a random way every few seconds
}

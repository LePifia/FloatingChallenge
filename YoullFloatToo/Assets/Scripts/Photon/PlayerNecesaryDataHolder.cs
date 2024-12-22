using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerNecesaryDataHolder : MonoBehaviour
{
    public static PlayerNecesaryDataHolder instance;
    [SerializeField] string player;
    [SerializeField] TextMeshProUGUI playerName;

    private void Awake() {
        // Singleton pattern implementation
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {

        
            

            if (playerName == null){
                OnSpawningNewPlayer();
                Debug.Log("+ 0");
            }
            else{
                player = playerName.text;
                Debug.Log("+ 0");
            }
        
        
            
        
    }

    public void OnSpawningNewPlayer(){
        player = GetPlayerName();
        Debug.Log(player +  "+ 0");
    }

    

    public string GetPlayerName(){
        return player;
    }

    public void SetPlayerName(string playerName){
        player = playerName;
    }

    
}

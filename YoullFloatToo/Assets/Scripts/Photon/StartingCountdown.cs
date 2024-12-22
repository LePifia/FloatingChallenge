using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartingCountdown : MonoBehaviour
{

    public static StartingCountdown instance = null;
    float countdownTimer;
    [SerializeField] float countdownTimerMax;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] Button button;
    bool timerOn = true;

    private void Awake()
    {
        //Singleton pattern

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        
    }

    private void Start() {
        RoomManager.Instance.onJoinedRoom += OnTheEventPlayed; 
        countdownTimer = countdownTimerMax; 
    }

    private void OnTheEventPlayed()
    {
        countdownTimer = countdownTimerMax;
    }

    private void Update() {
        if (timerOn){
            // Reduce the countdown timer each frame by the time elapsed
            countdownTimer -= Time.deltaTime;
            
            
            if (countdownTimer <= 0) {
                countdownTimer = 0;
                timerOn = false;  // Stop the countdown

                // Trigger the countdown end event
                button.onClick.Invoke();
            }

            
            UpdateCountdownText();
        }
    }

    private void UpdateCountdownText() {
        countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
    }

    void OnDestroy(){
        RoomManager.Instance.onJoinedRoom -= OnTheEventPlayed;  
    }
    
}

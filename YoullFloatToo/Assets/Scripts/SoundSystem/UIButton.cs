using System.Collections;
using System.Collections.Generic;
//using CandyCoded.HapticFeedback;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    public string clickSound = "click";
    public bool useVibration = true;

    [SerializeField] private bool varietyButton = true;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    void Click()
    {
        if (varietyButton == false){
                SoundManager.instance.PlaySound(clickSound);
            if (useVibration)
            {
                /*if (SoundManager.instance.IsHapticsOff() == false) 
                { HapticFeedback.LightFeedback(); }*/
            }
        }
        else{
            SoundManager.instance.PlayRandomSound();
            if (useVibration){
                /*if (SoundManager.instance.IsHapticsOff() == false) 
                { HapticFeedback.LightFeedback(); }*/
            }
        }
        
    }
}

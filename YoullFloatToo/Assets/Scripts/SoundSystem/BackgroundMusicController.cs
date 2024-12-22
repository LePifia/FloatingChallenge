using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public static BackgroundMusicController instance;
    SoundManager soundManager;
    public TMP_Text currentTuneTxt;
    [Space]
    public GameObject bgMusicButton;
    public Transform musicBtnsContainer;
    private List<BGMusicItem> bgMusicItems = new List<BGMusicItem>();

    private void Awake() {
        instance = this;
    }

    private void Start() {
        soundManager = SoundManager.instance;
        GenerateBGMusicBtns();
        UpdateCurrentTuneText();
    }

    public string GetCurrentTune()
    {
        string defaultTune = PlayerPrefs.GetString("defaultBGMusic");
        return defaultTune;
    }

    public void GenerateBGMusicBtns()
    {
        // if we haven't preloaded buttons yet
        if (bgMusicItems.Count < 1)
        {
            for (int i = 0; i < soundManager.BGTunes.Count; i++)
            {
                if (soundManager.BGTunes[i].isTune)
                {
                    //create a new button
                    GameObject musicItem = Instantiate(bgMusicButton, musicBtnsContainer);
                    
                    BGMusicItem itemRef;
                    if (musicItem != null)
                    {
                        //get component from button
                        itemRef = musicItem.GetComponent<BGMusicItem>();
                        //let's set appropriate items
                        itemRef.musicName.text = soundManager.BGTunes[i].ClipName;

                        if (itemRef.musicName.text == GetCurrentTune())
                        {
                            itemRef.tickIcon.SetActive(true);
                        }else{
                            itemRef.tickIcon.SetActive(false);
                        }

                        bgMusicItems.Add(itemRef);
                    }
                }
            }
        }
    }

    void UpdateCurrentTuneText()
    {
        currentTuneTxt.text = $"Tunes - {GetCurrentTune()}";
    }

    void UpdateMusicItems()
    {
        foreach(var item in bgMusicItems)
        {
            if (item.musicName.text != GetCurrentTune())
            {
                item.tickIcon.SetActive(false);
            }else{
                item.tickIcon.SetActive(true);
            }
        }
    }

    public void ChangeDefaultBGMusic(string _newMusic)
    {
        soundManager.ChangeDefaultBGMusic(_newMusic, false);
        UpdateMusicItems();
        UpdateCurrentTuneText();
    }
}

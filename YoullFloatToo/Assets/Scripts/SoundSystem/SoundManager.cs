using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    private static string MASTER_AUDIO = "Master";

    // [SerializeField]
    public List<Sound> BGTunes;
    public Sound[] sounds;
    public BeatManager beatManager;
    [SerializeField] AudioMixer globalAudioMixer;
    
    [HideInInspector]
    private Sound overlapMusic;
    public Sound currentBG;
    
    private bool _muteBackgroundMusic = false;
    private bool _muteSoundFx = false;  
    private bool _hapticStatus = false;  
    float defaultBGVol = 0;
    bool hasReducedBG = false;
    bool _isPlayingDefaultBG = false;

    private void Awake ()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            
        }
        else
        { Destroy(this); }
    }

    
    void Start()
    {
        InitializeSounds();
        SetDefaultBGMusic(PlayerPrefs.GetString("defaultBGMusic"));
        PlayDefualtBG();
        GetCurBgAudioSource();                   
    }

    void InitializeSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].ClipName);
            _go.transform.SetParent(transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
            sounds[i].GetAudioSource().outputAudioMixerGroup = globalAudioMixer.FindMatchingGroups(MASTER_AUDIO)[2];
            
        }
        for (int i = 0; i < BGTunes.Count; i++)
        {
            GameObject _go = new GameObject("bgSound_" + i + "_" + BGTunes[i].ClipName);
            _go.transform.SetParent(transform);
            BGTunes[i].SetSource(_go.AddComponent<AudioSource>());
            BGTunes[i].GetAudioSource().outputAudioMixerGroup = globalAudioMixer.FindMatchingGroups(MASTER_AUDIO)[1];
        }
    }

    public void PlayDefualtBG(bool isLoud = true){
        string defaultTune = PlayerPrefs.GetString("defaultBGMusic");
        ChangeDefaultBGMusic(defaultTune, true);
        SetDefaultBGStatus(true);
        ToggleLoudness(isLoud);
    }

    public static void SetDefaultBGStatus(bool hasStartedPlaying)
    {
        instance._isPlayingDefaultBG = hasStartedPlaying;
        if(hasStartedPlaying)
        {
            instance.StopSound("loading");
        }
    }

    void GetCurBgAudioSource()
    {
        beatManager._audioSource = currentBG.GetAudioSource();
        beatManager._bpm = currentBG.bpm;
    }

    public void SetDefaultBGMusic(string ClipName)
    {
        string defaultTune = PlayerPrefs.GetString("defaultBGMusic");
        if (ClipName == "" && defaultTune == "")
        {
            PlayerPrefs.SetString("defaultBGMusic", BGTunes[0].ClipName);
            ClipName = PlayerPrefs.GetString("defaultBGMusic");            
        }else if (ClipName == "" && defaultTune != "")
        {
            ClipName = PlayerPrefs.GetString("defaultBGMusic");
        }

        foreach(var sound in BGTunes)
        {
            if (sound.isBGSound)
            {
                sound.isDefaultBG = false; //reset all BG sounds
                if (sound.ClipName == ClipName) 
                {
                    sound.isDefaultBG = true;
                    currentBG = sound;
                }
            }
        }
    }


    public void ChangeDefaultBGMusic(string ClipName, bool overlapBossTheme)
    {
        foreach(var sound in BGTunes)
        {
            if (sound.isBGSound)
            {
                sound.isDefaultBG = false; //reset all BG sounds
                if (sound.ClipName == ClipName) 
                {
                    sound.isDefaultBG = true;
                    //hard coding to check if we aren't currently playing the boss theme
                    if (currentBG.ClipName != "bossTheme" || overlapBossTheme)
                    {
                        currentBG = sound;
                    }

                    if (IsBackgroundMusicMuted() == false && sound.isPlaying() == false)
                    {
                        //hard coding to check if we aren't currently playing the boss theme
                        if (currentBG.ClipName != "bossTheme" || overlapBossTheme)
                        {
                            sound.Play(); 
                        }
                    }
                    PlayerPrefs.SetString("defaultBGMusic", ClipName);
                    GetCurBgAudioSource();              
                }
                else{
                    sound.Stop();
                }
            }

        }
    }

    public static bool isPlayingDefaultBG()
    {
        return instance._isPlayingDefaultBG;
    }

    public void StopDefaultBG(){
        foreach(var sound in BGTunes)
        {
            if (sound.isBGSound && sound.isPlaying() && sound.isDefaultBG)
            {
                sound.Stop();
            }
        }
    }

    public void PlaySound(string _name)
    {
        if (!IsSoundFXMuted())
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                if (sounds[i].ClipName == _name)
                {
                    // Debug.Log("Playing " + _name);
                    sounds[i].Play();
                    return;
                }
            }

            // no sound with _name
            Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
        }
    }

    public void PlayRandomSound(){
        if (!IsSoundFXMuted())
        {
            int randomIndex = Random.Range(0, sounds.Length);
                
                    // Debug.Log("Playing " + _name);
                    sounds[randomIndex].Play();
                    return;
            
        }
    }

    public void SetCurrentBG(string ClipName)
    {
        foreach(var sound in BGTunes)
        {
            if (sound.isBGSound)
            {
                if (sound.ClipName == ClipName)
                {
                    currentBG = sound;
                    if (IsBackgroundMusicMuted() == false && sound.isPlaying() == false)
                    {
                        sound.Play();                      
                        // Debug.LogError("playing default bg");
                    }
                    GetCurBgAudioSource();
                }
                else{
                    sound.Stop();
                }
            }

        }
    }

    public void OverlapAndDistortCurrentBG(string ClipName)
    {
        if (IsBackgroundMusicMuted()) { return; }
        foreach(var sound in BGTunes)
        {
            if (sound.ClipName == ClipName)
            {
                if (sound.isBGSound && !sound.isDefaultBG)
                {
                    overlapMusic = sound;
                    overlapMusic.Play();
                    DistortDefaultBG();
                }
            }
        }
    }

    public void CheckDefaultBGVolume()
    {
        if (!IsBackgroundMusicMuted())
        {
            foreach(var sound in BGTunes)
            {
                if (sound.isDefaultBG && hasReducedBG)
                {
                    Debug.LogError("inceasing background");
                    IncreaseBG();
                }
            }
        }
    }

    public void DistortDefaultBG()
    {
        foreach(var sound in BGTunes)
        {
            if (sound.isDefaultBG)
            {
                sound.SetPitch(0.9f);
            }
        }
    }

    public void RemoveDistortionBG()
    {
        foreach(var sound in BGTunes)
        {
            if (sound.isDefaultBG && sound.isBGSound)
            {
                sound.SetPitch(1);
            }
            else if(overlapMusic != null)
            {
                overlapMusic.Stop();
            }
        }    
    }


    public void ReduceBG()
    {
        defaultBGVol = currentBG.volume;
        currentBG.SetVolume(0f);
        hasReducedBG = true;
    }


    public void IncreaseBG()
    {
        if (IsBackgroundMusicMuted() == false)
        {
            currentBG.SetVolume(defaultBGVol);
        }
        hasReducedBG = false;
    }


    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].ClipName == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }

    public void ToggleLoudness(bool status)
    {
        if (IsBackgroundMusicMuted()) { return; }
        foreach(var sound in BGTunes)
        {
            sound.ToggleLoudness(status);
            // sound.SetStereo(status);
        }
    }

    public void ToggleBackgroundMusic()
    {
        _muteBackgroundMusic = !_muteBackgroundMusic;
        if (_muteBackgroundMusic)
        {
            currentBG.SetVolume(0f);
        }
        else
        {
            currentBG.SetVolume(1f);
        }
    }

    public void ToggleSoundFX()
    {
        _muteSoundFx = !_muteSoundFx;
    }

    public void ToggleHaptics()
    {
        _hapticStatus = !_hapticStatus;
    }

    public bool IsBackgroundMusicMuted()
    {
        return _muteBackgroundMusic;
    }

    public bool IsSoundFXMuted()
    {
        return _muteSoundFx;
    }

    public bool IsHapticsOff()
    {
        return _hapticStatus;
    }
}

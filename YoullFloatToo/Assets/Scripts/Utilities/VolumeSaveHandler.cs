using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSaveHandler : MonoBehaviour
{
    
    public static VolumeSaveHandler instance { get; private set; }

    [Header("VolumeHandlerInfo")]
    [Space]

    [SerializeField] Slider volumeSliderMaster;
    [SerializeField] TextMeshProUGUI volumeTextMaster;

    [Space]

    [SerializeField] Slider volumeSliderMusic;
    [SerializeField] TextMeshProUGUI volumeTextMusic;

    [Space]

    [SerializeField] Slider volumeSliderFx;
    [SerializeField] TextMeshProUGUI volumeTextFX;

    [Space]

    [SerializeField] Slider volumeSliderAmbience;
    [SerializeField] TextMeshProUGUI volumeTextAmbience;

    [Space]

    [SerializeField] AudioMixer audioMixer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        LoadValues();
        
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
        volumeTextMaster.text = sliderValue.ToString("0.0");
        SaveVolumeMaster();
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        volumeTextMusic.text = sliderValue.ToString("0.0");
        SaveVolumeMusic();
    }

    public void SetFXVolume(float sliderValue)
{
    audioMixer.SetFloat("FX", Mathf.Log10(sliderValue) * 20);
    volumeTextFX.text = sliderValue.ToString("0.0");
    SaveVolumeFX();
    
}

    public void SetAmbienceVolume(float sliderValue)
    {
        audioMixer.SetFloat("Ambient", Mathf.Log10(sliderValue) * 20);
        volumeTextAmbience.text = sliderValue.ToString("0.0");
        SaveVolumeAmbience();
    }


    public void SaveVolumeMaster()
    {
        float volumeValueMaster = volumeSliderMaster.value;
        PlayerPrefs.SetFloat("Master", volumeValueMaster);

    }
    
    public void SaveVolumeMusic()
    {
        float volumeValueMusic = volumeSliderMusic.value;
        PlayerPrefs.SetFloat("Music", volumeValueMusic);
    }

    public void SaveVolumeFX()
{
    float volumeValueFX = volumeSliderFx.value;
    PlayerPrefs.SetFloat("FX", volumeValueFX); // Clave "Sounds" utilizada
    
}

    public void SaveVolumeAmbience()
    {
        float volumeValueAmbience = volumeSliderAmbience.value;
        PlayerPrefs.SetFloat("Ambient", volumeValueAmbience);
    }

    

    private void LoadValues()
{
    // Cargar y asignar el valor del Master
    if (PlayerPrefs.HasKey("Master"))
    {
        float volumeValueMaster = PlayerPrefs.GetFloat("Master");
        volumeSliderMaster.value = volumeValueMaster;
        audioMixer.SetFloat("Master", Mathf.Log10(volumeValueMaster) * 20);
    }

    // Cargar y asignar el valor del Music
    if (PlayerPrefs.HasKey("Music"))
    {
        float volumeValueMusic = PlayerPrefs.GetFloat("Music");
        volumeSliderMusic.value = volumeValueMusic;
        audioMixer.SetFloat("Music", Mathf.Log10(volumeValueMusic) * 20);
    }

    // Cargar y asignar el valor del Sounds
    if (PlayerPrefs.HasKey("FX"))
    {
        float volumeValueFX = PlayerPrefs.GetFloat("FX");

        volumeSliderFx.value = volumeValueFX;
        audioMixer.SetFloat("FX", Mathf.Log10(volumeValueFX) * 20);
        
    }

    // Cargar y asignar el valor del Ambient
    if (PlayerPrefs.HasKey("Ambient"))
    {
        float volumeValueAmbience = PlayerPrefs.GetFloat("Ambient");
        volumeSliderAmbience.value = volumeValueAmbience;
        audioMixer.SetFloat("Ambient", Mathf.Log10(volumeValueAmbience) * 20);
    }
}


}

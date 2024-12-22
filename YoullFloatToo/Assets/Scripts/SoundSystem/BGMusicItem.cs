using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BGMusicItem : MonoBehaviour
{
    public TMP_Text musicName;
    public GameObject tickIcon;

    private void Start() {
        gameObject.GetComponent<Button>().onClick.AddListener(ChangeBGMusic);
    }

    public void ChangeBGMusic()
    {
        BackgroundMusicController.instance.ChangeDefaultBGMusic(musicName.text);
    }
}

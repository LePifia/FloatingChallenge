using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EasyTransition{
    public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private TransitionSettings transition;
    [SerializeField] private float startDelay;

    private void Awake() {
        playButton.onClick.AddListener(PlayCLick);
        quitButton.onClick.AddListener(QuitClick);

        Time.timeScale = 1f;
    }

    private void PlayCLick(){
        TransitionManager.Instance().Transition(transition, startDelay);
        Loader.Load(Loader.Scene.GameScene);

    }

    private void QuitClick(){
        TransitionManager.Instance().Transition(transition, startDelay);
        Application.Quit((int)startDelay);
    }
}

}


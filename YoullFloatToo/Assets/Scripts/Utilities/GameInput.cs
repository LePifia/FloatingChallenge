using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput instance { get; private set; }

    private const string PLAYERPREFS_BINDINGS = "InPutBindings";

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractActionAlternative;
    public event EventHandler OnPauseAction;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        AlternativeInteraction,
        Pause
    }

    private InputSystem inputActions;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        inputActions = new InputSystem();

        inputActions.Player.Enable();

        
    }

    private void OnEnable() {
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.AlternativeInteraction.performed += AlternativeInteraction_performed;
        inputActions.Player.Pause.performed += OnPause;

        if (PlayerPrefs.HasKey(PLAYERPREFS_BINDINGS)){
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYERPREFS_BINDINGS)) ;

        }
    }

    

    private void OnDisable() {
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.AlternativeInteraction.performed -= AlternativeInteraction_performed;
        inputActions.Player.Pause.performed -= OnPause;
    }

    private void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
    }

    private void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        
        
    }

    private void AlternativeInteraction_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteractActionAlternative?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector(){

        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2> ();

        return inputVector;
    }

    public string GetBindingText(Binding binding){
        switch(binding){
            default:
            case Binding.Interact:
            return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.AlternativeInteraction:
            return inputActions.Player.AlternativeInteraction.bindings[0].ToDisplayString();
            case Binding.Move_Up:
            return inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
            return inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
            return inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
            return inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Pause:
            return inputActions.Player.Pause.bindings[0].ToDisplayString();
            
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound){
        inputActions.Player.Disable();

        InputAction inputAction;
        int inputIndex;

        switch (binding){
            default:
            case Binding.Move_Up:
            inputAction = inputActions.Player.Move;
            inputIndex = 1;
            break;
            case Binding.Move_Down:
            inputAction = inputActions.Player.Move;
            inputIndex = 2;
            break;
            case Binding.Move_Left:
            inputAction = inputActions.Player.Move;
            inputIndex = 3;
            break;
            case Binding.Move_Right:
            inputAction = inputActions.Player.Move;
            inputIndex = 4;
            break;

            case Binding.Interact:
            inputAction = inputActions.Player.Interact;
            inputIndex = 0;
            break;
            case Binding.AlternativeInteraction:
            inputAction = inputActions.Player.AlternativeInteraction;
            inputIndex = 0;
            break;
            case Binding.Pause:
            inputAction = inputActions.Player.Pause;
            inputIndex = 0;
            break;
        }

        inputAction.PerformInteractiveRebinding(inputIndex)
            .OnComplete(callback => {
                //Debug.Log(callback.action.bindings[1].path);
                //Debug.Log(callback.action.bindings[1].overridePath);
                callback.Dispose();
                inputActions.Player.Enable();
                onActionRebound();

                //inputActions.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString(PLAYERPREFS_BINDINGS,inputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
                }).Start();
    }
}

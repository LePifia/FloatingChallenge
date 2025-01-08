
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class HealthDisplay : NetworkBehaviour 
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthbarImage;


    public override void OnNetworkSpawn()
    {
       if (!IsClient){
        return;
       }

       health.CurrentHealth.OnValueChanged += Health_OnValueChanged;
       HandleHealthChanged(0, health.CurrentHealth.Value);
    }

    

    public override void OnNetworkDespawn()
    {
        if (!IsClient){
        return;
       }

       health.CurrentHealth.OnValueChanged -= Health_OnValueChanged;
    }

    private void Health_OnValueChanged(int previousValue, int newValue)
    {
        throw new NotImplementedException();
    }

    private void HandleHealthChanged(int oldHealth, int newHealth){
        healthbarImage.fillAmount = (float)newHealth / health.MaxHealth;
    }
}

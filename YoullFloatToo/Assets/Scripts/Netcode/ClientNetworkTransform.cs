using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

     void Update() {
        CanCommitToTransform = IsOwner;

        if (NetworkManager != null){
            if (NetworkManager.IsConnectedClient || NetworkManager.IsListening){
                if (CanCommitToTransform){
                    TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
                }
            }
        }
    }
    
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }


}

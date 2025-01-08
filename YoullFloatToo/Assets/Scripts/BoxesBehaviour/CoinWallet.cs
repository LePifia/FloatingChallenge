using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> totalCoins = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner){return;}
        
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsOwner){return;}
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.TryGetComponent<DestroyBoxes>(out DestroyBoxes detsroyBoxes)){return;}

        if (!IsServer){return;}

        totalCoins.Value += other.GetComponent<DestroyBoxes>().GetCoin();
        Debug.Log ("Got The coin");
    }

    private void OnCollisionEnter(Collision other) {
        
        if (!other.gameObject.GetComponent<DestroyBoxes>()){return;}
        if (!IsServer){return;}
         totalCoins.Value += 1;
         Debug.Log ("Got The coin");
    }

}

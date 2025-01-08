using Unity.Netcode;
using UnityEngine;

public class DamagingCollisions : NetworkBehaviour
{
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] Health yourHealth;

    private ulong ownerClientID;

    public void SetOwner(ulong ownerClientID){
        this.ownerClientID = ownerClientID;
    }

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
    private void OnCollisionEnter(Collision other) {

        if (other.rigidbody == null){return;}

        if (other.rigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj)){
            if (ownerClientID == netObj.OwnerClientId){return;}
        }

        if (((1 << other.gameObject.layer) & playerLayerMask) != 0 && other.gameObject.TryGetComponent<DamageCollision>(out var damage)) {
            yourHealth.TakeDamage(1);  
        }
    }

    private void  OnTriggerEnter(Collider other) {

        if (other.GetComponent<Rigidbody>() == null){return;}

        if (other.GetComponent<Rigidbody>().TryGetComponent<NetworkObject>(out NetworkObject netObj)){
            if (ownerClientID == netObj.OwnerClientId){return;}
        }
        
    
        if (((1 << other.gameObject.layer) & playerLayerMask) != 0 && other.gameObject.TryGetComponent<DamageCollision>(out var damage)){
        
            yourHealth.TakeDamage(1);
        }
        
        }
}

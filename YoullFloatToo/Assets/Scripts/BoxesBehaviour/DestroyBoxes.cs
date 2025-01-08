using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DestroyBoxes : NetworkBehaviour
{
    

    [Header("Destructible")]
    [Space]
    [SerializeField] Transform crateDestroyedPrefab;
    [SerializeField] LayerMask playerLayerMask;

    [Header ("CoinBehaviour")]
    [Space]

    [SerializeField] int basicCoin;

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
        if (((1 << other.gameObject.layer) & playerLayerMask) != 0 && other.gameObject.TryGetComponent<DamageCollision>(out var damageCollision)){
        
            Damage();
            
            
        }
    }

    private void  OnTriggerEnter(Collider other) {
        
    
        if (((1 << other.gameObject.layer) & playerLayerMask) != 0 && other.gameObject.TryGetComponent<DamageCollision>(out var damageCollision)){
        
            Damage();
            
            
        }
        
        }


    public void Damage()
    {
        Debug.Log ("Just destroy");
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(crateDestroyedTransform, 5000f, transform.position, 100f);
        Destroy(gameObject, 0.1f);

    }
    

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }

    public int GetCoin(){
        return basicCoin;
    }
}

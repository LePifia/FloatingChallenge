using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private ShipPlayer playerPrefab;
    [SerializeField] private float keptCoinPercentage;


    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        ShipPlayer[] players = FindObjectsByType<ShipPlayer>(FindObjectsSortMode.None);
        foreach (ShipPlayer player in players)
        {
            HandlePlayerSpawned(player);
        }

        ShipPlayer.OnPlayerSpawned += HandlePlayerSpawned;
        ShipPlayer.OnPlayerDespawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }

        ShipPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
        ShipPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(ShipPlayer player)
    {
        player.Health.OnDie += (health) => HandlePlayerDie(player);
    }

    private void HandlePlayerDespawned(ShipPlayer player)
    {
        player.Health.OnDie -= (health) => HandlePlayerDie(player);
    }

    private void HandlePlayerDie(ShipPlayer player)
    {
         int keptCoins = (int)(player.Wallet.totalCoins.Value * (keptCoinPercentage / 100));
        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId, keptCoins));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId, int keptCoins)
    {
        yield return null;

        ShipPlayer playerInstance = Instantiate(
            playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);

        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);

        playerInstance.Wallet.totalCoins.Value += keptCoins;

    }
}

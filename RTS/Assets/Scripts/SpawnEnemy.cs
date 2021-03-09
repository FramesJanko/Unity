using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnEnemy : NetworkBehaviour
{
    private NetworkConnection noConnection = null;

    [SerializeField]
    private GameObject enemyToSpawn;
    [Server]
    public void SpawnEnemyOnStart()
    {
        Debug.Log("Spawning enemy...");
        enemyToSpawn = Instantiate(enemyToSpawn, this.transform.position, Quaternion.identity);
        NetworkServer.Spawn(enemyToSpawn, noConnection);
    }
}

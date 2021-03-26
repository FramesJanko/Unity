using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnEnemy : NetworkBehaviour
{
    //private NetworkConnection noConnection = null;

    [SerializeField]
    private GameObject enemyToSpawn;
    
    public void SpawnEnemyOnStart()
    {
        if (!isServer)
            return;
        enemyToSpawn = Instantiate(enemyToSpawn, this.transform.position, enemyToSpawn.transform.rotation);
        NetworkServer.Spawn(enemyToSpawn);
        Debug.Log($"Spawning enemy..., rotation is {enemyToSpawn.transform.rotation}");
    }
}

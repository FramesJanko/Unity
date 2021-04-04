using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnEnemy : NetworkBehaviour
{
    //private NetworkConnection noConnection = null;

    [SerializeField]
    private GameObject enemyPrefab;
    
    public void SpawnEnemyOnStart()
    {
        if (!isServer)
            return;
        GameObject enemy = Instantiate(enemyPrefab, transform.position, enemyPrefab.transform.rotation);
        NetworkServer.Spawn(enemy);
        enemy.GetComponent<NpcController>().origin = transform.position;
        Debug.Log($"Spawning enemy..., rotation is {enemyPrefab.transform.rotation}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [SerializeField]
    SpawnEnemy se;
    public override void OnStartServer()
    {
        Debug.Log("Server Started!");
        
    }
    public override void OnStopServer()
    {
        Debug.Log("Server Stopped!");
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log($"Connected to server: {conn}");
        se.SpawnEnemyOnStart();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log($"Disonnected from server: {conn}");
    }
}

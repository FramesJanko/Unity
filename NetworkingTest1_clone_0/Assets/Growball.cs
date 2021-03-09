using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growball : NetworkBehaviour
{
    // Start is called before the first frame update
    public Vector3 spawn;

    public override void OnStartServer()
    {
        if (isServer)
        {
            Instantiate(gameObject, spawn, Quaternion.identity);
            NetworkServer.Spawn(gameObject);
        }
    }

}

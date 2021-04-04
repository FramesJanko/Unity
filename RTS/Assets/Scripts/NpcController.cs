using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class NpcController : NetworkBehaviour
{
    public GameObject target;
    public Player[] players;
    public List<float> playerDistances;
    public List<Player> playerList;
    public float distanceFromTarget;
    float previousDistanceFromPlayer = 0f;
    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        players = FindObjectsOfType<Player>();


        foreach (Player p in players)
        {


            float distanceFromPlayer = Vector3.Distance(p.transform.position, transform.position);
            if (distanceFromPlayer < 5f)
            {
                playerDistances.Add(distanceFromPlayer);

                if (distanceFromPlayer < previousDistanceFromPlayer)
                {
                    playerList.Clear();
                    
                }
                playerList.Add(p);

                previousDistanceFromPlayer = distanceFromPlayer;
                target = playerList[0].gameObject;
            }
        }
        
        
        
        //playerDistances.ToArray();
        

    }
}

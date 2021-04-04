using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using System;
using UnityEngine.AI;

public class NpcController : NetworkBehaviour
{
    [SyncVar]
    public GameObject target;
    public Player[] players;
    public List<float> playerDistances;
    public List<Player> playerList;
    public float distanceFromTarget;
    float previousDistanceFromPlayer = 0f;
    private bool gameStarted;
    public Vector3 origin;
    public NavMeshAgent agent;
    public Vector3 movementLocation;
    public bool walking;
    public bool returningToOrigin;
    public float distanceFromOrigin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        

    }
    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            CheckIfWalking();
            FindTarget();
            if (target != null)
            {
                distanceFromTarget = CalculateDistanceFromTarget();
            }
            HandleMovement();
        }
        
        
        
        
        //playerDistances.ToArray();
        

    }

    private void CheckIfWalking()
    {
        if (movementLocation != transform.position)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }
    }

    private void HandleMovement()
    {
        distanceFromOrigin = Vector3.Distance(transform.position, origin);
        if (target == null || distanceFromOrigin > 10)
        {
            returningToOrigin = true;
            target = null;
            ClearTarget();
            Debug.Log("Clearing target: " + target);

            movementLocation = origin;
        }
        if (Vector3.Distance(transform.position, origin) < 1)
        {
            returningToOrigin = false;
        }
        
        
        if (!returningToOrigin)
        {
            if (target != null)
            {
                if (Vector3.Distance(target.transform.position, transform.position) > 5)
                {
                    movementLocation = target.transform.position;

                }
                if (Vector3.Distance(target.transform.position, transform.position) <= 5)
                {

                    movementLocation = transform.position;

                }
            }
        }
        
        
        agent.SetDestination(movementLocation);
    }

    public GameObject FindTarget()
    {
        if (!returningToOrigin)
        {
            Debug.Log("test if returning to Origin");

            players = FindObjectsOfType<Player>();


            foreach (Player p in players)
            {


                float distanceFromPlayer = Vector3.Distance(p.transform.position, transform.position);
                if (distanceFromPlayer < 7f)
                {
                    playerDistances.Add(distanceFromPlayer);

                    if (distanceFromPlayer < previousDistanceFromPlayer)
                    {
                        playerList.Clear();

                    }
                    playerList.Add(p);

                    previousDistanceFromPlayer = distanceFromPlayer;
                    target = playerList[0].gameObject;
                    CmdSetTarget();
                }
            }
        }
        
        return target;
    }
    public void CmdSetTarget()
    {
        GetComponent<Combat>().target = target;
    }
    public void ClearTarget()
    {
        GetComponent<Combat>().target = null;

    }
    public float CalculateDistanceFromTarget()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        return distance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;
using System;
using UnityEngine.AI;

public class NpcController : NetworkBehaviour
{
    public GameObject target;

    public List<Player> players;
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

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        
        CheckIfWalking();
        FindTarget();
        if (target != null)
        {
            distanceFromTarget = CalculateDistanceFromTarget();
        }
        HandleMovement();
        
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
            RpcClearTarget();
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
    
    public void FindTarget()
    {
        if (!returningToOrigin && target == null)
        {
            players = FindObjectsOfType<Player>().ToList();
            foreach (Player p in players)
            {
                float distanceFromPlayer = Vector3.Distance(p.transform.position, transform.position);
                if (distanceFromPlayer < 7f)
                {
                    if (previousDistanceFromPlayer == 0)
                    {
                        target = p.gameObject;
                        previousDistanceFromPlayer = distanceFromPlayer;
                    }
                    else if (distanceFromPlayer < previousDistanceFromPlayer)
                    {
                        target = p.gameObject;
                        previousDistanceFromPlayer = distanceFromPlayer;
                    }
                    RpcSetTarget(target);
                }
            }
        }
    }

    public void RpcSetTarget(GameObject tempTarget)
    {
        target = tempTarget;
        GetComponent<Combat>().target = target;
    }

    public void RpcClearTarget()
    {
        target = null;
        GetComponent<Combat>().target = null;
    }
    public float CalculateDistanceFromTarget()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        return distance;
    }
}

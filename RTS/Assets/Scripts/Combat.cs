using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Combat : NetworkBehaviour
{
    private Player player;
    private NpcController npcController;

    [SyncVar]
    public GameObject target;

    [SerializeField]
    private float baseAttackTime;

    [SerializeField]
    private float damage;
    [SyncVar]
    public float distanceFromTarget;
    [SyncVar]
    public bool walking;
    [SyncVar]
    public bool isAttacking;
    
    private bool isPlayer;
    private int coroutineCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Player>())
        {
            player = GetComponent<Player>();
            isPlayer = true;
        }
        if (GetComponent<NpcController>())
        {
            npcController = GetComponent<NpcController>();
            isPlayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isLocalPlayer)
        //    return;
        if (isPlayer)
        {
            distanceFromTarget = player.distanceFromTarget;
            walking = player.walking;
        }
        
        if (!isPlayer)
        {
            distanceFromTarget = npcController.distanceFromTarget;
            walking = npcController.walking;
        }
        if (CheckValidTarget(target))
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        StartCoroutine(Attack(damage));
        Debug.Log(name + ": Count is " + coroutineCount);

    }
    private bool CheckValidTarget(GameObject currentTarget)
    {
        if (currentTarget != null && distanceFromTarget < 7 && !isAttacking)
        {

            Debug.Log(name + ": target is valid.");
            return true;
            

        }
        else
        {
            return false;
        }
    }
    [Command]
    public void CmdModifyHealth(GameObject currentTarget, float healthChange)
    {
        //if (!isServer)
        //    return;

        
        currentTarget.GetComponent<Health>().currentHealth += healthChange;

        //float newHealthPercent = currentTarget.GetComponent<Health>().currentHealth / currentTarget.GetComponent<Health>().maxHealth;
        
        //currentTarget.GetComponentInChildren<Healthbar>().currentHealthPercent = newHealthPercent;
        
        //Remove(currentTarget);
    }

    private IEnumerator Attack(float damage)
    {
        isAttacking = true;
        coroutineCount++;
        
        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);


        if (distanceFromTarget < 7 && !walking && currentTarget == target && gameObject.activeSelf)
        {
            Debug.Log(name + " is attacking...");
            if (isPlayer)
            {
                CmdModifyHealth(currentTarget, damage);

            }
            if (!isPlayer && isServer)
            {
                currentTarget.GetComponent<Health>().currentHealth += damage;
            }

        }

        isAttacking = false;

    }
    
    
    //public void Remove(GameObject currentTarget)
    //{

    //    if (currentTarget.GetComponent<Health>().currentHealth <= 0f)
    //    {
    //        Player[] playerList = GetComponents<Player>();
    //        foreach (Player p in playerList)
    //        {
    //            if (p.target == gameObject)
    //            {
    //                p.target = null;
    //            }
    //        }
    //        Destroy(currentTarget);
    //    }
    //}

}

    


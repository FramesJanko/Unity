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

        }
        if (CheckValidTarget(target))
        {
            StartAttack();
        }

    }
    
    private void StartAttack()
    {
        StartCoroutine(Attack(damage));
        Debug.Log("Count is " + coroutineCount);

    }
    private bool CheckValidTarget(GameObject currentTarget)
    {
        if (currentTarget != null && player.distanceFromTarget < 7 && !isAttacking)
        {

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


        if (player.distanceFromTarget < 7 && !player.walking && currentTarget == target)
        {
            Debug.Log("Attacking...");
            CmdModifyHealth(currentTarget, damage);
            
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

    


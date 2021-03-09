using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Combat : NetworkBehaviour
{
    private Player player;
    private GameObject target;

    [SerializeField]
    private float baseAttackTime;

    [SerializeField]
    private float damage;

    public bool isAttacking;

    private int coroutineCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        target = player.target;
        if (CheckValidTarget(target))
        {
            CmdAttack();
        }

    }
    [Command]
    private void CmdAttack()
    {
        StartCoroutine(Attack(damage));
        Debug.Log("Count is " + coroutineCount);
    }
    private bool CheckValidTarget(GameObject target)
    {
        if (target != null && player.distanceFromTarget < 7 && !isAttacking)
        {

            return true;
            

        }
        else
        {
            return false;
        }
    }
    
    //[ClientCallback]
    private IEnumerator Attack(float damage)
    {
        isAttacking = true;
        coroutineCount++;
        Debug.Log("Attacking...");
        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);


        if (player.distanceFromTarget < 7 && !player.walking && currentTarget == target)
        {
            target.GetComponent<Health>().ModifyHealth(damage);
            
        }

        isAttacking = false;

    }
        
}

    


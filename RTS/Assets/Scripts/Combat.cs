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
    private bool hasAnimator;
    [SyncVar]
    public bool attackIsCanceled;
    private bool isPlayer;
    private int coroutineCount = 0;
    public Animator animator;

    IEnumerator AttackCoroutine;
    [SerializeField]
    public float attackRange;
    [SerializeField]
    public float baseAttackRange;



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

        if (GetComponent<Animator>())
        {
            hasAnimator = true;
            animator = GetComponent<Animator>();
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
            target = player.target;
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
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.S))
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
            CancelAttack();
            
        }
        
    }

    private void StartAttack()
    {
        
        if(AttackCoroutine != null)
            StopCoroutine(AttackCoroutine);

        
        AttackCoroutine = Attack();
        StartCoroutine(AttackCoroutine);
        

    }

    public void CancelAttack()
    {
        attackIsCanceled = true;
        isAttacking = false;
    }
    private bool CheckValidTarget(GameObject currentTarget)
    {
        bool TargetValid;
        if (currentTarget != null && distanceFromTarget < baseAttackRange && !isAttacking)
        {


            
            TargetValid = true;
            

        }
        else if (currentTarget != null && distanceFromTarget < attackRange && isAttacking)
        {
            TargetValid = false;
        }
        else if(currentTarget != null && distanceFromTarget > attackRange)
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
            
            isAttacking = false;
            attackIsCanceled = true;
            TargetValid = false;
        }
        else if(target == null)
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
            attackIsCanceled = true;

            TargetValid = false;
        }
        else
        {
            TargetValid = false;
        }
        if (walking)
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
            attackIsCanceled = true;
            
            TargetValid = false;
            
        }
        return TargetValid;
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
    
    private IEnumerator Attack()
    {
        isAttacking = true;
        attackIsCanceled = false;
        coroutineCount++;
        if (hasAnimator)
        {
            animator.SetBool("IsAttacking", true);
        }
        Debug.Log("Attack Coroutine Started: " + coroutineCount);

        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);



        if (distanceFromTarget < attackRange && !walking && currentTarget == target && target.activeSelf && gameObject.activeSelf && !attackIsCanceled)
        {
            Debug.Log(name + " attacked successfully");
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

    


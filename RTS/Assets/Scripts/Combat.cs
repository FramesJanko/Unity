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
    public float baseAttackTime;

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

    
    public float attackTimer;

    IEnumerator AttackCoroutine;
    [SerializeField]
    public float attackRange;
    [SerializeField]
    public float baseAttackRange;

    [SyncVar]
    public bool shouldAnimateAttack;



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
        if (!isPlayer)
        {
            if (isAttacking)
            {
                attackTimer += Time.deltaTime;
                
            }
            else
            {
                attackTimer = 0f;
            }
        }
        if (CheckValidTarget(target))
        {
            StartAttack();
        }
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.S))
        {
            StopAnimateAttack();
            CancelAttack();
            
        }
        if (shouldAnimateAttack)
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", true);
            }
        }
        else
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
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
            AnimateAttack();
            TargetValid = true;


        }
        else if (currentTarget != null && distanceFromTarget < attackRange && isAttacking)
        {
            TargetValid = false;
        }
        else if(currentTarget != null && distanceFromTarget > attackRange)
        {
            StopAnimateAttack();
            isAttacking = false;
            attackIsCanceled = true;
            TargetValid = false;
        }
        else if(target == null)
        {
            StopAnimateAttack();
            attackIsCanceled = true;

            TargetValid = false;
        }
        else
        {
            TargetValid = false;
        }
        if (walking)
        {
            StopAnimateAttack();
            attackIsCanceled = true;
            
            TargetValid = false;
            
        }
        return TargetValid;
    }

    [Command]
    public void AnimateAttack()
    {
        shouldAnimateAttack = true;
    }
    [Command]
    public void StopAnimateAttack()
    {
        shouldAnimateAttack = false;
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
        AnimateAttack();
        

        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);



        if (distanceFromTarget < attackRange && !walking && currentTarget == target && target.activeSelf && gameObject.activeSelf && !attackIsCanceled)
        {
            
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

    


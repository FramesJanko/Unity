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
    public float attackspeed;

    [SyncVar]
    public GameObject target;

    public float initialBaseAttackTimeAndBackSwing;

    [SerializeField]
    public float baseAttackTimeAndBackSwing;

    
    public float baseAttackTime;

    
    public float attackBackswing;

    [SyncVar]
    public float damage;

    [SerializeField]
    public float attackRange;
    [SerializeField]
    public float baseAttackRange;

    [SyncVar]
    public float momentumAttackSpeedValue;

    [SyncVar]
    public float distanceFromTarget;

    [SyncVar]
    public bool walking;

    [SyncVar]
    public bool isAttacking;

    [SyncVar]
    public bool attackFinished;
    private bool incrementAttackTimer;
    private bool hasAnimator;

    [SyncVar]
    public bool attackIsCanceled;
    public bool isPlayer;
    private int coroutineCount = 0;
    public Animator animator;

    [SyncVar]
    public float attackTimer;

    IEnumerator AttackCoroutine;
    
    [SyncVar]
    public bool shouldAnimateAttack;

    
    public float timeSinceLastSuccessfulAttack;



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
        
        attackFinished = true;
        attackspeed = 1f;
        CalculateAttackSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {

        }
        if (isPlayer)
        {
            distanceFromTarget = player.distanceFromTarget;
            walking = player.walking;
            target = player.target;
        }
        
        if (!isPlayer && isServer)
        {
            distanceFromTarget = npcController.distanceFromTarget;
            walking = npcController.walking;
            
            //attackTimer += Time.deltaTime;
            
            if(incrementAttackTimer)
            {
                attackTimer += Time.deltaTime;
            }
        }
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.S))
        {
            StopAttack();
            StopAttackServer();
            StopAnimateAttack();
            StopAnimateAttackServer();
        }
        if (isLocalPlayer && Input.GetKey(KeyCode.Q))
        {
            ModifyAttackSpeed(momentumAttackSpeedValue);
            CalculateAttackSpeed();
            CmdCalculateAttackSpeed();

        }

        //CalculateAttackSpeed();
        
        timeSinceLastSuccessfulAttack += Time.deltaTime;

        if (CheckValidTarget(target))
        {
            Debug.Log(CheckValidTarget(target));
            StartAttack();
        }

        if (shouldAnimateAttack)
        {
            if (hasAnimator && !animator.GetBool("IsAttacking"))
            {
                animator.SetBool("IsAttacking", true);
                
            }
        }
        else
        {
            if (hasAnimator && animator.GetBool("IsAttacking"))
            {
                animator.SetBool("IsAttacking", false);
            }
        }
    }
    [Command]
    public void CmdCalculateAttackSpeed()
    {
        baseAttackTimeAndBackSwing = (1 / (1 / initialBaseAttackTimeAndBackSwing * attackspeed));
        baseAttackTime = .8f * baseAttackTimeAndBackSwing;
        attackBackswing = .2f * baseAttackTimeAndBackSwing;

        if (hasAnimator)
            animator.SetFloat("speedMultiplier", attackspeed);
    }
    private void CalculateAttackSpeed()
    {
        baseAttackTimeAndBackSwing = (1/(1/initialBaseAttackTimeAndBackSwing * attackspeed));
        baseAttackTime = .8f * baseAttackTimeAndBackSwing;
        attackBackswing = .2f * baseAttackTimeAndBackSwing;
        
        if (hasAnimator)
            animator.SetFloat("speedMultiplier", attackspeed);
    }

    [Command]
    public void ModifyAttackSpeed(float attackspeedChange)
    {
        attackspeed += attackspeedChange;
    }

    private void StartAttack()
    {
        
        if(AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            if (isPlayer && isLocalPlayer)
                Debug.Log("Stopping current attackCoroutine before initiating new one");

        }
        AttackCoroutine = Attack();
        StartCoroutine(AttackCoroutine);
    }
    
    private bool CheckValidTarget(GameObject currentTarget)
    {
        bool TargetValid;

        if (currentTarget != null && distanceFromTarget < baseAttackRange && !isAttacking)
        {
            TargetValid = true;
            //Debug.Log(gameObject.name + " Success: Starting Attack - Code 1 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        }
        else if (currentTarget != null && distanceFromTarget < attackRange && isAttacking && timeSinceLastSuccessfulAttack > attackBackswing && attackFinished)
        {
            if (isPlayer && isLocalPlayer)
            {

                //StopAttack();
                StopAttackServer();
                Debug.Log(gameObject.name + " Finished attack - Code 2 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
            else
            {
                Debug.Log("Line 221");
                NPCStopAttack();
            }
                
            TargetValid = false;
            
        }
        else if (currentTarget != null && distanceFromTarget < attackRange && isAttacking)
        {
            TargetValid = false;
            //if(isPlayer && isLocalPlayer)
            //    Debug.Log("Error: already attacking - Code 3 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        }
        
        else if (currentTarget != null && distanceFromTarget > attackRange)
        {
            if (isPlayer && isLocalPlayer && isAttacking)
            {
                
                //StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                Debug.Log(gameObject.name + " Error: Target too far - Code 4 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
            else if(!isPlayer && isAttacking)
            {
                NPCStopAttack();
                Debug.Log("Line 249");
            }
                



            TargetValid = false;
            
        }
        else if ((target == null && isAttacking) || (target == null && shouldAnimateAttack))
        {
            if (isPlayer && isLocalPlayer)
            {
                
                //StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                Debug.Log(gameObject.name + " Error: No target - Code 5 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
            else
            {
                Debug.Log("Line 271");
                NPCStopAttack();
            }
                


            TargetValid = false;
            
        }
        else
        {
            TargetValid = false;
            //if (isPlayer && isLocalPlayer)
            //    Debug.Log(gameObject.name + " Error: Other - Code 6 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        }
        if (walking && isAttacking)
        {
            if (isPlayer && isLocalPlayer)
            {
                //Debug.Log("StopAttack called");
                //StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                //Debug.Log(gameObject.name + " Error: Walking - Code 7 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
                
            else
            {
                Debug.Log("Line 300");
                NPCStopAttack();
            }
                


            TargetValid = false;
            

        }
        //if (timeSinceLastSuccessfulAttack > attackBackswing)
        //{
        //    StopAnimateAttack();
        //}
        //else
        //    TargetValid = false;
        return TargetValid;
    }

    private void NPCStopAttack()
    {
        Debug.Log(name + " NPCStopAttack");
        isAttacking = false;
        timeSinceLastSuccessfulAttack = attackBackswing;
        attackFinished = true;
        incrementAttackTimer = false;
        attackTimer = 0f;
    }
    
    public void AnimateAttack()
    {
        attackFinished = false;
        shouldAnimateAttack = true;
        isAttacking = true;
        attackIsCanceled = false;
    }
    [Command]
    public void AnimateAttackServer()
    {
        attackFinished = false;
        shouldAnimateAttack = true;
        isAttacking = true;
        attackIsCanceled = false;
    }
    
    public void StopAnimateAttack()
    {
        Debug.Log("StopAnimateAttack called");
        shouldAnimateAttack = false;
    }
    [Command]
    public void StopAnimateAttackServer()
    {
        Debug.Log("StopAnimateAttack called");
        shouldAnimateAttack = false;
    }
    public void StopAttack()
    {

        Debug.Log("StopAttack called");
        attackIsCanceled = true;
        isAttacking = false;
        //timeSinceLastSuccessfulAttack = attackBackswing;
        attackFinished = true;
    }
    [Command]
    public void StopAttackServer()
    {

        Debug.Log("StopAttackServer called");
        attackIsCanceled = true;
        isAttacking = false;
        //timeSinceLastSuccessfulAttack = attackBackswing;
        attackFinished = true;
    }
    [Command]
    public void CmdModifyHealth(GameObject currentTarget, float damage, float healing)
    {
        currentTarget.GetComponent<Health>().currentHealth -= damage;
        currentTarget.GetComponent<Health>().currentHealth += healing;
    }
    
    private IEnumerator Attack()
    {
        Debug.Log($" {name} Is player: {isPlayer}, Is Local Player: {isLocalPlayer}");
        if (isPlayer && isLocalPlayer)
        {
            Debug.Log("AnimateAttack called");
            AnimateAttack();
            AnimateAttackServer();
        }
        else if(!isPlayer)
            NPCStartAttack();
        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);

        if (distanceFromTarget < attackRange && !walking && currentTarget == target && target.activeSelf && gameObject.activeSelf && !attackIsCanceled)
        {
            
            if (isPlayer)
            {
                CmdModifyHealth(currentTarget, damage, 0f);
            }
            if (!isPlayer && isServer)
            {
                currentTarget.GetComponent<Health>().currentHealth += damage;
            }
        }
        Debug.Log("Time since last attack: " + timeSinceLastSuccessfulAttack);
        timeSinceLastSuccessfulAttack = 0f;
        
        attackFinished = true;
    }

    private void NPCStartAttack()
    {
        Debug.Log(name + " NPCStartAttack");
        isAttacking = true;
        attackFinished = false;
        incrementAttackTimer = true;
    }
}

    


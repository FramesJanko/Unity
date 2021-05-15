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
    public bool shouldAttack;

    [SyncVar]
    public float attackspeed;

    [SyncVar]
    public GameObject target;

    public float initialBaseAttackTimeAndBackSwing;

    [SerializeField]
    public float baseAttackTimeAndBackSwing;

    
    public float baseAttackTime;

    
    public float attackBackswing;

    [SerializeField]
    private float damage;

    [SerializeField]
    public float maxAttackRange;
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
    private bool isPlayer;
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
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        if (isPlayer)
        {
            distanceFromTarget = player.distanceFromTarget;
            walking = player.walking;
            target = player.target;
        }
=======
        if (isServer)
        {
            if (isPlayer)
            {
                walking = player.walking;
                target = player.target;
                distanceFromTarget = player.distanceFromTarget;
            }
            else if (!isPlayer)
            {
                distanceFromTarget = npcController.distanceFromTarget;
                walking = npcController.walking;

                if (incrementAttackTimer)
                {
                    attackTimer += Time.deltaTime;
                }
            }
            CalculateAttackSpeed();
        }
        if (hasAnimator)
            animator.SetFloat("speedMultiplier", attackspeed);
>>>>>>> Stashed changes
        
        if (isServer)
        { 
            timeSinceLastSuccessfulAttack += Time.deltaTime;
            if (target != null)
            {
                shouldAttack = CheckValidTarget(target);
                if (shouldAttack)
                {
                    Debug.Log(shouldAttack);
                    //StartAttack();
                }
            }
            else
                shouldAttack = false;
        }
        
        //if (isPlayer)
        //{
        //    distanceFromTarget = player.distanceFromTarget;
        //    walking = player.walking;
        //    target = player.target;
        //}

        //if (!isPlayer && isServer)
        //{
        //    distanceFromTarget = npcController.distanceFromTarget;
        //    walking = npcController.walking;

        //    //attackTimer += Time.deltaTime;

        //    if(incrementAttackTimer)
        //    {
        //        attackTimer += Time.deltaTime;
        //    }
        //}
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.S))
        {
            //StopAttack();
            //StopAttackServer();
            //StopAnimateAttack();
            //StopAnimateAttackServer();
        }
        if (isLocalPlayer && Input.GetKey(KeyCode.Q))
        {
<<<<<<< Updated upstream
            ModifyAttackSpeed(momentumAttackSpeedValue);

        }

        CalculateAttackSpeed();
        
        timeSinceLastSuccessfulAttack += Time.deltaTime;
        
        if (CheckValidTarget(target))
        {
            StartAttack();
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
=======
            //ModifyAttackSpeed(momentumAttackSpeedValue);
            //CalculateAttackSpeed();
            //CmdCalculateAttackSpeed();

        }

        

        

        //if (CheckValidTarget(target))
        //{
        //    Debug.Log(CheckValidTarget(target));
        //    StartAttack();
        //}

        //if (shouldAnimateAttack)
        //{
        //    if (hasAnimator && !animator.GetBool("IsAttacking"))
        //    {
        //        animator.SetBool("IsAttacking", true);

        //    }
        //}
        //else
        //{
        //    if (hasAnimator && animator.GetBool("IsAttacking"))
        //    {
        //        animator.SetBool("IsAttacking", false);
        //    }
        //}
>>>>>>> Stashed changes
    }

    private void CalculateAttackSpeed()
    {
        baseAttackTimeAndBackSwing = (1/(1/initialBaseAttackTimeAndBackSwing * attackspeed));
        baseAttackTime = .8f * baseAttackTimeAndBackSwing;
        attackBackswing = .2f * baseAttackTimeAndBackSwing;
        
        
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
            //if (isPlayer && isLocalPlayer)
            //    Debug.Log("Stopping current attackCoroutine before initiating new one");

        }
        AttackCoroutine = Attack();
        StartCoroutine(AttackCoroutine);
    }
    
    private bool CheckValidTarget(GameObject currentTarget)
    {
        bool TargetValid;

        if (distanceFromTarget < baseAttackRange && !isAttacking)
        {
            TargetValid = true;
            //Debug.Log(gameObject.name + " Success: Starting Attack - Code 1 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        }
<<<<<<< Updated upstream
        else if (currentTarget != null && distanceFromTarget < attackRange && isAttacking && timeSinceLastSuccessfulAttack > attackBackswing && attackFinished)
        {
            if (isPlayer && isLocalPlayer)
            {
                Debug.Log("StopAttack called, but animation continue");
                StopAttack();
                StopAttackServer();
                //Debug.Log(gameObject.name + " Finished attack - Code 2 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
            else
                NPCStopAttack();
            TargetValid = false;
=======
        //else if (distanceFromTarget < maxAttackRange && isAttacking && timeSinceLastSuccessfulAttack > attackBackswing && attackFinished)
        //{
        //    //if (isPlayer && isLocalPlayer)
        //    //{

        //    //    //StopAttack();
        //    //    StopAttackServer();
        //    //    Debug.Log(gameObject.name + " Finished attack - Code 2 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log("Line 221");
        //    //    NPCStopAttack();
        //    //}
                
        //    TargetValid = false;
>>>>>>> Stashed changes
            
        //}
        else if (distanceFromTarget < maxAttackRange && isAttacking)
        {
            TargetValid = false;
            //if(isPlayer && isLocalPlayer)
            //    Debug.Log("Error: already attacking - Code 3 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        }
        
        else if (distanceFromTarget > maxAttackRange)
        {
<<<<<<< Updated upstream
            if (isPlayer && isLocalPlayer)
            {
                Debug.Log("StopAttack called");
                StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                //Debug.Log(gameObject.name + " Error: Target too far - Code 4 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
            else
                NPCStopAttack();
=======
            //if (isPlayer && isLocalPlayer && isAttacking)
            //{
                
            //    //StopAttack();
            //    StopAttackServer();
            //    StopAnimateAttack();
            //    StopAnimateAttackServer();
            //    Debug.Log(gameObject.name + " Error: Target too far - Code 4 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            //}
            //else if(!isPlayer && isAttacking)
            //{
            //    NPCStopAttack();
            //    Debug.Log("Line 249");
            //}
                
>>>>>>> Stashed changes



            TargetValid = false;
            
        }
<<<<<<< Updated upstream
        else if (target == null)
        {
            if (isPlayer && isLocalPlayer)
            {
                Debug.Log("StopAttack called");
                StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                //Debug.Log(gameObject.name + " Error: No target - Code 5 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
            else
                NPCStopAttack();
=======
        //else if ((target == null && isAttacking) || (target == null && shouldAnimateAttack))
        //{
        //    if (isPlayer && isLocalPlayer)
        //    {
                
        //        //StopAttack();
        //        StopAttackServer();
        //        StopAnimateAttack();
        //        StopAnimateAttackServer();
        //        Debug.Log(gameObject.name + " Error: No target - Code 5 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        //    }
        //    else
        //    {
        //        Debug.Log("Line 271");
        //        NPCStopAttack();
        //    }
                
>>>>>>> Stashed changes


        //    TargetValid = false;
            
        //}
        else
        {
            TargetValid = false;
            //if (isPlayer && isLocalPlayer)
            //    Debug.Log(gameObject.name + " Error: Other - Code 6 - Network ID: " + GetComponent<NetworkIdentity>().netId);
        }
        if (walking)
        {
<<<<<<< Updated upstream
            if (isPlayer && isLocalPlayer)
            {
                Debug.Log("StopAttack called");
                StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                //Debug.Log(gameObject.name + " Error: Walking - Code 7 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            }
                
            else
                NPCStopAttack();
=======
            //if (isPlayer && isLocalPlayer)
            //{
            //    //Debug.Log("StopAttack called");
            //    //StopAttack();
            //    StopAttackServer();
            //    StopAnimateAttack();
            //    StopAnimateAttackServer();
            //    //Debug.Log(gameObject.name + " Error: Walking - Code 7 - Network ID: " + GetComponent<NetworkIdentity>().netId);
            //}
                
            //else
            //{
            //    Debug.Log("Line 300");
            //    NPCStopAttack();
            //}
                
>>>>>>> Stashed changes


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
        shouldAnimateAttack = false;
    }
    [Command]
    public void StopAnimateAttackServer()
    {
        shouldAnimateAttack = false;
    }
    public void StopAttack()
    {


        attackIsCanceled = true;
        isAttacking = false;
        //timeSinceLastSuccessfulAttack = attackBackswing;
        attackFinished = true;
    }
    [Command]
    public void StopAttackServer()
    {

        
        attackIsCanceled = true;
        isAttacking = false;
        //timeSinceLastSuccessfulAttack = attackBackswing;
        attackFinished = true;
    }
    [Command]
    public void CmdModifyHealth(GameObject currentTarget, float healthChange)
    {
        currentTarget.GetComponent<Health>().currentHealth += healthChange;
    }
    
    private IEnumerator Attack()
    {
        //Debug.Log($"Is player: {isPlayer}, Is Local Player: {isLocalPlayer}");
        if (isPlayer && isLocalPlayer)
        {
            Debug.Log("AnimateAttack called");
            AnimateAttack();
            AnimateAttackServer();
        }
            
        else
            NPCStartAttack();
        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);

        if (distanceFromTarget < maxAttackRange && !walking && currentTarget == target && target.activeSelf && gameObject.activeSelf && !attackIsCanceled)
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
        Debug.Log("Time since last attack: " + timeSinceLastSuccessfulAttack);
        timeSinceLastSuccessfulAttack = 0f;
        
        attackFinished = true;
    }

    private void NPCStartAttack()
    {
        isAttacking = true;
        attackFinished = false;
        incrementAttackTimer = true;
    }
}

    


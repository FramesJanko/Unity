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

<<<<<<< Updated upstream
=======
    
    public float baseAttackTime;

    [SyncVar]
    public float AttackAnimatorNormalizedTime;
    
    public float attackBackswing;

    [SyncVar]
    public float damage;

    [SerializeField]
    public float attackRange;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        //if (!isLocalPlayer)
        //    return;
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        
        if (isPlayer)
        {
            distanceFromTarget = player.distanceFromTarget;
            walking = player.walking;
            target = player.target;
        }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        
=======

>>>>>>> Stashed changes
=======

>>>>>>> Stashed changes
=======

>>>>>>> Stashed changes
        if (!isPlayer)
        {
            distanceFromTarget = npcController.distanceFromTarget;
            walking = npcController.walking;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        }
        if (CheckValidTarget(target))
        {
            StartAttack();
        }
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.S))
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

            //attackTimer += Time.deltaTime;

            if (incrementAttackTimer)
            {
                attackTimer += Time.deltaTime;
            }
        }
        timeSinceLastSuccessfulAttack += Time.deltaTime;
         
        

        if (isLocalPlayer && Input.GetKeyDown(KeyCode.S))
        {
            //StopAttack();
            StopAttackServer();
            //StopAnimateAttack();
            StopAnimateAttackServer();
        }
        if (isLocalPlayer && Input.GetKey(KeyCode.Q))
        {
            ModifyAttackSpeed(momentumAttackSpeedValue);
            CmdCalculateAttackSpeed();
            CalculateAttackSpeed();
            

        }
        if (hasAnimator)
        {
            animator.SetFloat("speedMultiplier", attackspeed);
        }
        
        if (hasAnimator)
        {
            AttackAnimatorNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        if (CheckValidTarget(target))
        {
            BeginAnimatedAttack();
            //StartAttack();
<<<<<<< Updated upstream
        }
        if (AttackAnimatorNormalizedTime == baseAttackTime)
        {
            CmdModifyHealth(target, damage, 0f);
        }
        if(AttackAnimatorNormalizedTime == baseAttackTimeAndBackSwing)
        {
            StopAttack();
        }
=======
        }
        if (AttackAnimatorNormalizedTime == baseAttackTime)
        {
            CmdModifyHealth(target, damage, 0f);
        }
        if(AttackAnimatorNormalizedTime == baseAttackTimeAndBackSwing)
        {
            StopAttack();
        }
>>>>>>> Stashed changes
        

        //CalculateAttackSpeed();

        

        

        if (shouldAnimateAttack)
        {
            if (hasAnimator && !animator.GetBool("IsAttacking"))
            {
                animator.SetBool("IsAttacking", true);
                
            }
        }
        else
>>>>>>> Stashed changes
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
            CancelAttack();
            
        }
<<<<<<< Updated upstream
=======
    }
    public void ResolveAttackHit()
    {
        Debug.Log("Attack Hit!");
    }
    private void BeginAnimatedAttack()
    {
        shouldAnimateAttack = true;
        if (isPlayer)
        {
            attackFinished = false;
            shouldAnimateAttack = true;
            isAttacking = true;
            attackIsCanceled = false;
        }
        else if (!isPlayer)
            NPCStartAttack();
    }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
    public void ResolveAttackHit()
    {
        Debug.Log("Attack Hit!");
    }
    private void BeginAnimatedAttack()
    {
        shouldAnimateAttack = true;
        if (isPlayer)
        {
            attackFinished = false;
            shouldAnimateAttack = true;
            isAttacking = true;
            attackIsCanceled = false;
        }
        else if (!isPlayer)
            NPCStartAttack();
    }
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

    [Command]
    public void CmdCalculateAttackSpeed()
    {
        baseAttackTimeAndBackSwing = (1 / (1 / initialBaseAttackTimeAndBackSwing * attackspeed));
        baseAttackTime = .8f * baseAttackTimeAndBackSwing;
        attackBackswing = .2f * baseAttackTimeAndBackSwing;

        //if (hasAnimator)
        //    animator.SetFloat("speedMultiplier", attackspeed);
    }
    private void CalculateAttackSpeed()
    {
        baseAttackTimeAndBackSwing = (1/(1/initialBaseAttackTimeAndBackSwing * attackspeed));
        baseAttackTime = .5f * baseAttackTimeAndBackSwing;
        attackBackswing = .5f * baseAttackTimeAndBackSwing;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        
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
<<<<<<< Updated upstream


            
            TargetValid = true;
            

=======
            TargetValid = true;
        }
        else if (currentTarget != null && distanceFromTarget < attackRange && isAttacking && timeSinceLastSuccessfulAttack > attackBackswing && attackFinished)
        {
            if (isPlayer)
            {
                StopAttack();
            }
            else
            {
                NPCStopAttack();
            }
            TargetValid = false;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }
        else if (currentTarget != null && distanceFromTarget < attackRange && isAttacking)
        {
            TargetValid = false;
        }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        else if(currentTarget != null && distanceFromTarget > attackRange)
        {
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
            
            isAttacking = false;
            attackIsCanceled = true;
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        else if (currentTarget != null && distanceFromTarget > attackRange)
        {
            if (isPlayer && isAttacking)
            {
                StopAttack();
            }
            else if(!isPlayer && isAttacking)
            {
                NPCStopAttack();
            }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            TargetValid = false;
        }
        else if(target == null)
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            if (hasAnimator)
            {
                animator.SetBool("IsAttacking", false);
            }
            attackIsCanceled = true;

=======
            if (isPlayer)
            {
=======
            if (isPlayer)
            {
>>>>>>> Stashed changes
=======
            if (isPlayer)
            {
>>>>>>> Stashed changes
                StopAttack();
            }
            else
            {
                NPCStopAttack();
            }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            TargetValid = false;
        }
        else
        {
            TargetValid = false;
        }
        if (walking)
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
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
=======
            if (isPlayer)
            {
=======
            if (isPlayer)
            {
>>>>>>> Stashed changes
=======
            if (isPlayer)
            {
>>>>>>> Stashed changes
                StopAttack();
            }
            else
            {
                NPCStopAttack();
            }
            TargetValid = false;
        }
        return TargetValid;
    }

    private void NPCStopAttack()
    {
        //Debug.Log(name + " NPCStopAttack");
        isAttacking = false;
        timeSinceLastSuccessfulAttack = attackBackswing;
        attackFinished = true;
        incrementAttackTimer = false;
        attackTimer = 0f;
    }
    
    //public void AnimateAttack()
    //{
    //    attackFinished = false;
    //    shouldAnimateAttack = true;
    //    isAttacking = true;
    //    attackIsCanceled = false;
    //}
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
        //Debug.Log("StopAnimateAttack called");
        shouldAnimateAttack = false;
    }
    [Command]
    public void StopAnimateAttackServer()
    {
        //Debug.Log("StopAnimateAttack called");
        shouldAnimateAttack = false;
    }
    public void StopAttack()
>>>>>>> Stashed changes
    {
        //if (!isServer)
        //    return;

<<<<<<< Updated upstream
        
        currentTarget.GetComponent<Health>().currentHealth += healthChange;
=======
        Debug.Log("StopAttack called");
        attackIsCanceled = true;
        isAttacking = false;
        shouldAnimateAttack = false;
        attackFinished = true;
    }
    [Command]
    public void StopAttackServer()
    {
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
            animator.SetBool("IsAttacking", true);
=======
            Debug.Log("AnimateAttack called");
            //AnimateAttack();
            AnimateAttackServer();
>>>>>>> Stashed changes
        }
        Debug.Log("Attack Coroutine Started: " + coroutineCount);

        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);



        if (distanceFromTarget < attackRange && !walking && currentTarget == target && target.activeSelf && gameObject.activeSelf && !attackIsCanceled)
        {
            Debug.Log(name + " attacked successfully");
            if (isPlayer)
            {
<<<<<<< Updated upstream
                CmdModifyHealth(currentTarget, damage);


=======
                CmdModifyHealth(currentTarget, damage, 0f);
                Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            }
            if (!isPlayer && isServer)
            {
                currentTarget.GetComponent<Health>().currentHealth += damage;
            }

        }

<<<<<<< Updated upstream
        isAttacking = false;

=======
    private void NPCStartAttack()
    {
        //Debug.Log(name + " NPCStartAttack");
        isAttacking = true;
        attackFinished = false;
        incrementAttackTimer = true;
>>>>>>> Stashed changes
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

    


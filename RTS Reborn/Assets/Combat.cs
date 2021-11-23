using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private PlayerScript player;
    private NPCController npcController;

    public float attackspeed;

    public GameObject target;

    public float initialBaseAttackTimeAndBackSwing;

    [SerializeField]
    public float baseAttackTimeAndBackSwing;


    public float baseAttackTime;


    public float attackBackswing;

    public float damage;

    [SerializeField]
    public float attackRange;
    [SerializeField]
    public float baseAttackRange;

    public float momentumAttackSpeedValue;

    public float distanceFromTarget;

    public bool walking;

    public bool isAttacking;

    public bool attackFinished;
    private bool incrementAttackTimer;
    private bool hasAnimator;

    public bool attackIsCanceled;
    public bool isPlayer;
    private int coroutineCount = 0;
    public Animator animator;

    public float attackTimer;

    IEnumerator AttackCoroutine;

    public bool shouldAnimateAttack;


    public float timeSinceLastSuccessfulAttack;



    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PlayerScript>())
        {
            player = GetComponent<PlayerScript>();
            isPlayer = true;
        }
        if (GetComponent<NPCController>())
        {
            npcController = GetComponent<NPCController>();
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

            //attackTimer += Time.deltaTime;

            if (incrementAttackTimer)
            {
                attackTimer += Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopAttack();
            StopAttackServer();
            StopAnimateAttack();
            StopAnimateAttackServer();
        }
        if (Input.GetKey(KeyCode.Q))
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
        baseAttackTimeAndBackSwing = (1 / (1 / initialBaseAttackTimeAndBackSwing * attackspeed));
        baseAttackTime = .8f * baseAttackTimeAndBackSwing;
        attackBackswing = .2f * baseAttackTimeAndBackSwing;

        if (hasAnimator)
            animator.SetFloat("speedMultiplier", attackspeed);
    }

    public void ModifyAttackSpeed(float attackspeedChange)
    {
        attackspeed += attackspeedChange;
    }

    private void StartAttack()
    {

        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            if (isPlayer)
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
            if (isPlayer)
            {

                //StopAttack();
                StopAttackServer();
                Debug.Log(gameObject.name + " Finished attack - Code 2");
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
            if (isPlayer && isAttacking)
            {

                //StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                Debug.Log(gameObject.name + " Error: Target too far - Code 4");
            }
            else if (!isPlayer && isAttacking)
            {
                NPCStopAttack();
                Debug.Log("Line 249");
            }




            TargetValid = false;

        }
        else if ((target == null && isAttacking) || (target == null && shouldAnimateAttack))
        {
            if (isPlayer)
            {

                //StopAttack();
                StopAttackServer();
                StopAnimateAttack();
                StopAnimateAttackServer();
                Debug.Log(gameObject.name + " Error: No target - Code 5");
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
            if (isPlayer)
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
    public void StopAttackServer()
    {

        Debug.Log("StopAttackServer called");
        attackIsCanceled = true;
        isAttacking = false;
        //timeSinceLastSuccessfulAttack = attackBackswing;
        attackFinished = true;
    }
    public void CmdModifyHealth(GameObject currentTarget, float damage, float healing)
    {
        currentTarget.GetComponent<Health>().currentHealth -= damage;
        currentTarget.GetComponent<Health>().currentHealth += healing;
    }

    private IEnumerator Attack()
    {
        Debug.Log($" {name} Is player: {isPlayer}");
        if (isPlayer)
        {
            Debug.Log("AnimateAttack called");
            AnimateAttack();
            AnimateAttackServer();
        }
        else if (!isPlayer)
            NPCStartAttack();
        GameObject currentTarget = target;
        yield return new WaitForSeconds(baseAttackTime);

        if (distanceFromTarget < attackRange && !walking && currentTarget == target && target.activeSelf && gameObject.activeSelf && !attackIsCanceled)
        {

            if (isPlayer)
            {
                CmdModifyHealth(currentTarget, damage, 0f);
            }
            if (!isPlayer)
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

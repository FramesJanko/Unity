using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour
{
    
    public int towerSize;
    public Vector3 scale;
    public int attackDamage;
    public GameObject attackProjectile;
    public Transform attackTarget;
    Collider[] hits;
    //float[] potentialTargets;
    GameObject currentAttack;
    public float attackRange;
    public LayerMask targetMask;
    bool shouldAttack;
    public float attackTime;
    bool invoked;
    bool attackCanceled;
    public float timeSinceAttack;

    // Start is called before the first frame update
    void Awake()
    {
        scale = transform.localScale;
        shouldAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldAttack)
        {
            if (attackTarget != null && !invoked && timeSinceAttack >= attackTime)
            {
                InvokeRepeating("Attack", 0f, attackTime);
                invoked = true;
            }
        }
        if (!shouldAttack && attackTarget != null && !attackCanceled)
        {
            CancelAttack();
        }
            
        if ((attackTarget == null || !attackTarget.gameObject.activeSelf) && !attackCanceled)
        {
            CancelAttack();
        }
        timeSinceAttack += Time.deltaTime;

    }

    private void CancelAttack()
    {
        attackTarget = null;
        CancelInvoke("Attack");
        invoked = false;
        attackCanceled = true;
        CheckForTarget();
    }

    
    public void SetTowerSize()
    {
        transform.localScale = scale * towerSize;
    }
    public void CheckForTarget()
    {

        Debug.Log("Checking for target.");
        hits = Physics.OverlapSphere(transform.position, attackRange, targetMask);
        //potentialTargets = new float[hits.Length];
        if(hits.Length > 0)
        {
            if (hits.Length == 1)
            {
                attackTarget = hits[0].transform;
            }
            else
            {
                float lowestDistance = attackRange * attackRange * 1.01f;
                int indexOfTarget = 0;
                for (int i = 0; i < hits.Length; i++)
                {
                    
                    float x = Mathf.Abs(hits[i].transform.position.x - transform.position.x);
                    float z = Mathf.Abs(hits[i].transform.position.z - transform.position.z);
                    float distance = x * x + z * z;
                    if(distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        indexOfTarget = i;
                    }
                }
                //Array.Sort(potentialTargets);
                attackTarget = hits[indexOfTarget].transform;
            }
            attackCanceled = false;
        }


    }
    public void Attack()
    {
        Debug.Log("Attacking");
        float distanceToTarget = Vector3.Distance(attackTarget.transform.position, transform.position);
        if(attackTarget != null && distanceToTarget <= attackRange)
        {
            currentAttack = Instantiate(attackProjectile, transform.position + Vector3.up*4.15f, Quaternion.identity, transform.parent);
            AttackProjectile projectile = currentAttack.GetComponent<AttackProjectile>();
            projectile.SetTarget(attackTarget, gameObject);
            projectile.AssignDamage(attackDamage);
            projectile.origin = gameObject;
            timeSinceAttack = 0f;
        }
        if (distanceToTarget > attackRange)
        {
            attackTarget = null;
        }
        
    }
}

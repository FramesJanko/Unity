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
    float[] potentialTargets;
    GameObject currentAttack;
    public float attackRange;
    public LayerMask targetMask;

    // Start is called before the first frame update
    void Awake()
    {
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
            CheckForTarget();

        //Attack(attackTarget);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
    public void SetTowerSize()
    {
        transform.localScale = scale * towerSize;
    }
    void CheckForTarget()
    {
        Debug.Log("Checking for targets");
        hits = Physics.OverlapSphere(transform.position, attackRange, targetMask);
        potentialTargets = new float[hits.Length];
        if(hits.Length > 0)
        {
            if (potentialTargets.Length == 1)
            {
                attackTarget = hits[0].transform;
            }
            else
            {
                for (int i = 0; i < potentialTargets.Length; i++)
                {
                    
                    float x = Mathf.Abs(hits[i].transform.position.x - transform.position.x);
                    float z = Mathf.Abs(hits[i].transform.position.z - transform.position.z);
                    float distance = x * x + z * z;
                    potentialTargets[i] = distance;
                }
                Array.Sort(potentialTargets);
                Debug.Log($"{potentialTargets[0]}");
            }
        }


    }
    public void Attack(Transform target)
    {
        if (attackTarget != null)
        {
            currentAttack = Instantiate(attackProjectile, transform.position, Quaternion.identity, transform.parent);
            currentAttack.GetComponent<AttackProjectile>().SetTarget(target);
        }
        
    }
}

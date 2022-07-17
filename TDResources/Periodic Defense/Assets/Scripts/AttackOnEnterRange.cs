using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnEnterRange : MonoBehaviour
{
    Transform parent;
    private void Awake()
    {
        parent = transform.parent;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        BasicTower basicTowerScript = parent.GetComponent<BasicTower>();
        if(basicTowerScript.attackTarget == null)
            basicTowerScript.CheckForTarget();
    }
}

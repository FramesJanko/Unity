using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonitorAttack : MonoBehaviour
{
    Combat _combat;

    [SerializeField]
    Image attackBar;

    void Awake()
    {
        _combat = GetComponent<Combat>();
    }

    // Update is called once per frame
    void Update()
    {
        attackBar.fillAmount = _combat.attackTimer / _combat.baseAttackTimeAndBackSwing;
    }
}

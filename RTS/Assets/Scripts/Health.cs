using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SerializeField] [SyncVar]
    public float maxHealth = 100;

    [SyncVar]
    public float currentHealth;

    public delegate void OnHealthPercentChanged(float currentHealthPercent);

    public event OnHealthPercentChanged EventHealthChanged;


    

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    //public void ModifyHealth(float healthChange)
    //{
    //    CmdModifyHealth(healthChange);
    //}
    //[Command]
    public void CmdModifyHealth(float healthChange)
    {
        if (!isServer)
            return;

        currentHealth += healthChange;

        float currentHealthPercent = currentHealth / maxHealth;
        GetComponentInChildren<Healthbar>().UpdateCurrentHealthPercent(currentHealthPercent);
        
    }

    //[Command]
    //public void CmdModifyHealth(float damage) => ModifyHealth(damage);

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
        
        
    }

    

}

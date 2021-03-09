using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SerializeField] [SyncVar]
    private float maxHealth = 100;

    [SerializeField] [SyncVar]
    private float currentHealth;

    public delegate void OnHealthPercentChanged(float currentHealthPercent);

    public event OnHealthPercentChanged EventHealthChanged;


    

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    
    public void ModifyHealth(float healthChange)
    {
        if (!isServer)
            return;

        currentHealth += healthChange;

        float currentHealthPercent = (float)currentHealth / (float)maxHealth;
        EventHealthChanged?.Invoke(currentHealthPercent);
        CheckForDeath();
    }

    [Command]
    public void CmdModifyHealth(float damage) => ModifyHealth(damage);

    private void Awake()
    {
        CheckForDeath += Remove;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!hasAuthority) { return; }
            CmdModifyHealth(-10);
        }
        
    }

    public event Action CheckForDeath = delegate { };
    
    public void Remove()
    {
        
        if (currentHealth <= 0f)
        {
            Player[] playerList = GetComponents<Player>();
            foreach(Player p in playerList)
            {
                if (p.target == gameObject)
                {
                    p.target = null;
                }
            }
            Destroy(gameObject);
        }
    }

}

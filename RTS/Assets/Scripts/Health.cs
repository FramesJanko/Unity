using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class Health : NetworkBehaviour
{
    [SerializeField] [SyncVar]
    public float maxHealth = 100;

    [SyncVar]
    public float currentHealth;

    public delegate void OnHealthPercentChanged(float currentHealthPercent);
    public List<Player> playersInExpRange;
    public event OnHealthPercentChanged EventHealthChanged;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }
    
    public void CmdModifyHealth(float healthChange)
    {
        if (!isServer)
            return;

        currentHealth += healthChange;

        float currentHealthPercent = currentHealth / maxHealth;
        GetComponentInChildren<Healthbar>().UpdateCurrentHealthPercent(currentHealthPercent);
        
    }

    void Update()
    {
        CheckForDeath();

    }

    private void CheckForDeath()
    {
        if(currentHealth <= 0)
        {
            Death();
        }
    }
    
    private void Death()
    {
        Player[] playerList = GetComponents<Player>();
        foreach (Player p in playerList)
        {
            if (p.target == gameObject)
            {
                p.target = null;
            }
        }
        GetListofPlayersInRange();
        AwardExperience();
        gameObject.SetActive(false);
    }
    public void GetListofPlayersInRange()
    {
        playersInExpRange = FindObjectsOfType<Player>().ToList();
        foreach (Player p in playersInExpRange)
        {
            if (Vector3.Distance(transform.position, p.transform.position) > GetComponent<NpcController>().experianceRange)
            {
                playersInExpRange.Remove(p);
            }
        }
    }

    public void AwardExperience()
    {
        foreach (Player p in playersInExpRange)
        {
            if(playersInExpRange.Count() == 1)
            {
                p.totalExperience += GetComponent<NpcController>().experiencePool;
            }
            else if (playersInExpRange.Count() > 1)
            {
                p.totalExperience += ((GetComponent<NpcController>().experiencePool / playersInExpRange.Count() + 10));
            }
        }
    }
}

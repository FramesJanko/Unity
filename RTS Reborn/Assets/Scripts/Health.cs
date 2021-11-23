using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Health : MonoBehaviour
{
    [SerializeField]
    public float maxHealth = 100;

    public float currentHealth;

    public delegate void OnHealthPercentChanged(float currentHealthPercent);
    public PlayerScript[] playersList;
    public int playersInExpRange;

    public event OnHealthPercentChanged EventHealthChanged;

    [SerializeField]
    public float experianceRange;

    [SerializeField]
    private Healthbar childHealthbar;
    public int experiencePool;
    bool isPlayer;
    //StatAttributes _sa;
    //private LootTable _lootTable;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        if (GetComponent<PlayerScript>())
        {
            isPlayer = true;
            //_sa = GetComponent<StatAttributes>();
        }
        //if (GetComponent<LootTable>())
        //    _lootTable = GetComponent<LootTable>();
    }
    private void Awake()
    {

    }

    public void CmdModifyHealth(float healthChange)
    {
        currentHealth += healthChange;

        float currentHealthPercent = currentHealth / maxHealth;
        childHealthbar.UpdateCurrentHealthPercent(currentHealthPercent);

    }

    void Update()
    {
        CheckForDeath();

    }

    private void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        PlayerScript[] playerList = FindObjectsOfType<PlayerScript>();
        foreach (PlayerScript p in playerList)
        {
            if (p.target == gameObject)
            {
                p.target = null;
            }
        }
        //_lootTable.DropLoot();
        GetListofPlayersInRange();
        //AwardExperience();
        gameObject.SetActive(false);

    }
    public void GetListofPlayersInRange()
    {
        playersList = FindObjectsOfType<PlayerScript>();


        if (playersList.Length > 0)
        {


            foreach (PlayerScript p in playersList)
            {

                if (Vector3.Distance(transform.position, p.transform.position) <= experianceRange)
                {
                    playersInExpRange++;

                }


            }
            foreach (PlayerScript p in playersList)
            {

                if (Vector3.Distance(transform.position, p.transform.position) <= experianceRange)
                {

                    AwardExperience(p);
                }


            }
            Debug.Log("Players in experience range: " + playersInExpRange);
            playersInExpRange = 0;
        }

    }

    public void AwardExperience(PlayerScript p)
    {
        if (playersInExpRange == 1)
            p.totalExperience += experiencePool;

        if (playersInExpRange > 1)
            p.totalExperience += ((experiencePool / playersInExpRange) + 5);

        //p.GetComponent<StatAttributes>().CheckLevelUp();
    }

}

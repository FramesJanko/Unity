using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAttributes : NetworkBehaviour
{
    [SyncVar]
    public float Strength;
    [SyncVar]
    public float Agility;
    [SyncVar]
    public float Intelligence;
    public Combat _combat;
    public Player _player;
    [SyncVar]
    public int Level;
    
    public List<int> experienceThresholds;
    [SerializeField]
    public double statGainStr;
    [SerializeField]
    public double statGainAgi;
    [SerializeField]
    public double statGainInt;
    

    void Awake()
    {
        
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            _combat = GetComponent<Combat>();
            _player = GetComponent<Player>();
            Strength = 5f;
            Agility = 5f;
            Intelligence = 5f;
            experienceThresholds.Add(0);
            experienceThresholds.Add(40);
            experienceThresholds.Add(80);
            experienceThresholds.Add(130);
            experienceThresholds.Add(180);
            experienceThresholds.Add(200);
            experienceThresholds.Add(300);
            experienceThresholds.Add(380);
            experienceThresholds.Add(480);
            experienceThresholds.Add(590);
            experienceThresholds.Add(800);
        }
        SetStats();
    }
    [Server]
    private void SetStats()
    {
        _combat.damage += Strength;
        _combat.attackspeed += .01f * Agility;
        
        //Mana not yet implemented;
        //_combat.manapool += Intelligence * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckLevelUp()
    {
        if (_player.totalExperience >= experienceThresholds[Level])
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        
        if(Level < experienceThresholds.Count)
        {
            Level++;
            Strength += (float)statGainStr;
            Agility += (float)statGainAgi;
            Intelligence += (float)statGainInt;
            _combat.damage += (float)statGainStr;
            _combat.attackspeed += (float)statGainAgi * .01f;
        }
        
    }
}

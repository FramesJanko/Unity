using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootTable : NetworkBehaviour
{
    int[] dropChances = new[] { 10, 20, 30, 40 };
    string[] dropChanceNames = new[] { "legendary", "epic", "rare", "common" };

    [SerializeField]
    private GameObject lootPrefab;

    private void Awake()
    {
        
    }

    public void DropLoot()
    {
        float lootRoll = Random.Range(0f, 1f);
        if (lootRoll > .5f)
        {
            GameObject loot = Instantiate(lootPrefab, transform.position, lootPrefab.transform.rotation);
            NetworkServer.Spawn(loot);
        }
            
    }
}

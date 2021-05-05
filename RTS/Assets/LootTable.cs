using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootTable : MonoBehaviour
{
    int[] dropChances = new[] { 10, 20, 30, 40 };
    private Text rarityName;
    private int dropChance;
    string[] dropChanceNames = new[] { "legendary", "epic", "rare", "common" };

    // Start is called before the first frame update
    void Start()
    {
         foreach(int chance in dropChances)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropLoot()
    {

    }
}

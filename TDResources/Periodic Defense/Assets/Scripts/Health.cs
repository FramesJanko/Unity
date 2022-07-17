using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHp;
    public int currentHp;
    public int damage;
    public GameObject attacker;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
            
    }

    float CalculatePercentHp()
    {
        return (float)currentHp / (float)maxHp;
    }

    public void UpdateHp(int change, GameObject _attacker)
    {
        attacker = _attacker;
        currentHp += change;
        float hpPercent = CalculatePercentHp();
        transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().fillAmount = hpPercent;
        if(hpPercent <= 0)
        {
            GetComponent<Unit>().Death();
        }
    }
}

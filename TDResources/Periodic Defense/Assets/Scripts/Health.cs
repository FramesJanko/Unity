using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHp;
    public int currentHp;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHp--;
            UpdateHp();
        }
            
    }

    float CalculatePercentHp()
    {
        return (float)currentHp / (float)maxHp;
    }

    public void UpdateHp()
    {
        float hpPercent = CalculatePercentHp();
        transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().fillAmount = hpPercent;
        if(hpPercent <= 0)
        {
            GetComponent<Unit>().Death();
        }
    }
}

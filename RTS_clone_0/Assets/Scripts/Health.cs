using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;

    private float currentHealth;

    public event Action<float> OnHealthPercentChanged = delegate { };

    

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void ModifyHealth(float healthChange)
    {
        currentHealth += healthChange;

        float currentHealthPercent = (float)currentHealth / (float)maxHealth;
        OnHealthPercentChanged(currentHealthPercent);
        CheckForDeath();
    }

    private void Awake()
    {
        CheckForDeath += Remove;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ModifyHealth(-10);
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

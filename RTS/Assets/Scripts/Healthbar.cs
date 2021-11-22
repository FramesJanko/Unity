using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Healthbar : NetworkBehaviour
{
    [SerializeField]
    private float healthChangeTimer;

    [SerializeField]
    private Image healthbarImage;

    public float currentHealthPercent;

    [SerializeField]
    private Transform rotationBase;

    [SerializeField]
    private Health parentHealth;
    private void OnEnable()
    {
        //GetComponentInParent<Health>().EventHealthChanged += CmdChangeHealthbar;
        currentHealthPercent = healthbarImage.fillAmount;
    }

    
    private void CmdChangeHealthbar(float percent)
    {
        StartCoroutine(HealOrDamage(percent));
    }
    
    public void HealOrDamageWithoutEnumerator()
    {
        float preChangePercent = healthbarImage.fillAmount;
        float timeElapsed = healthChangeTimer;
        
        if (timeElapsed < 0)
        {
            timeElapsed = 0;
        }

        if (preChangePercent != currentHealthPercent && timeElapsed >= 0)
        {
            timeElapsed -= Time.deltaTime;
            healthbarImage.fillAmount = currentHealthPercent + (preChangePercent - currentHealthPercent) * (timeElapsed / healthChangeTimer);
            //Mathf.Lerp(preChangePercent, percent, timeElapsed / healthChangeTimer);
        }
    }

    private IEnumerator HealOrDamage(float percent)
    {
        float preChangePercent = healthbarImage.fillAmount;
        float timeElapsed = 0f;

        while (timeElapsed < healthChangeTimer)
        {
            timeElapsed += Time.deltaTime;
            healthbarImage.fillAmount = Mathf.Lerp(preChangePercent, percent, timeElapsed / healthChangeTimer);
            yield return null;
        }

        healthbarImage.fillAmount = percent;

    }
    
    public void UpdateCurrentHealthPercent(float percent)
    {
        currentHealthPercent = percent;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        healthbarImage.fillAmount = parentHealth.currentHealth / parentHealth.maxHealth;
        //HealOrDamageWithoutEnumerator();
    }
    void LateUpdate()
    {
        var targetPosition = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
        transform.LookAt(targetPosition);
        transform.Rotate(90, 0, 0);
        
        
        
    }
}

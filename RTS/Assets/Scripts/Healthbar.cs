﻿using System.Collections;
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

    private void OnEnable()
    {
        GetComponentInParent<Health>().EventHealthChanged += ChangeHealthbar;
    }

    [ClientCallback]
    private void ChangeHealthbar(float percent)
    {
        StartCoroutine(HealOrDamage(percent));
    }
    [ClientCallback]
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.LookAt(Camera.main.transform);
        //transform.Rotate(0, 180, 0);
    }
}

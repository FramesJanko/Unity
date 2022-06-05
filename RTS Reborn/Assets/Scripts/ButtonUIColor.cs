using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonUIColor : MonoBehaviour
{
    public GameObject currentSpell;
    public Image thisImage;
    public Button thisButton;
    public Color readyColor;
    public int fadeColor;
    public Blink blink;

    // Start is called before the first frame update
    void Start()
    {
        readyColor = thisImage.color;
        blink = currentSpell.GetComponent<Blink>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blink.cooldownReady == false)
        {
            thisImage.fillAmount = 1 - (blink.currentCooldown / blink.cooldown);
        }
        else
        {
            thisImage.fillAmount = 1f;
        }
        
    }
}

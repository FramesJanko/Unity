using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int text;
    

    // Update is called once per frame
    void Update()
    {
        text = Mathf.RoundToInt(Time.time);
        GetComponent<Text>().text = text.ToString();
    }
}

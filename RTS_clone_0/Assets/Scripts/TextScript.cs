using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar]
    public int time;
    
    private int timeToSubtract;

    [SyncVar]
    public bool readyToShowTime;
    public bool needToSetFirstTime;



    // Update is called once per frame
    void Update()
    {
        if (isServer)
            SetTime();

        if (readyToShowTime)
            ShowTime();

    }

    private void ShowTime()
    {
        GetComponent<Text>().text = time.ToString();
    }

    [Server]
    private void SetTime()
    {
        if (needToSetFirstTime)
        {
            timeToSubtract = Mathf.RoundToInt(Time.time);
            needToSetFirstTime = false;
        }
        time = Mathf.RoundToInt(Time.time) - timeToSubtract;
        
    }
    
}

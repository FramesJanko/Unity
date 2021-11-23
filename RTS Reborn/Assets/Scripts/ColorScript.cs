using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScript : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Color oldColor;
    public Color newColor;
    public float timeTracker;
    public bool trackingTime;
    public float timeForNewColor;
    public bool itsTimeForNewColor;
    public Color[] colorPie = new Color[5];
    private int colorPieCounter;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        oldColor = _meshRenderer.material.color;
    }

    private void Update()
    {
        if (trackingTime)
        {
            timeTracker += Time.deltaTime;
            if(timeTracker > timeForNewColor)
            {
                itsTimeForNewColor = true;
            }
            if(itsTimeForNewColor)
            {
                
                oldColor = newColor;
                newColor = colorPie[colorPieCounter];
                colorPieCounter++;
                if (colorPieCounter > 4)
                    colorPieCounter = 0;
                timeTracker = 0f;
                itsTimeForNewColor = false;
            }
        }
        _meshRenderer.material.color = Color.Lerp(oldColor, newColor, timeTracker / timeForNewColor);
    }
    private void OnCollisionEnter(Collision collision)
    {
        trackingTime = true;
        //newColor = Random.ColorHSV();
        //_meshRenderer.material.color = newColor;
    }
    
}

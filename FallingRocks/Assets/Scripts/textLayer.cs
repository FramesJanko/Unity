using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textLayer : MonoBehaviour
{
    [SerializeField]
    private string layer = "Default";
    [SerializeField]
    private int sortOrder = 11;

    private void Start()
    {
        GetComponent<Renderer>().sortingLayerName = "Death";
        GetComponent<Renderer>().sortingOrder = -20;

    }

    public void DeathScreen()
    {
        GetComponent<Renderer>().sortingLayerName = layer;
        GetComponent<Renderer>().sortingOrder = sortOrder;
    }

    public void ResetStatus()
    {
        GetComponent<Renderer>().sortingLayerName = "Death";
        GetComponent<Renderer>().sortingOrder = -20;
    }

}

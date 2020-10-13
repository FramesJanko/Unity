using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBackground : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public string layer = "Default";
    [SerializeField]
    private int order = 10;

    private void Start()
    {
        GetComponent<Renderer>().sortingLayerName = "Death";
        GetComponent<Renderer>().sortingOrder = -20;

    }
    public void DeathBg()
    {
        GetComponent<Renderer>().sortingLayerName = layer;
        GetComponent<Renderer>().sortingOrder = order;
    }
    public void ResetStatus()
    {
        GetComponent<Renderer>().sortingLayerName = "Death";
        GetComponent<Renderer>().sortingOrder = -20;
    }
}

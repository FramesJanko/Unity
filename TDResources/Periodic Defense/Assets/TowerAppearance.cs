using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAppearance : MonoBehaviour
{
    public List<Material> materials;

    private void Awake()
    {
        

    }

    public void Prebuild()
    {
        Debug.Log(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            Color materialColor = transform.GetChild(i).GetComponent<Renderer>().material.color;
            materialColor = new Color(materialColor.r, materialColor.g, materialColor.b, .5f);
            transform.GetChild(i).GetComponent<Renderer>().material.color = materialColor;
        }
        
    }
    public void Build()
    {
        Debug.Log(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            Color materialColor = transform.GetChild(i).GetComponent<Renderer>().material.color;
            materialColor = new Color(materialColor.r, materialColor.g, materialColor.b, 1f);
            transform.GetChild(i).GetComponent<Renderer>().material.color = materialColor;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

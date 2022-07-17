using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAppearance : MonoBehaviour
{
    public List<Material> materials;
    public Vector3 scale;
    public bool prebuilt;

    private void Awake()
    {
        
        scale = transform.localScale;
    }

    public void Prebuild()
    {

        GetComponent<BasicTower>().SetTowerSize();

        for (int i = 0; i < transform.childCount; i++)
        {
            Color materialColor = new Color();
            Renderer childRenderer = new Renderer();
            transform.GetChild(i).TryGetComponent<Renderer>(out childRenderer);
            if (childRenderer != null)
            {
                materialColor = childRenderer.material.color;
                materialColor = new Color(materialColor.r, materialColor.g, materialColor.b, .5f);
                childRenderer.material.color = materialColor;
            }
            
            prebuilt = true;
            
        }
        
    }
    public void Build()
    {
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Color materialColor = new Color();
            Renderer childRenderer = new Renderer();
            transform.GetChild(i).TryGetComponent<Renderer>(out childRenderer);
            if (childRenderer != null)
            {
                materialColor = childRenderer.material.color;
                materialColor = new Color(materialColor.r, materialColor.g, materialColor.b, 1f);
                childRenderer.material.color = materialColor;
            }
            
        }
        prebuilt = false;

    }

    
}

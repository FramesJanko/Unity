using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electronegativity : MonoBehaviour
{
    public int towerRange;
    public bool needToUpdateTowerRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (needToUpdateTowerRange)
        {
            GetComponent<SphereCollider>().radius = (transform.localScale.x*2) + towerRange;
            needToUpdateTowerRange = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Electronegativity Script: Trigger Enter");
    }
}

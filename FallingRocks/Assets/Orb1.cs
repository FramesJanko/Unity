using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb1 : MonoBehaviour
{
    [SerializeField]
    private GameObject[] orbPrefabs;

    void Start()
    {
        GameObject orb = Instantiate(orbPrefabs[0], transform.GetChild(2)) as GameObject;
        GameObject orb2 = Instantiate(orbPrefabs[1], transform.GetChild(2)) as GameObject;
        GameObject orb3 = Instantiate(orbPrefabs[2], transform.GetChild(2)) as GameObject;
    }

    
}

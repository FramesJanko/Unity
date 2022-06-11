using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public GameObject basicElemental;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    int waveSize;
    [SerializeField]
    float spawnInterval;
    [SerializeField]
    Transform targetPos;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //March _march = new March(gameObject);
            //March[] _array = new March[] { _march };
            SphereCreator _sc = gameObject.AddComponent<SphereCreator>();
            _sc.Initialize(spawnInterval, waveSize, /*_array, */basicElemental, spawnPoint.position, targetPos);


            Debug.Log("Spawned");
        }
    }
}

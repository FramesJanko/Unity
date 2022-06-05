using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCreator : MonoBehaviour
{
    [SerializeField]
    bool invoked;
    public GameObject prefab;
    [SerializeField]
    int spawnsRemaining;
    [SerializeField]
    float spawnTime;
    March[] sphereTypes;
    [SerializeField]
    Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float timeSinceSpawn = Time.time % spawnTime;
        //if (!recentlySpawned)
        //{
        if (!invoked)
        {
            InvokeRepeating("SpawnBall", 2f, .1f);
            invoked = true;
        }
        else if(spawnsRemaining <= 0)
        {
            CancelInvoke("SpawnBall");
        }


        if (/*timeSinceSpawn > spawnTime*.95 && */spawnsRemaining <= 0)
        {

            //recentlySpawned = true;
            //Debug.Log(timeSinceSpawn);
        }

        //}

        //if (recentlySpawned)
        //{
        //    if (timeSinceSpawn < .1)
        //    {
        //        recentlySpawned = false;
        //    }
        //}
    }

    private void SpawnBall()
    {
        GameObject _newSphere = Instantiate(prefab, transform);
        _newSphere.transform.position = spawnPoint;
        _newSphere.GetComponent<March>().SetMap(gameObject);

        spawnsRemaining--;
    }

    public void Initialize(float _spawnTime, int _spawnsRemaining, March[] _sphereTypes, GameObject basicElemental, Vector3 _spawnPoint)
    {
        spawnTime = _spawnTime;
        spawnsRemaining = _spawnsRemaining;
        sphereTypes = _sphereTypes;
        prefab = basicElemental;
        spawnPoint = _spawnPoint;
    }
}

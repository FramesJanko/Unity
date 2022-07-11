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
    [SerializeField]
    Transform targetPos;
    GameplayManager _gameplayerManager;

    // Start is called before the first frame update
    void Awake()
    {
        _gameplayerManager = GetComponent<GameplayManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!invoked)
        {
            InvokeRepeating("SpawnBall", .5f, spawnTime);
            invoked = true;
        }
        else if(spawnsRemaining <= 0)
        {
            CancelInvoke("SpawnBall");
        }
    }

    private void SpawnBall()
    {
        GameObject _newSphere = Instantiate(prefab, transform);
        _newSphere.transform.position = spawnPoint;
        //_newSphere.GetComponent<March>().SetMap(gameObject);
        _newSphere.GetComponent<Unit>().target = targetPos;
        _newSphere.GetComponent<Unit>().BeginPath();
        _gameplayerManager.spawnedUnits.Add(_newSphere);
        spawnsRemaining--;
    }

    public void Initialize(float _spawnTime, int _spawnsRemaining, GameObject _prefab, Vector3 _spawnPoint, Transform _targetPos)
    {
        spawnTime = _spawnTime;
        spawnsRemaining = _spawnsRemaining;
        prefab = _prefab;
        spawnPoint = _spawnPoint;
        targetPos = _targetPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    Vector3[] path;
    int targetIndex;
    public float electronDropChance;
    public int electronDropNumber;
    public GameObject electronPrefab;
    public float dropRange;
    public void BeginPath()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && gameObject.activeSelf)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    gameObject.SetActive(false);
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            currentWaypoint.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed*Time.deltaTime);
            yield return null;
        }
    }
    public void Death()
    {
        for (int i = 0; i < electronDropNumber; i++)
        {
            float roll = Random.value * 100;
            if (roll < electronDropChance)
            {
                //Drops();
            }

        }
            
            
        gameObject.SetActive(false);
    }
    void Drops()
    {
        GameObject electron = Instantiate(electronPrefab, transform.position + new Vector3(Random.value, 0, Random.value) * dropRange, Quaternion.identity, transform.parent);
        electron.GetComponent<Electron>().SetTarget(GetComponent<Health>().attacker.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger: Unit");
    }
}

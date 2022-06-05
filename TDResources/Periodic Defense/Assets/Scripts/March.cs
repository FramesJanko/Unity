using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class March : MonoBehaviour
{
    public Vector3[] waypoints;
    PathWaypoints path;
    public GameObject map;
    public float speed;
    int currentWaypointIndex = 0;
    bool mapSet;

    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapSet)
        {
            path = map.GetComponent<PathWaypoints>();
            waypoints = new Vector3[path.waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = path.waypoints[i].transform.position;
            }
            mapSet = true;
        }

        Vector3 currentWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentWaypoint) < .001 && currentWaypointIndex < waypoints.Length - 1)
        {
            currentWaypointIndex++;
        }
    }
    public March(GameObject _map)
    {
        map = _map;
        
    }
    public void SetMap(GameObject _map)
    {
        map = _map;
    }
}

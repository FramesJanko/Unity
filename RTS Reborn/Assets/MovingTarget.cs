using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    public float locationx;
    public float locationy;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        locationx = (Mathf.Sin(Time.realtimeSinceStartup*speed));
        locationy = (Mathf.Cos(Time.realtimeSinceStartup*speed));

        transform.position = transform.position + new Vector3(4*locationx*Time.deltaTime, 0f, 0f);
    }
}

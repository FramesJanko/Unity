using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Transform tf;
    
    void Start()
    {
        Debug.Log("y = " + Screen.height);
        Debug.Log("-y = " + Screen.height * -1);
        tf = GetComponentInParent<Transform>();
        

    }

    void Update()
    {
        if (transform.position.x != 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
}

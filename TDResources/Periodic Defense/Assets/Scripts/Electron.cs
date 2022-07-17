using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron : MonoBehaviour
{
    Rigidbody rb;
    Vector3 randomDirection;
    public float startingForce;
    Transform target;
    public float speed;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        randomDirection = Random.insideUnitSphere * startingForce;
        randomDirection = new Vector3(randomDirection.x, Mathf.Abs(randomDirection.y), randomDirection.z);
    }

    private void Start()
    {
        rb.AddForce(randomDirection);
         
    }

    // Update is called once per frame
    void Update()
    {
        //if(target != null)
        //    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.x *= speed;
        direction.y *= speed;
        direction.z *= speed;
        rb.AddForce(direction);
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public Transform target;
    public float speed;
    int damage;
    public GameObject origin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        float x = Mathf.Abs(target.position.x - transform.position.x);
        float z = Mathf.Abs(target.position.z - transform.position.z);
        float distance = x * x + z * z;
        if(distance < 0.1f)
        {
            target.GetComponent<Health>().UpdateHp(damage, origin);
            gameObject.SetActive(false);
        }
    }
    public void SetTarget(Transform _target, GameObject _origin)
    {
        target = _target;
        origin = _origin;
    }
    public void AssignDamage(int _damage)
    {
        damage = _damage;
    }
}

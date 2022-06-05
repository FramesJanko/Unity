using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    [SerializeField]
    int damage;
    [SerializeField]
    float projectileSpeed;
    [SerializeField]
    float cooldown;
    Vector3 target;
    private bool tracking;
    [SerializeField]
    GameObject daggerPrefab;
    GameObject dagger;
    GameObject trackingTarget;
    bool daggerExists;
    bool daggerReachedTarget;
    public Camera cam;
    [SerializeField]
    Animator animator;
    private AnimatorStateInfo state;
    private RaycastHit hitInfo;

    private void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ThrowDagger();
        }
        

        if (daggerExists && dagger.activeInHierarchy)
        {
            Debug.Log("Traveling");
            dagger.transform.LookAt(target);
            if (!tracking)
            {
                dagger.transform.position = Vector3.MoveTowards(dagger.transform.position, target, projectileSpeed * Time.deltaTime);
                if (dagger.transform.position == target)
                {
                    daggerReachedTarget = true;
                }
            }
            else
            {
                dagger.transform.position = Vector3.MoveTowards(dagger.transform.position, trackingTarget.transform.position, projectileSpeed * Time.deltaTime);
                if (dagger.transform.position == trackingTarget.transform.position)
                {
                    daggerReachedTarget = true;
                }
            }
            
            if (daggerReachedTarget)
            {
                dagger.SetActive(false);

            }
        }
    }

    private void EndThrowDagger()
    {
        
        
        animator.SetBool("isThrowing", false);
        if (!daggerExists)
        {
            Debug.Log("Instantiating...");

            dagger = Instantiate(daggerPrefab, transform.position, Quaternion.identity);
            daggerExists = true;
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                target = hitInfo.collider.gameObject.transform.position;
                tracking = true;
                trackingTarget = hitInfo.collider.gameObject;
            }
            else
            {
                target = hitInfo.point;
                tracking = false;
            }

        }
        if (!dagger.activeInHierarchy)
        {

            dagger.SetActive(true);
            dagger.transform.position = transform.position;
            daggerReachedTarget = false;
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                target = hitInfo.collider.gameObject.transform.position;
                tracking = true;
                trackingTarget = hitInfo.collider.gameObject;

            }
            else
            {
                target = hitInfo.point;
                tracking = false;
            }

        }
        animator.SetBool("isThrowing", false);

    
    }

    private void ThrowDagger()
    {
        animator.SetBool("isThrowing", true);
        Debug.Log("Throwing dagger");

        Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
        Vector3 direction = worldMousePosition - cam.transform.position;
        Vector3 startPosition = cam.transform.position;

        Physics.Raycast(startPosition, direction, out hitInfo, 200f);
        Debug.Log(hitInfo.collider);
    }
}

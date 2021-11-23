using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    public Vector3 movementLocation;
    Vector3 detourLocation;
    RaycastHit hit;

    [SerializeField]
    private float movespeed;
    public bool walking;

    public GameObject target;

    float destinationDistanceFromTarget;

    public float distanceFromTarget;

    public Collider destinationCollider;

    [SerializeField]
    LayerMask walkableTerrain;

    MeshRenderer lastHitMeshRenderer;

    NavMeshAgent _navMeshAgent;

    public int totalExperience;

    Camera cam;
    private CapsuleCollider _collider;

    [SerializeField]
    Combat _combat;

    // Start is called before the first frame update
    void Start()
    {
        movementLocation = transform.position;
        walking = false;
        cam = Camera.main;
        _collider = GetComponent<CapsuleCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfWalking();

        HandleMovement();



        //ShowBehindWalls();

        //if (walking)
        //{
        //    Debug.DrawLine(transform.position, movementLocation, Color.blue, .1f);

        //    //CalculateRoute();
        //}
    }
    private void LateUpdate()
    {
        transform.position = _navMeshAgent.gameObject.transform.position;
    }
    private void CalculateRoute()
    {
        if (Physics.Raycast(transform.position, movementLocation, out hit, 200f))
        {
            Debug.Log(hit.collider);

            Debug.Log(destinationCollider);
            Debug.DrawLine(transform.position, movementLocation, Color.yellow, 1f);

            //if (hit.collider != destinationCollider)
            //{
            //    Debug.DrawLine(transform.position, movementLocation, Color.yellow, 1f);

            //}

        }
    }
    private void HandleMovement()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
            Vector3 direction = worldMousePosition - cam.transform.position;
            Vector3 startPosition = cam.transform.position;

            if (Physics.Raycast(startPosition, direction, out hit, 200f, walkableTerrain))
            {
                Debug.DrawLine(startPosition, hit.point, Color.green, 0.5f);


                CheckForTarget();

                destinationCollider = hit.collider;

            }
            else
            {
                Debug.DrawLine(startPosition, worldMousePosition, Color.red, 0.5f);
            }

            destinationDistanceFromTarget = 0f;

            if (target != null)
            {
                float destinationDistanceFromTargetX = Math.Abs(movementLocation.x - target.transform.position.x);
                float destinationDistanceFromTargetZ = Math.Abs(movementLocation.z - target.transform.position.z);
                destinationDistanceFromTarget = destinationDistanceFromTargetX * destinationDistanceFromTargetX;
                destinationDistanceFromTarget += (destinationDistanceFromTargetZ * destinationDistanceFromTargetZ);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            movementLocation = transform.position;
            Deselect();
            target = null;

        }
        if (target != null)
        {
            float distanceFromTargetX = System.Math.Abs(transform.position.x - target.transform.position.x);
            float distanceFromTargetZ = System.Math.Abs(transform.position.z - target.transform.position.z);
            distanceFromTarget = distanceFromTargetX * distanceFromTargetX;
            distanceFromTarget += (distanceFromTargetZ * distanceFromTargetZ);


            if (destinationDistanceFromTarget < 7 && distanceFromTarget < 7)
            {
                movementLocation = transform.position;
            }
        }
        if (hit.collider != null && hit.collider.tag == "Loot" && Vector3.Distance(transform.position, hit.transform.position) < 2f)
        {
            movementLocation = transform.position;
        }
        if (target != null && distanceFromTarget > _combat.baseAttackRange && !_combat.isAttacking)
        {
            movementLocation = target.transform.position;
        }
        if (target != null && distanceFromTarget <= _combat.baseAttackRange * .9)
        {
            movementLocation = transform.position;
        }
        _navMeshAgent.SetDestination(movementLocation);
        //transform.position = Vector3.MoveTowards(transform.position, movementLocation, movespeed * Time.deltaTime);
    }
    private void CheckForTarget()
    {
        Debug.Log(hit.collider.tag);

        if (hit.collider.tag == "Enemy")
        {
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
            Vector3 direction = worldMousePosition - cam.transform.position;
            Vector3 startPosition = cam.transform.position;

            SetTarget(startPosition, direction);
            //Debug.Log($"Targeting {target.name}. Network ID is {target.GetComponent<NetworkIdentity>().netId}");

            
        }
        else
        {
            Debug.Log("Deselecting");
            Deselect();
            target = null;
            movementLocation = new Vector3 (hit.point.x, hit.point.y + _collider.height/2, hit.point.z);

        }
    }
    public void CheckIfWalking()
    {

        walking = (movementLocation.x != transform.position.x && movementLocation.y != transform.position.z);

    }
    public void Deselect()
    {
        Debug.Log("Running Deselect Command");
        target = null;
    }
    public void SetTarget(Vector3 startPosition, Vector3 direction)
    {
        RaycastHit targetHit;

        if (Physics.Raycast(startPosition, direction, out targetHit, 200f, walkableTerrain))
        {
            Debug.Log(targetHit.collider.name);
            target = targetHit.collider.gameObject;
        }

        //GetComponent<Combat>().target = target;
    }
}

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public GameObject player;

    Vector3 movementLocation;
    Vector3 detourLocation;

    RaycastHit hit;

    [SerializeField]
    private float movespeed;
    [SyncVar]
    public GameObject target;

    public bool walking;

    float destinationDistanceFromTarget;

    public float distanceFromTarget;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        movementLocation = transform.position;
        walking = false;

    }

    // Update is called once per frame
    void Update()
    {


        CheckIfWalking();

        HandleMovement();

        CheckIfWalking();
    }

    private void HandleMovement()
    {
        if (isLocalPlayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
                Vector3 direction = worldMousePosition - Camera.main.transform.position;

                if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 200f))
                {
                    Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green, 0.5f);
                    Debug.Log("Clicked at " + hit.point);

                    if (hit.collider.tag == "Enemy")
                    {
                        target = hit.collider.gameObject;
                        Debug.Log($"Targeting {target.name}. Network ID is {target.GetComponent<NetworkIdentity>().netId}");
                        Debug.Log(target.name + " is located at " + target.transform.position);
                        movementLocation = hit.collider.gameObject.transform.position;
                    }
                    else
                    {
                        movementLocation = hit.point + new Vector3(0f, GetComponent<MeshRenderer>().bounds.size.y / 2f, 0f);
                    }

                    Debug.Log("Going to " + movementLocation);
                }
                else
                {
                    Debug.DrawLine(Camera.main.transform.position, worldMousePosition, Color.red, 0.5f);
                }

                destinationDistanceFromTarget = 0f;

                if (target != null)
                {
                    float destinationDistanceFromTargetX = System.Math.Abs(movementLocation.x - target.transform.position.x);
                    float destinationDistanceFromTargetZ = System.Math.Abs(movementLocation.z - target.transform.position.z);
                    destinationDistanceFromTarget = destinationDistanceFromTargetX * destinationDistanceFromTargetX;
                    destinationDistanceFromTarget += (destinationDistanceFromTargetZ * destinationDistanceFromTargetZ);
                }




                Debug.Log("Destination's distance from target: " + System.Math.Sqrt(destinationDistanceFromTarget));
                //Debug.Log("Distance from target: " + System.Math.Sqrt(distanceFromTarget));



            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                movementLocation = transform.position;
            }



            if (target != null)
            {
                float distanceFromTargetX = System.Math.Abs(transform.position.x - target.transform.position.x);
                float distanceFromTargetZ = System.Math.Abs(transform.position.z - target.transform.position.z);
                distanceFromTarget = distanceFromTargetX * distanceFromTargetX;
                distanceFromTarget += (distanceFromTargetZ * distanceFromTargetZ);


                if (destinationDistanceFromTarget < 1.5 && distanceFromTarget < 5)
                {
                    //if (System.Math.Abs(movementLocation.x - target.transform.position.x) > 1.5 && System.Math.Abs(movementLocation.z - target.transform.position.z) > 1.5)
                    //{
                    //    movementLocation = hit.point + new Vector3(0f, player.GetComponent<MeshRenderer>().bounds.size.y / 2f, 0f);
                    //}
                    //else
                    //{

                    //    movementLocation = transform.position;
                    //}
                    movementLocation = transform.position;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, movementLocation, movespeed * Time.deltaTime);
        }
    }
    public void CheckIfWalking()
    {
        if (movementLocation != transform.position)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }
    }
    public void Deselect()
    {
        target = null;
    }
}

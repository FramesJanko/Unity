using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject player;
    Vector3 movementLocation;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        movementLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 200f));
            Vector3 direction = worldMousePosition - Camera.main.transform.position;

            if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 200f))
            {
                Debug.DrawLine(Camera.main.transform.position, hit.point, Color.green, 0.5f);
                Debug.Log(hit.point);
                movementLocation = hit.point + new Vector3(0f, player.GetComponent<MeshRenderer>().bounds.size.y/2f, 0f);
                
            }
            else
            {
                Debug.DrawLine(Camera.main.transform.position, worldMousePosition, Color.red, 0.5f);
            }
         
            
        }

        transform.position = Vector3.MoveTowards(transform.position, movementLocation, 15f * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float wasdModifier;
    private Vector3 startPosition;

    private Vector3 newCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            startPosition = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse2))
        {

            Vector3 changingMousePosition = Input.mousePosition;
            float timeElapsed = 0f;
            timeElapsed += Time.deltaTime;

            newCameraPosition = Camera.main.transform.position;

            newCameraPosition.x += (startPosition.x - changingMousePosition.x) / (1 / cameraSpeed);
            newCameraPosition.z += (startPosition.y - changingMousePosition.y) / (1 / cameraSpeed);


            Camera.main.transform.position = newCameraPosition;
            startPosition = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += Vector3.forward*cameraSpeed*wasdModifier*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position -= Vector3.right * cameraSpeed * wasdModifier * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position -= Vector3.forward * cameraSpeed * wasdModifier * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += Vector3.right * cameraSpeed * wasdModifier * Time.deltaTime;
        }
    }
}

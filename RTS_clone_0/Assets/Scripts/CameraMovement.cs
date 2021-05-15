using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;

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

            newCameraPosition.x += (startPosition.x - changingMousePosition.x) / 10;
            newCameraPosition.z += (startPosition.y - changingMousePosition.y) / 10;
            
            
            Camera.main.transform.position = newCameraPosition;
            startPosition = Input.mousePosition;
            //while (timeElapsed < cameraSpeed)
            //{
            //    Camera.main.transform.position = Mathf.Lerp(Camera.main.transform.position, newCameraPosition, timeElapsed / cameraSpeed);
            //}

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjectOnClick : MonoBehaviour
{
    public GameObject selectedObject;

    public GameObject SelectObject()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0);
        if (hitData && Input.GetMouseButtonDown(0))
        {
            selectedObject = hitData.transform.gameObject;
        }
        return selectedObject;
    }
}

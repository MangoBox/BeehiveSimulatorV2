using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public float scrollSpeed;
    public Camera currentCamera;
    public float minCameraSize;
    public float maxCameraSize;
    public float dragSpeed;
    private Vector3 lastMousePos;

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float verticalDelta = Input.mouseScrollDelta.y;
            currentCamera.orthographicSize += verticalDelta * scrollSpeed;
            currentCamera.orthographicSize = Mathf.Clamp(currentCamera.orthographicSize, minCameraSize, maxCameraSize);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                lastMousePos = Input.mousePosition;
            }
            Vector3 mouseDelta = (lastMousePos - Input.mousePosition).normalized;
            currentCamera.transform.position += mouseDelta * dragSpeed * Time.deltaTime;
            lastMousePos = Input.mousePosition;
        }
    }
}

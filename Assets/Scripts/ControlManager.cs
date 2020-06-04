using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;

public class ControlManager : MonoBehaviour
{

    public Camera currentCamera;
    public float scrollSpeed;
    public float dragSpeed;

    private Vector3 lastMousePos;


    void Update()
    {
        //When user clicks
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = currentCamera.ScreenPointToRay(mousePos);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Vector3Int tilePos = BeehiveManager.bm.frameTilemap.WorldToCell(worldPoint);
            tilePos.z = 0;
            //Check cell is valid
            if (BeehiveManager.bm.frameTilemap.GetTile(tilePos) != null)
            {
                BeehiveManager.bm.ClickOnCell(tilePos);
            }
               

        }


        if(Input.mouseScrollDelta.y != 0)
        {
            float verticalDelta = Input.mouseScrollDelta.y;
            currentCamera.orthographicSize += verticalDelta * scrollSpeed;
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePos;
            currentCamera.transform.position += -mouseDelta * dragSpeed;
            lastMousePos = Input.mousePosition;
        }
    }
}

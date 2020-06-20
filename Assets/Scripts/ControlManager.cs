using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Transactions;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;

public class ControlManager : MonoBehaviour
{

    //MouseSettings for current camera and game variables
    public Camera currentCamera;
    public MouseSettings hiveSettings;
    public MouseSettings mapSettings;
    private Vector3 lastMousePos;

    //Camerasizes between hive and map views;
    public float hiveCameraSize;
    public float mapCameraSize;

    //Where did our camera start?
    private Vector3 originalCameraPos;

    //Assign default camera position
    private void Start()
    {
        originalCameraPos = currentCamera.transform.position;
    }

    void Update()
    {
        //When user clicks
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Create a ray to determine where we hit in the world.
            Vector3 mousePos = Input.mousePosition;
            Ray ray = currentCamera.ScreenPointToRay(mousePos);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            //Are we in hive view and thus should we place a tile?
            if(BeehiveManager.bm.gameState == GameState.HIVE_VIEW)
            {
                //Place tile based on position
                Vector3Int tilePos = BeehiveManager.bm.frameTilemap.WorldToCell(worldPoint);
                tilePos.z = 0;
                BeehiveManager.bm.ClickOnCell(tilePos);
            }
            
        }
        //Depending on our view, assign mouse settings.
        MouseSettings currentSettings = BeehiveManager.bm.gameState == GameState.MAP_VIEW ? mapSettings : hiveSettings;

        //If we've scrolled since the last frame
        if(Input.mouseScrollDelta.y != 0)
        {
            //Adjust our orthographic sized based on how far we scrolled, with a multiplier.
            float verticalDelta = Input.mouseScrollDelta.y;
            currentCamera.orthographicSize += verticalDelta * currentSettings.scrollSpeed;
            //Clamp high and low so the user doesn't go crazy with scroll amount.
            currentCamera.orthographicSize = Mathf.Clamp(currentCamera.orthographicSize, currentSettings.minCameraSize, currentSettings.maxCameraSize);
        }

        //Drag mode
        if(Input.GetKey(KeyCode.Mouse1))
        {
            //This ensures our last mouse position is kept before we drag again, prevents glitchiness.
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                lastMousePos = Input.mousePosition;
            }
            //Normalize our delta vector so the camera doesn't go anywhere super fast.
            Vector3 mouseDelta = (lastMousePos - Input.mousePosition).normalized;
            //Offset our camera position.
            currentCamera.transform.position += mouseDelta * currentSettings.dragSpeed * Time.deltaTime;
            lastMousePos = Input.mousePosition;
        }
    }


    //If we need to reset our camera when we change scenes.
    public void ResetCamera()
    {
        currentCamera.orthographicSize = BeehiveManager.bm.gameState == GameState.MAP_VIEW ? mapCameraSize : hiveCameraSize;
        currentCamera.transform.position = originalCameraPos;

    }

    //Serializable class for mouse settings. Easier to track mouse settings between map and hive view!
    [System.Serializable]
    public class MouseSettings
    {
        public float scrollSpeed;
        public float dragSpeed;

        public float minCameraSize;
        public float maxCameraSize;
    }
}

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

    public Camera currentCamera;
    public MouseSettings hiveSettings;
    public MouseSettings mapSettings;
    private Vector3 lastMousePos;

    private Vector3 originalCameraPos;
    private float originalCameraSize;

    private void Start()
    {
        originalCameraPos = currentCamera.transform.position;
        originalCameraSize = currentCamera.orthographicSize;
    }

    void Update()
    {
        //When user clicks
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = currentCamera.ScreenPointToRay(mousePos);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            if(BeehiveManager.bm.gameState == GameState.HIVE_VIEW)
            {
                Vector3Int tilePos = BeehiveManager.bm.frameTilemap.WorldToCell(worldPoint);
                tilePos.z = 0;
                BeehiveManager.bm.ClickOnCell(tilePos);
            }
            
        }
        MouseSettings currentSettings = BeehiveManager.bm.gameState == GameState.MAP_VIEW ? mapSettings : hiveSettings;

        if(Input.mouseScrollDelta.y != 0)
        {
            float verticalDelta = Input.mouseScrollDelta.y;
            currentCamera.orthographicSize += verticalDelta * currentSettings.scrollSpeed;
            currentCamera.orthographicSize = Mathf.Clamp(currentCamera.orthographicSize, currentSettings.minCameraSize, currentSettings.maxCameraSize);
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                lastMousePos = Input.mousePosition;
            }
            Vector3 mouseDelta = (lastMousePos - Input.mousePosition).normalized;
            currentCamera.transform.position += mouseDelta * currentSettings.dragSpeed * Time.deltaTime;
            lastMousePos = Input.mousePosition;
        }
    }


    public void ResetCamera()
    {
        currentCamera.orthographicSize = originalCameraSize;
        currentCamera.transform.position = originalCameraPos;

    }

    [System.Serializable]
    public class MouseSettings
    {
        public float scrollSpeed;
        public float dragSpeed;

        public float minCameraSize;
        public float maxCameraSize;
    }
}

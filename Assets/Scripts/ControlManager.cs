using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;

public class ControlManager : MonoBehaviour
{

    public Camera currentCamera;
    public MouseSettings hiveSettings;
    public MouseSettings mapSettings;
    private Vector3 lastMousePos;


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
                //Check cell is valid
                if (BeehiveManager.bm.frameTilemap.GetTile(tilePos) != null)
                {
                    BeehiveManager.bm.ClickOnCell(tilePos);
                }
            } else if (BeehiveManager.bm.gameState == GameState.MAP_VIEW)
            {
                RaycastHit rhi;
                var result = Physics.Raycast(ray, out rhi);
                GameObject obj_hit = rhi.collider.gameObject;
                Debug.Log(obj_hit.name);
                if (obj_hit.name.StartsWith("Flower"))
                {
                    
                }

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

    [System.Serializable]
    public class MouseSettings
    {
        public float scrollSpeed;
        public float dragSpeed;

        public float minCameraSize;
        public float maxCameraSize;
    }
}

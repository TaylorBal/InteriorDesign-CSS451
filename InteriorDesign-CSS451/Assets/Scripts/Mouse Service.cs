using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class TheWorld : MonoBehaviour {

    public Camera MainCamera = null;
    public CameraControlPreview CamControlPrev = null;

    void LMBService()
    {
        if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
    }

    void SelectObject()
    {
        // Copied from: https://forum.unity.com/threads/preventing-ugui-mouse-click-from-passing-through-gui-controls.272114/
        if (!EventSystem.current.IsPointerOverGameObject()) // check for GUI
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, 1);
            // 1 is the mask for default 
            if (hit)
            {
                //string name = hitInfo.transform.gameObject.name;
                Debug.Log("Selecting object...");
                CamControlPrev.OpenPreviewObject(hitInfo.transform.gameObject);
            }
        }
    }
}

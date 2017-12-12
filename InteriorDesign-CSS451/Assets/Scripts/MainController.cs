﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class MainController : MonoBehaviour {

    public TheWorld theWorld = null;        //represents "The Model" in our design

    //Camera
    public CameraControl mainCameraControl = null;
    public Camera MainCamera = null;
    public Furniture selected = null;

    //for the preview
    public GameObject previewObject = null;
    public CameraControlPreview previewCameraContol = null;

    //EventSystem For Input
    public EventSystem eventSystem = null;

    //Manipulator Control Stuff
    public Manipulator manipulator = null;
    AxisBehavior axisBehavior = null;
    GameObject axis = null;


    //UI Elements Go Here
    public AddFurnitureControl addControl = null;
    public AnchorPlaneControl aPlaneControl = null;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(theWorld != null);
        Debug.Assert(mainCameraControl != null);
        Debug.Assert(previewCameraContol != null);
        Debug.Assert(MainCamera != null);
        Debug.Assert(eventSystem != null);

        //Debug.Assert(addControl != null);

        //UI asserts
        Debug.Assert(addControl != null);
        Debug.Assert(aPlaneControl != null);

        addControl.SetAddListener(AddFurniture);
        aPlaneControl.SetToggleListener(SetAnchorPlaneVisible);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }


    void AddFurniture(string toAdd)
    {
        if (selected != null)
        {
            Furniture toSelect = theWorld.AddFurniture(toAdd, selected);
            if(toSelect != null)
            {
                manipulator.Select(toSelect);
                selected = toSelect;
            }
        }


    }

    void SetAnchorPlaneVisible(bool visible)
    {
        theWorld.SetAnchorSufacesVisible(visible);
    }

    //give the preview a new object to mess with
    void SetPreviewObject(Furniture toCopy)
    {
         if (previewCameraContol.previewObject == toCopy.gameObject)
             return;

         Furniture previewFurniture = theWorld.makePreviewFurniture(toCopy);

         if (previewFurniture == null)
             return;

         previewObject = previewFurniture.gameObject;
         previewCameraContol.previewObject = previewFurniture.gameObject;
         previewCameraContol.LookAt = previewFurniture.gameObject.transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class MainController : MonoBehaviour {

    public TheWorld theWorld = null;        //represents "The Model" in our design

    //Camera
    public CameraControl mainCameraControl = null;
    public Camera MainCamera = null;
    public Furniture selected = null;

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
            theWorld.AddFurniture(toAdd, selected);
        }
    }

    void SetAnchorPlaneVisible(bool visible)
    {
        theWorld.SetAnchorSufacesVisible(visible);
    }
}

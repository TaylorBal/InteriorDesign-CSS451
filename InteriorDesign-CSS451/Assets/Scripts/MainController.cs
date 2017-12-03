using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour {

    public TheWorld theWorld = null;        //represents "The Model" in our design

    //Camera
    public CameraControl mainCamera = null;
    public Furniture testParent= null;
    //EventSystem For Input


    //UI Elements Go Here
    public AddFurnitureControl addControl = null;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(theWorld != null);
        Debug.Assert(mainCamera != null);

        //Debug.Assert(addControl != null);
        Debug.Assert(testParent != null);

        addControl.SetAddListener(AddFurniture);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }


    void AddFurniture(string toAdd)
    {
        theWorld.AddFurniture(toAdd, testParent);
    }
}

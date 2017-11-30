using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour {

    public TheWorld theWorld = null;        //represents "The Model" in our design

    //Camera
    public CameraControl mainCamera = null;

    //EventSystem For Input
    

    //UI Elements Go Here

    // Use this for initialization
    void Start()
    {
        Debug.Assert(theWorld != null);
        Debug.Assert(mainCamera != null);

    }

    // Update is called once per frame
    void Update()
    {

        ProcessInput();
    }


}

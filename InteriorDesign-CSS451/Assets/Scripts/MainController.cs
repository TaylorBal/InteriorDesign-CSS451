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
    public PreviewMenuControl prevMenuControl = null;
    public MatNameIndicator matIndicator = null;

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
        Debug.Assert(prevMenuControl != null);
        Debug.Assert(matIndicator != null);

        addControl.SetAddListener(AddFurniture);
        aPlaneControl.SetToggleListener(SetAnchorPlaneVisible);
        prevMenuControl.SetApplyListener(ApplyTextureChanges);
        prevMenuControl.SetTexListener(ChangeTexture);
        prevMenuControl.SetDeleteListener(DeleteSelected);
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
         matIndicator.SetMat(previewFurniture.materials[previewFurniture.curMatIdx]);
    }

    void DeleteSelected()
    {
        if (selected == null)
            return;

        if ((selected.tag == "Room") ||
                (selected.tag == "Floor") ||
                (selected.tag == "Wall"))
            return;

        theWorld.RemoveFurniture(selected);
        ResetManipulator();
        selected = null;
        previewCameraContol.previewObject = null;
        theWorld.ClearPreviewFurniture();

        matIndicator.SetMat(null);
    }

    void ChangeTexture()
    {
        Furniture f = previewObject.GetComponent<Furniture>();

        int curMatIdx = f.curMatIdx;
        f.ApplyMaterial((curMatIdx + 1) % f.materials.Count);   //cycle to the next one, loop over

        matIndicator.SetMat(f.materials[f.curMatIdx]);
    }

    void ApplyTextureChanges()
    {
        Furniture f = previewObject.GetComponent<Furniture>();

        selected.ApplyMaterial(f.curMatIdx);
    }
}

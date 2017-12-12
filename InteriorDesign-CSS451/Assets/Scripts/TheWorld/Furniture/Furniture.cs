using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Furniture class
//Aso serves as a SceneNode Class
public class Furniture : MonoBehaviour {

    //the location the node pivots around when scaling and rotating
    public Vector3 Pivot = Vector3.zero;

    public Furniture parent = null;
    public List<Furniture> childrenFurniture;

    //a whitelist of furniture this piece can
    //have as children
    public List<string> whitelist;

    //parent
    private Matrix4x4 mCombinedParentXForm = Matrix4x4.identity;
    public List<NodePrimitive> primitives;

    //A list of materials the user can choose from for this Furniture
    public List<Material> materials;

    //Anchor Surface (where children can attach)
    public AnchorSurface anchorSurface = null;
    public AnchorSurface parentAnchorSurface = null;

    //Anchor Point (where the object meet's its
    //parent's anchor surface
    //can also serve as a rotation anchor
    public Vector3 AnchorPoint = Vector3.zero;

    private BoxCollider selectionCollider = null;

    /*
     * SceneNode Content
     * These are variables borrowed from the SceneNode class in MP4
     */

    //The locatin of where the pivot indicator is
    public Transform AxisFrame;
    public float axisFrameSize = 5.0f;

    //The Initial Transform (for reset)
    private Vector3 InitialPos;
    private Quaternion InitialRot;
    private Vector3 InitialScale;



    // Use this for initialization
    void Start () {
        InitialPos = transform.localPosition;
        InitialRot = transform.localRotation;
        InitialScale = transform.localScale;

        InitializeSceneNode();

        selectionCollider = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update() { 
        if(parent != null && parentAnchorSurface == null)
        {
            parentAnchorSurface = parent.GetAnchorSurface();
        }
	}

    private void InitializeSceneNode()
    {
        mCombinedParentXForm = Matrix4x4.identity;
    }

    public void SetAnchorSurfaceVisible(bool visible)
    {
        if (anchorSurface != null)
        {
            anchorSurface.SetVisible(visible);
        }

        foreach(Furniture f in childrenFurniture)
        {
            f.SetAnchorSurfaceVisible(visible);
        }
    }

    /*
     * Transform modificaton
     * ->We implement our own because they are restricted
     * ->By properties of the furniture itself
     */
    public void Translate(Vector3 deltaPos)
    {
        if(parentAnchorSurface != null)
        {
            Pivot += parentAnchorSurface.FixDelta(this, deltaPos);
        }
        else
        {
            Pivot += deltaPos;
        }

    }

    public void Rotate(Vector3 axis, float angle)
    {
        //I don't have rotation restrictions set up yet
        //transform.localRotation *= q;
    }

    public void Scale(Vector3 scaleDelta)
    {

    }


    public void SetParentAnchor(AnchorSurface parentAnchor)
    {
        parentAnchorSurface = parentAnchor;
    }

    public AnchorSurface GetAnchorSurface()
    {
        return anchorSurface;
    }

    //check if a particular tag is whitelisted as a child
    public bool IsWhitelisted(string tag)
    {
        for(int i = 0; i < whitelist.Count; i++)
        {
            if (whitelist[i] == tag)
                return true;
        }

        Debug.Log(tag + " not in whitelist!");
        return false;
    }

    //Try to add a piece of furniture as the child of this one
    //Hierarchy restrictions can cause this to fail
    public Furniture AddChild(GameObject prefab)
    {
        if (!IsWhitelisted(prefab.tag))
            return null;

        //Do the actual add process
        GameObject child = Instantiate(prefab, transform);

        //we want to place it at the mouse position,
        //but for now, place it at the origin of the parent
        child.transform.localPosition = Vector3.zero;

        //Do some placement inside its restriction zone
        //for this we need the child's Furniture class
        Furniture childFurn = child.GetComponent<Furniture>();
        childrenFurniture.Add(childFurn);
        childFurn.parent = this;

        childFurn.SetParentAnchor(anchorSurface);

        if(anchorSurface != null)
        {
           anchorSurface.AnchorTransform(ref childFurn);
        }

        return childFurn;
    }

    public bool DeleteChild(Furniture child)
    {
        //is the piece of furniture even a child?
        if (!childrenFurniture.Contains(child))
            return false;

        child.DeleteAllChildren();
        childrenFurniture.Remove(child);
        Destroy(child);

        return true;
    }

    public void DeleteAllChildren()
    {
        foreach(Furniture child in childrenFurniture)
        {
            child.DeleteAllChildren();
            childrenFurniture.Remove(child);
            Destroy(child);
        }
    }

    //MATERIAL SWAPPING

    public bool SetMat(int index)
    {
        if(index < 0 || index >= materials.Count)
        {
            Debug.Log("invalid index!");
            return false;
        }

        //set the material for each of the children meshes
        //don't know how to implement this
        //maybe we should have groups of materials? idk
        return false;
    }

    public List<Material> GetMatList()
    {
        return materials;
    }


    /*
     * SceneNode Content
     * 
     */
     //CompositeXForm adapted from prof. Sung's MP4
    // tipPos: is the origin of this scene node
    // topDir: is the y-direction of this node
    public void CompositeXForm(ref Matrix4x4 parentXform, out Vector3 snPivot, out Vector3 snUp)
    {
        Matrix4x4 pivot = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);  // Pivot translation
        Matrix4x4 invPivot = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);  // inv Pivot
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        mCombinedParentXForm = parentXform * pivot * trs;

        // let's decompose the combined matrix into R, and S
        Vector3 c0 = mCombinedParentXForm.GetColumn(0);
        Vector3 c1 = mCombinedParentXForm.GetColumn(1);
        Vector3 c2 = mCombinedParentXForm.GetColumn(2);
        Vector3 s = new Vector3(c0.magnitude, c1.magnitude, c2.magnitude);
        Matrix4x4 r = Matrix4x4.identity;
        c0 /= s.x;  // normalize the columns
        c1 /= s.y;
        c2 /= s.z;
        r.SetColumn(0, c0);
        r.SetColumn(1, c1);
        r.SetColumn(2, c2);
        Quaternion q = Quaternion.LookRotation(c2, c1); // creates a rotation matrix with c2-Forward, c1-up

        snPivot = mCombinedParentXForm.GetColumn(3);
        snUp = c1;

        if(selectionCollider != null)
        {
            selectionCollider.center = snPivot;
        }

        if (AxisFrame != null)
        {
            AxisFrame.localPosition = snPivot;  // our location is Pivot 
            AxisFrame.localScale = s * axisFrameSize;
            AxisFrame.localRotation = q;
        }

        // propagate to all children
        foreach (Transform child in transform)
        {
            Furniture cn = child.GetComponent<Furniture>();
            if (cn != null)
            {
                cn.CompositeXForm(ref mCombinedParentXForm, out snPivot, out snUp);
            }
        }

        // disenminate to primitives
        foreach (NodePrimitive p in primitives)
        {
            p.LoadShaderMatrix(ref mCombinedParentXForm);
        }
    }

    public Matrix4x4 getXForm()
    {
        return mCombinedParentXForm;
    }

    public void ResetXForm()
    {
        transform.localPosition = InitialPos;
        transform.localRotation = InitialRot;
        transform.localScale = InitialScale;

        //propogate to children
        foreach (Transform child in transform)
        {
            Furniture node = child.GetComponent<Furniture>();
            if (node != null)
            {
                node.ResetXForm();
            }
        }
    }
}

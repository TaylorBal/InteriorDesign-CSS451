using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {

    //a whitelist of furniture this piece can
    //have as children
    public List<string> whitelist;

    public Furniture parent = null;
    public List<Furniture> childrenFurniture;

    //A list of materials the user can choose from for this Furniture
    public List<Material> materials;



    //Anchor Surface (where children can attach)
    public AnchorSurface anchorSurface = null;
    public AnchorSurface parentAnchorSurface = null;

    //Anchor Point (where the object meet's its
    //parent's anchor surface
    //can also serve as a rotation anchor
    public Vector3 AnchorOffset = Vector3.zero;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update() { 
        if(parent != null && parentAnchorSurface == null)
        {
            parentAnchorSurface = parent.GetAnchorSurface();
        }
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
            transform.position = parentAnchorSurface.RestrictMotion(this, deltaPos);
        }
        else
        {
            transform.position += deltaPos;
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
    public bool AddChild(GameObject prefab)
    {
        if (!IsWhitelisted(prefab.tag))
            return false;

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

        //NOT YET IMPLEMENTED
        if(anchorSurface != null)
        {
           anchorSurface.AnchorTransform(ref childFurn);
        }

        return true;
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
}

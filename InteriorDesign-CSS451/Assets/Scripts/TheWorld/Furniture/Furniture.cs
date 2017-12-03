using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {

    //a whitelist of furniture this piece can
    //have as children
    public List<string> whitelist;

    //A list of materials the user can choose from for this Furniture
    public List<Material> materials;

    //Anchor Surface (where children can attach)
    //public AnchorSurface anchorSurface = null;

    //Anchor Point (where the object meet's its
    //parent's anchor surface
    //can also serve as a rotation anchor
    public Vector3 AnchorOffset = Vector3.zero;

	// Use this for initialization
	void Start () {
        //anchorSurface = new AnchorSurface();
	}
	
	// Update is called once per frame
	void Update() { 
	}

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

        //Do some placement inside its restriction zone
        //for this we need the child's Furniture class
        Furniture childFurn = child.GetComponent<Furniture>();

        //NOT YET IMPLEMENTED


        return true;
    }

    public Vector3 RestrictChildMotion(Vector3 childPos, Vector3 deltaT)
    {
        //return anchorSurface.RestrictMotion(childPos, deltaT);
        return Vector3.zero;
    }


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;



public partial class TheWorld : MonoBehaviour {

    //store a . This way we can look up
    //and instantiate by name
    [Serializable]
    public struct NamedPrefab
    {
        public string name;
        public GameObject prefab;
    }

    //A Dictionary is preferred, but they're not serializeable :(
    public NamedPrefab[] furniturePrefabs;

    //Essentially our "Root" node of the hierarchy
    public Furniture theRoom = null;
    public Furniture previewRoot = null;
    private Furniture previewFurniture = null;

	// Use this for initialization
	void Start () {
        Debug.Assert(theRoom != null);
        Debug.Assert(previewRoot != null);
	}
	
	// Update is called once per frame
	void Update () {
        Matrix4x4 m = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Vector3 pivot = Vector3.zero;
        Vector3 up = Vector3.up;
        theRoom.CompositeXForm(ref m, out pivot, out up);

        Matrix4x4 m2 = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        previewRoot.CompositeXForm(ref m2, out pivot, out up);
    }


    /*
     *      Adding and Removing Furniture 
     */
    public GameObject FindPrefabByName(string name)
    {
        for(int i = 0; i < furniturePrefabs.Length; i++)
        {
            if (furniturePrefabs[i].name == name)
                return furniturePrefabs[i].prefab;
        }

        Debug.Log("Could not find prefab (" + name + ")");
        return null;
    }

    //add a piece of furniture into the scene
    //Can fail, based on hierarcy restrictions
    public Furniture AddFurniture(string toAdd, Furniture parent)
    {
        Debug.Log("TheWorld making furniture (" + toAdd + ")");

        GameObject prefab = FindPrefabByName(toAdd);
        if (prefab == null)
            return null;

        return parent.AddChild(prefab);
    }

    //Remove a piece of furniture from the scene
    public bool RemoveFurniture(Furniture toRemove)
    {
        return toRemove.parent.DeleteChild(toRemove);
    }

    public void SetAnchorSufacesVisible(bool visible)
    {
        theRoom.SetAnchorSurfaceVisible(visible);
    }

    public bool IsTagFurniture(string tag)
    {
        for (int i = 0; i < furniturePrefabs.Length; i++)
        {
            if (furniturePrefabs[i].name == tag)
                return true;
        }
        return false;
    }

    public Furniture makePreviewFurniture(Furniture toCopy)
    {
        //if the preview root has something in it, remove first
        if (previewRoot.childrenFurniture.Count > 0)
            previewRoot.DeleteChild(previewRoot.childrenFurniture[0]);

        //then add a new child, the same type as the toCopy
        previewFurniture = AddFurniture(toCopy.tag, previewRoot);

        if (previewFurniture == null)
            return null;

        previewFurniture.SetPreview();

        //now copy all of the relevant properties over
        previewFurniture.curMatIdx = toCopy.curMatIdx;


        return previewFurniture;
    }

    public void ClearPreviewFurniture()
    {
        if (previewRoot.childrenFurniture.Count > 0)
            previewRoot.DeleteChild(previewRoot.childrenFurniture[0]);
    }

    public Furniture GetPreviewFurniture()
    {
        return previewFurniture;
    }

    public void ApplyPreviewChanges(Furniture preview)
    {
        
    }
}

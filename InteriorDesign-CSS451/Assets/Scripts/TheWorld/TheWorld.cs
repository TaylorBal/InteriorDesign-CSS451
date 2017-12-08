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

	// Use this for initialization
	void Start () {
        Debug.Assert(theRoom != null);
	}
	
	// Update is called once per frame
	void Update () {
        Matrix4x4 m = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Vector3 pivot = Vector3.zero;
        Vector3 up = Vector3.up;
        theRoom.CompositeXForm(ref m, out pivot, out up);
    }


    /*
     *      Adding and Removing Furniture 
     */
    GameObject FindPrefabByName(string name)
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
    public bool AddFurniture(string toAdd, Furniture parent)
    {
        Debug.Log("TheWorld making furniture (" + toAdd + ")");

        GameObject prefab = FindPrefabByName(toAdd);
        if (prefab == null)
            return false;

        if (!parent.AddChild(prefab))
            return false;

        return true;
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
}

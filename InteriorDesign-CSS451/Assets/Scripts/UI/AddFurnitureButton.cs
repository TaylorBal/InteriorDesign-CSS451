using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddFurnitureButton : MonoBehaviour {

    public delegate void AFBCallbackDelegate(string name);
    private AFBCallbackDelegate mCallback = null;

    public string toAdd = "not set";
    public Button theButton = null;

    // Use this for initialization
    void Start () {
        Debug.Assert(theButton != null);
       
        theButton.onClick.AddListener(Add);
	}

    public void SetClickListener(AFBCallbackDelegate listener)
    {
        mCallback = listener;
    }

    public void Add()
    {
        if(mCallback != null)
        {
            mCallback(toAdd);
        }
    }
}

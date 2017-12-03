using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddFurnitureControl : MonoBehaviour
{

    public Button addButton = null;
    public string toMake = "Vase";

    public delegate void AddCallbackDelegate(string name);
    private AddCallbackDelegate mCallback = null;
    // Use this for initialization
    void Start()
    {
        Debug.Assert(addButton != null);

        addButton.onClick.AddListener(Reset);
    }

    public void SetAddListener(AddCallbackDelegate listener)
    {
        mCallback = listener;
    }

    public void Reset()
    {
        if (mCallback != null)
            mCallback(toMake);
    }
}

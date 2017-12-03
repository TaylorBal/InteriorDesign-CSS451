using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddFurnitureControl : MonoBehaviour
{

    public AddFurnitureButton[] addButtons;

    public delegate void AddCallbackDelegate(string name);
    private AddCallbackDelegate mCallback = null;
    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < addButtons.Length; i++)
        {
            addButtons[i].SetClickListener(AddFurniture);
        }
    }

    public void SetAddListener(AddCallbackDelegate listener)
    {
        mCallback = listener;
    }

    public void AddFurniture(string name)
    {
        if (mCallback != null)
        {
            mCallback(name);
        }
    }
}

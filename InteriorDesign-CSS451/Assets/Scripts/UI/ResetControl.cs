using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetControl : MonoBehaviour {

    public Button resetButton = null;

    public delegate void ResetCallbackDelegate();
    private ResetCallbackDelegate mCallback = null;
	// Use this for initialization
	void Start () {
        Debug.Assert(resetButton != null);

        resetButton.onClick.AddListener(Reset);
	}
	
    public void SetResetListener(ResetCallbackDelegate listener)
    {
        mCallback = listener;
    }

    public void Reset()
    {
        if (mCallback != null)
            mCallback();
    }
}

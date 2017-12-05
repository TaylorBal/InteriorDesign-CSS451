using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorPlaneControl : MonoBehaviour {

    public delegate void AnchorPlaneControlDelegate(bool state);
    private AnchorPlaneControlDelegate mCallback = null;

    public Toggle theToggle = null;

    // Use this for initialization
    void Start() {
        Debug.Assert(theToggle != null);

        theToggle.onValueChanged.AddListener(StateChange);

    }

    public void SetToggleListener(AnchorPlaneControlDelegate listener)
    {
        mCallback = listener;
    }
    
    public void StateChange(bool state)
    {
        if (mCallback != null)
            mCallback(state);
    }
}

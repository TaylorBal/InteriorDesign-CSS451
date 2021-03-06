﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour {

    public GameObject translateManip = null;
    public GameObject rotateManip = null;
    public GameObject scaleManip = null;

    private bool hasAxes = false;
    private GameObject axesType = null;
    public GameObject axes = null;
    public AxisFrm AF = null;

    public Vector3 manipSensitivity = new Vector3(1.0f, 1.0f, 1.0f);

    public enum ManipMode{
        translate,
        rotate,
        scale
    };

    public ManipMode mode = ManipMode.translate;

    public Furniture mSelected = null;

	// Use this for initialization
	void Start () {
        Debug.Assert(translateManip != null);
        Debug.Assert(rotateManip != null);
        Debug.Assert(scaleManip != null);

        axesType = translateManip;
	}
	
	// Update is called once per frame
	void Update () {
		if(hasAxes)
        {
            axes.transform.localPosition = transform.localPosition;
            axes.transform.localRotation = transform.localRotation;
        }

        if (mSelected != null)
        {
            if (hasAxes)
            {
                AF.SetActiveState(mSelected.canTranslate);       
            }
        }
	}

    public void SetManipMode(ManipMode newMode)
    {
        switch (newMode)
        {
            case ManipMode.translate:
                {
                    axesType = translateManip;
                    break;
                }
            case ManipMode.rotate:
                {
                    axesType = rotateManip;
                    break;
                }
            case ManipMode.scale:
                {
                    axesType = scaleManip;
                    break;
                }
        }

        mode = newMode;
    }

    public void Select(Furniture toSelect)
    {
        if(mSelected !=  null)
            mSelected.AxisFrame = null;

        mSelected = toSelect;
        if(!hasAxes)
        {
            axes = Instantiate(axesType, mSelected.transform);
            AF = axes.GetComponent<AxisFrm>();
            hasAxes = true;
        }


        mSelected.AxisFrame = transform;

    }

    public void Deselect()
    {
        if(hasAxes)
        {
            Destroy(axes);
            AF = null;
            hasAxes = false;
        }

        if (mSelected != null)
        {
            mSelected.AxisFrame = null;
            mSelected = null;
        }
    }

    public void MoveX(Vector3 inputVec)
    {
        if (mSelected == null)
            return;

        if (hasAxes == false)
            return;

        if (!mSelected.canTranslate[0])
            return;

        float mag = manipSensitivity.x * Vector3.Dot(inputVec, axes.transform.right);
        switch(mode)
        {
            case ManipMode.translate:
                mSelected.Translate(mag * Vector3.right);
                break;
            case ManipMode.rotate:
                break;
            case ManipMode.scale:
                break;
        }
    }

    public void MoveY(Vector3 inputVec)
    {
        if (mSelected == null)
            return;

        if (hasAxes == false)
            return;

        if (!mSelected.canTranslate[1])
            return;

        float mag = manipSensitivity.y * Vector3.Dot(inputVec, axes.transform.up);
        switch (mode)
        {
            case ManipMode.translate:
                mSelected.Translate(mag * Vector3.up);
                break;
            case ManipMode.rotate:
                break;
            case ManipMode.scale:
                break;
        }
    }

    public void MoveZ(Vector3 inputVec)
    {

        if (mSelected == null)
            return;

        if (hasAxes == false)
            return;

        if (!mSelected.canTranslate[2])
            return;

        float mag = manipSensitivity.z * Vector3.Dot(inputVec, axes.transform.forward);
        switch (mode)
        {
            case ManipMode.translate:
                mSelected.Translate(mag * Vector3.forward);
                break;
            case ManipMode.rotate:
                break;
            case ManipMode.scale:
                break;
        }
    }
}

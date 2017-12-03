using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNode : MonoBehaviour {

    //the location the node pivots around when scaling and rotating
    public Vector3 Pivot = Vector3.zero;

    //parent
    private Matrix4x4 mCombinedParentXForm = Matrix4x4.identity;
    public List<NodePrimitive> primitives;

    //The locatin of where the pivot indicator is
    public Transform AxisFrame;

    //The Initial Transform (for reset)
    private Vector3 InitialPos;
    private Quaternion InitialRot;
    private Vector3 InitialScale;
    // Use this for initialization
	void Start () {
        InitialPos = transform.localPosition;
        InitialRot = transform.localRotation;
        InitialScale = transform.localScale;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void CompositeXForm(ref Matrix4x4 parentXForm)
    {
        Matrix4x4 pivot = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        mCombinedParentXForm = parentXForm * pivot * trs;

        //propogate to children
        foreach(Transform child in transform)
        {
            SceneNode node = child.GetComponent<SceneNode>();
            if(node != null)
            {
                node.CompositeXForm(ref mCombinedParentXForm);
            }
        }

        //propogate to primitives
        foreach(NodePrimitive p in primitives)
        {
            p.LoadShaderMatrix(ref mCombinedParentXForm);
        }

        //compute axisFrame
        if(AxisFrame != null)
        {
            AxisFrame.localPosition = mCombinedParentXForm.MultiplyPoint(Vector3.zero);
            Vector3 fwd = mCombinedParentXForm.MultiplyPoint(Vector3.forward) - AxisFrame.localPosition;
            Vector3 up = mCombinedParentXForm.MultiplyPoint(Vector3.up) - AxisFrame.localPosition;
            AxisFrame.localRotation = Quaternion.LookRotation(fwd, up);
        }
    }

    public Matrix4x4 getXForm()
    {
        return mCombinedParentXForm;
    }

    public void ResetXForm()
    {
        transform.localPosition = InitialPos;
        transform.localRotation = InitialRot;
        transform.localScale = InitialScale;

        //propogate to children
        foreach (Transform child in transform)
        {
            SceneNode node = child.GetComponent<SceneNode>();
            if (node != null)
            {
                node.ResetXForm();
            }
        }
    }
}

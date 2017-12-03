using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive : MonoBehaviour {

    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;

    public bool rotate = false;
    public float rotationSpeed = 0.0f;
    public Vector3 rotationAxis = Vector3.up;
    public float maxAngle = 45.0f;
    private float curAngle = 0.0f; 
    private int forward = 1;    //1 is fwd, -1 is backwards

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        IncrementXForm();
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix * p * trs * invp;

        GetComponent<Renderer>().material.SetMatrix("MyXformMat", m);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
    }

    void IncrementXForm()
    {
        //check if angle past maxAngle around
        if(forward == 1)
        {
            if(curAngle >= maxAngle)
            {
                forward *= -1;
            }
            else
            {
                curAngle += rotationSpeed * forward * Time.fixedDeltaTime;
            }
        }
        else if(forward == -1)
        {
            if (curAngle <= -maxAngle)
            {
                forward *= -1;
            }
            else
            {
                curAngle += rotationSpeed * forward * Time.fixedDeltaTime;
            }
        }

        Quaternion q = Quaternion.AngleAxis(rotationSpeed * forward * Time.fixedDeltaTime, rotationAxis);
        transform.localRotation = q * transform.localRotation;
    }
}

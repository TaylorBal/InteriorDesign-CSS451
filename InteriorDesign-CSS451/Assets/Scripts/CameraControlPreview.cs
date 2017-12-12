using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlPreview : MonoBehaviour {

    public GameObject previewObject = null;
    public Transform LookAt = null;

    bool dragging = false;

    private float mMouseX = 0f;
    private float mMouseY = 0f;

    public Vector3 sensitivity = new Vector3(1.0f, 1.0f, 1.0f);
    private const float kPixelToDegree = 0.1f;
    private const float kPixelToDistant = 0.05f;

    public float zoomMin = 0.5f;
    public float zoomMax = 20.0f;

    // Use this for initialization
    void Start () {
        Debug.Assert(LookAt != null);
    }
	
	// Update is called once per frame
	void Update () {

        if (previewObject != null)
        {
            // this will change the rotation
            LookAt.position = previewObject.GetComponent<Furniture>().getXForm().MultiplyPoint(Vector3.zero);
            transform.LookAt(LookAt);
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetMouseButtonDown(0))
            {
                dragging = true;
            }
        }

        if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            dragging = false;
        }


        //find the delta mouse
        Vector3 deltaMouse;
        deltaMouse.x = Input.GetAxis("Mouse X");
        deltaMouse.y = Input.GetAxis("Mouse Y");
        deltaMouse.z = Input.GetAxis("Mouse ScrollWheel");     //Input.mouseposition only stores in x, y

        if (dragging)
        {
            RotateCameraAboutUp(deltaMouse.x * sensitivity.x);
            RotateCameraAboutSide(-deltaMouse.y * sensitivity.y);
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            float moveDist = deltaMouse.z * sensitivity.z;
            Vector3 V = LookAt.localPosition - transform.localPosition;

            if (V.magnitude < zoomMin && moveDist > 0)
                return;

            if (V.magnitude > zoomMax && moveDist < 0)
                return;

            transform.localPosition += moveDist * V.normalized;
        }
    }

    private void RotateCameraAboutUp(float degree)
    {
        Quaternion up = Quaternion.AngleAxis(degree, transform.up);
        RotateCameraPosition(ref up);
    }

    private void RotateCameraAboutSide(float degree)
    {
        Quaternion side = Quaternion.AngleAxis(degree, transform.right);
        RotateCameraPosition(ref side);
    }

    private void RotateCameraPosition(ref Quaternion q)
    {
        Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, q, Vector3.one);
        Matrix4x4 invP = Matrix4x4.TRS(-LookAt.localPosition, Quaternion.identity, Vector3.one);
        Matrix4x4 m = invP.inverse * r * invP;

        Vector3 newCameraPos = m.MultiplyPoint(transform.localPosition);
        if (Mathf.Abs(Vector3.Dot(newCameraPos.normalized, Vector3.up)) < 0.985)
        {
            transform.localPosition = newCameraPos;

            // First way:
            // transform.LookAt(LookAt);
            // Second way:
            // Vector3 v = (LookAt.localPosition - transform.localPosition).normalized;
            // transform.localRotation = Quaternion.LookRotation(v, Vector3.up);
            // Third way: do everything ourselve!
            Vector3 v = (LookAt.localPosition - transform.localPosition).normalized;
            Vector3 w = Vector3.Cross(v, transform.up).normalized;
            Vector3 u = Vector3.Cross(w, v).normalized;
            // INTERESTING: 
            //    chaning the following directions must be done in specific sequence!
            //    E.g., NONE of the following order works: 
            //          Forward, Up, Right 
            //          Forward, Right, Up 
            //          Right, Forward, Up 
            //          Up, Forward, Right 
            //
            //   Forward-Vector MUST BE set LAST!!: both of the following works!
            //          Right, Up, Forward
            //          Up, Right, Forward
            transform.up = u;
            transform.right = w;
            transform.forward = v;
        }
    }

    public void SetLookAtPos(Vector3 p)
    {
        //LookAt.localPosition = p;
    }

    public void OpenPreviewObject(GameObject inPreviewObject)
    {
        //Delete previous preview object if it exists
        if (previewObject != null)
        {
            DestroyImmediate(previewObject);
            Debug.Log("Destroy preview");
        }

        //Instantiate copy
        previewObject = Instantiate(inPreviewObject);
        Debug.Log("Instantiate");

        previewObject.layer = LayerMask.GetMask("Preview");

        //Look at copy
        LookAt = previewObject.transform;
        Debug.Log("Look at");
    }
}

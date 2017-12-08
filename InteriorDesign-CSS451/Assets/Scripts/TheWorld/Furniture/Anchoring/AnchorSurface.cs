using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AnchorSurface
//Used to restrict motion of objects
//place at center of desired shape, and give desired orientation
//when changes to child objects are desired, they are passed through
//this class and restricted
public class AnchorSurface : MonoBehaviour
{

    public enum AnchorPlaneType
    {
        rectangle,
        circle
    }

    public AnchorPlaneType type = AnchorPlaneType.rectangle;

    public Vector3 AnchorOffset = Vector3.zero;

    //for Rectangle planes
    public float width = 1.0f;                //with is along transform.right
    public float height = 1.0f;               //height is along transform.forward

    //for Circular Planes
    public float radius = 1.0f;        //radius of circle around transform.up

    //FOR DEBUG PURPOSES, create primitives for visual aid?
    private GameObject circlePlanePrim = null;
    private GameObject rectPlanePrim = null;
    public bool showSurface = false;
    public Material AnchorDebugMaterial = null;

    public Furniture parent = null;

    // Use this for initialization
    void Start()
    {
        Debug.Assert(AnchorDebugMaterial != null);
        Debug.Assert(parent != null);

        circlePlanePrim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        circlePlanePrim.transform.parent = transform;
        circlePlanePrim.transform.localPosition = Vector3.zero;
        circlePlanePrim.transform.localRotation = Quaternion.identity;
        circlePlanePrim.transform.localScale = new Vector3(2 * radius, 0.01f, 2 * radius);
        MeshRenderer mr = circlePlanePrim.GetComponent<MeshRenderer>();
        mr.material = AnchorDebugMaterial;

        rectPlanePrim = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rectPlanePrim.transform.parent = transform;
        rectPlanePrim.transform.localPosition = Vector3.zero;
        rectPlanePrim.transform.localRotation = Quaternion.identity;
        rectPlanePrim.transform.localScale = new Vector3(width, 0.01f, height);
        mr = rectPlanePrim.GetComponent<MeshRenderer>();
        mr.material = AnchorDebugMaterial;

        SetVisible(showSurface);
    }

    void Update()
    { 
        UpdateTransform();
    }

    void UpdateTransform()
    {
        Matrix4x4 pXForm = parent.getXForm();

        // let's decompose the combined matrix into R, and S
        Vector3 c0 = pXForm.GetColumn(0);
        Vector3 c1 = pXForm.GetColumn(1);
        Vector3 c2 = pXForm.GetColumn(2);
        Vector3 s = new Vector3(c0.magnitude, c1.magnitude, c2.magnitude);
        Quaternion q = Quaternion.LookRotation(c2 / s.y, c1 / s.z); // creates a rotation matrix with c2-Forward, c1-up

        transform.position = pXForm.GetColumn(3);       //using localPosition here does WEIRD things?
        transform.localPosition += AnchorOffset;

        transform.localScale = s;
    }

    public void SetVisible(bool visible)
    {
        showSurface = visible;
        if (visible)
        {
            switch (type)
            {
                case AnchorPlaneType.circle:
                    circlePlanePrim.SetActive(true);
                    rectPlanePrim.SetActive(false);
                    break;
                case AnchorPlaneType.rectangle:
                    circlePlanePrim.SetActive(false);
                    rectPlanePrim.SetActive(true);
                    break;
            }
        }
        else
        {
            circlePlanePrim.SetActive(false);
            rectPlanePrim.SetActive(false);
        }
    }

    public bool IsVisible()
    {
        return showSurface;
    }

    //Anchor an object to the surface
    //Position and Orientation
    public void AnchorTransform(ref Furniture f)
    {
        //use the Furniture's Anchor offset
        Vector3 furniturePos = f.getXForm().GetColumn(3);
        Vector3 anchorPoint = furniturePos + f.AnchorPoint;


        //1. Anchor to the plane as a whole        
        //project a point onto a plane
        Vector3 V = anchorPoint - transform.position;
        Vector3 H = Vector3.Dot(V, transform.up) * transform.up;

        //clamp
        f.Pivot -= H;
        //furniturePos -= H;

        //then anchor the rotation (same as ours)
        f.transform.rotation = transform.rotation;
    }

    //Takes a translation and restricts it to stay within the surface
    public Vector3 FixDelta(Furniture f, Vector3 deltaT)
    {
        Vector3 furniturePos = f.getXForm().GetColumn(3);
        Vector3 valid = Restrain(furniturePos, deltaT, f.AnchorPoint);
        return valid - furniturePos;

    }


    //find the closest valid point to the
    //point starts in object space?
    private Vector3 Restrain(Vector3 furniturePos, Vector3 delta, Vector3 anchorOffset)
    {
        Matrix4x4 m = transform.worldToLocalMatrix;

        //remove the relative y-axis movement
        Vector3 Y = Vector3.Dot(delta, transform.up) * transform.up;
        delta -= Y;

        Vector3 localPos = m.MultiplyPoint(furniturePos + anchorOffset + delta);
        

        switch (type)
        {
            case AnchorPlaneType.circle:
                {
                    if (localPos.magnitude <= radius)
                    {
                        break;
                    }
                    else
                    {
                        float excess = localPos.magnitude - radius;     //the excess distance
                        Vector3 n = localPos.normalized;                //normal from center to localPos
                        localPos = localPos - excess * n;               //fix the localPos
                        break;
                    }
                }
            case AnchorPlaneType.rectangle:
                {
                    //fix the relative x-axis
                    if (localPos.x < -width / 2)
                    {
                        localPos.x = -width / 2;
                    }
                    else if (localPos.x > width / 2)
                    {
                        localPos.x = width / 2;
                    }

                    //fix the relative z-axis
                    if(localPos.z < -height / 2)
                    {
                        localPos.z = -height / 2;
                    }
                    else if(localPos.z > height / 2)
                    {
                        localPos.z = height / 2;
                    }

                    break;
                }
        }

        Matrix4x4 mback = transform.localToWorldMatrix;
        Vector3 validPos = mback.MultiplyPoint(localPos);

        return validPos - anchorOffset;
    }

}
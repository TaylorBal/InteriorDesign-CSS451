﻿using System.Collections;
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
        invalid,
        rectangle,
        circle
    }

    public AnchorPlaneType type = AnchorPlaneType.invalid;

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

    // Use this for initialization
    void Start()
    {
        Debug.Assert(AnchorDebugMaterial != null);

        circlePlanePrim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        circlePlanePrim.transform.parent = transform;
        circlePlanePrim.transform.localPosition = Vector3.zero;
        circlePlanePrim.transform.localScale = new Vector3(2 * radius, 0.01f, 2 * radius);
        MeshRenderer mr = circlePlanePrim.GetComponent<MeshRenderer>();
        mr.material = AnchorDebugMaterial;

        rectPlanePrim = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rectPlanePrim.transform.parent = transform;
        rectPlanePrim.transform.localPosition = Vector3.zero;
        rectPlanePrim.transform.localScale = new Vector3(width, 0.01f, height);
        mr = rectPlanePrim.GetComponent<MeshRenderer>();
        mr.material = AnchorDebugMaterial;



        if (showSurface)
        {
            switch (type)
            {
                case AnchorPlaneType.circle:
                    rectPlanePrim.SetActive(false);
                    break;
                case AnchorPlaneType.rectangle:
                    circlePlanePrim.SetActive(false);
                    break;
            }
        }
        else
        {
            circlePlanePrim.SetActive(false);
            rectPlanePrim.SetActive(false);
        }
    }

    void Update()
    {

    }

    public void SetVisible(bool visible)
    {
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

    //Anchor an object to the surface
    //Position and Orientation
    public void AnchorTransform(ref Furniture f)
    {
        //use the Furniture's Anchor offset
        Vector3 pos = f.transform.localPosition + f.AnchorOffset;


        //1. Anchor to the plane as a whole        
        //project a point onto a plane
        Vector3 V = pos - transform.position;
        Vector3 H = Vector3.Dot(V, transform.up) * transform.up;

        f.transform.position -= H;

        //then anchor the rotation (same as ours)
        f.transform.rotation = transform.rotation;

        //2. If not inside the surface, restrict
        //to the closest point
        Vector3 valid = GetClosestValid(f.transform.position, f.AnchorOffset);
        f.transform.position = valid;
    }


    //Takes a translation and restricts it to stay within the surface
    public Vector3 RestrictMotion(Furniture f, Vector3 deltaT)
    {
        return GetClosestValid(f.transform.position + deltaT, f.AnchorOffset);
    }


    //find the closest valid point to the
    //point starts in object space?
    private Vector3 GetClosestValid(Vector3 worldPos, Vector3 anchorOffset)
    {
        Matrix4x4 m = transform.worldToLocalMatrix;
        Vector3 localPos = m.MultiplyPoint(worldPos + anchorOffset);

        switch (type)
        {
            case AnchorPlaneType.circle:
                {
                    if (localPos.magnitude <= radius)
                    {
                        return worldPos;                              //we're good, no need to do another matrix mult
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

                    //fix the relative y-axis
                    if(localPos.y < -height / 2)
                    {
                        localPos.y = -height / 2;
                    }
                    else if(localPos.y > height / 2)
                    {
                        localPos.y = height / 2;
                    }

                    break;
                }
            case AnchorPlaneType.invalid:
                {
                    Debug.Log("invalid anchor surface, cannot get a valid point");
                    return Vector3.zero;
                }
        }

        Matrix4x4 mback = transform.localToWorldMatrix;
        Vector3 validPos = mback.MultiplyPoint(localPos);

        return validPos - anchorOffset;
    }

}
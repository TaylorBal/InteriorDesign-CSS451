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
        invalid,
        rectangle,
        circle
    }

    public AnchorPlaneType type = AnchorPlaneType.invalid;

    //how close an object needs to be to be considered "anchored"
    private float anchorTolerance = 0.01f;

    //for Rectangle planes
    float width = 1.0f;                //with is along transform.right
    float height = 1.0f;               //height is along transform.forward

    //for Circular Planes
    float radius = 1.0f;        //radius of circle around transform.up

    //FOR DEBUG PURPOSES, create primitives for visual aid?

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {

    }

    //Anchor an object to the surface
    //Position and Orientation
    void AnchorTransform(Furniture f)
    {
        //1. Anchor to the plane as a whole

        //project a point onto a plane
        Vector3 V = f.transform.localPosition - transform.localPosition;
        Vector3 H = Vector3.Dot(V, transform.up) * transform.up;
        Vector3 projected = f.transform.localPosition - H;

        //The projection is the new location
        f.transform.localPosition = projected;

        //then anchor the rotation (same as ours)
        f.transform.rotation = transform.rotation;


        //2. If not inside the surface, restrict
        //to the closest point
        Vector3 valid = GetClosestValid(f.transform.localPosition);
        f.transform.localPosition = valid;

    }


    //Takes a translation and restricts it to stay within the surface
    public Vector3 RestrictMotion(Vector3 curPos, Vector3 deltaT)
    {
        Vector3 fix = GetClosestValid(curPos + deltaT);
        /*
        switch (type)
        {
            case AnchorPlaneType.rectangle:
                {
                    return RectangleRestrict(curPos, deltaT);
                }
            case AnchorPlaneType.circle:
                {
                    return CircleRestrict(curPos, deltaT);
                }
            case AnchorPlaneType.invalid:
                return Vector3.zero;
        }
        */
        return fix;
        //return Vector3.zero;
    }

    private Vector3 RectangleRestrict(Vector3 curPos, Vector3 deltaT)
    {
        Vector3 adjustedDelta = deltaT;
        Vector3 targetPos = curPos + deltaT;

        //if (curpos.x + adjustedDelta.x)



        return Vector3.zero;
    }

    private Vector3 CircleRestrict(Vector3 curPos, Vector3 deltaT)
    {

        return Vector3.zero;
    }


    //find the closest valid point to the
    //point starts in object space?
    private Vector3 GetClosestValid(Vector3 pos)
    {
        Matrix4x4 m = transform.worldToLocalMatrix;
        Matrix4x4 mback = transform.localToWorldMatrix;

        Vector3 localPos = m * pos;

        switch (type)
        {
            case AnchorPlaneType.circle:
                {
                    if (localPos.magnitude <= radius)
                    {
                        return pos;                              //we're good, no need to do another matrix mult
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
                return Vector3.zero;
        }

        Vector3 validPos = mback * localPos;
        return validPos;
    }

}
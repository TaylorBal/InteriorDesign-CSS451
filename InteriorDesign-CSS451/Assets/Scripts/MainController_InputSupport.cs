using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class MainController : MonoBehaviour
{
    public enum manipAxis { nullAxis, xAxis, yAxis, zAxis };
    manipAxis curManipAxis;

    private void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectAnObject();
        }
        else if (Input.GetMouseButton(0))
        {
            DragManipulator();
        }
    }

    bool MouseSelectObject(out GameObject obj, out Vector3 point, int mask)
    {
        RaycastHit hit = new RaycastHit();
        bool hitSuccess = Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask);

        if (hitSuccess)
        {
            obj = hit.transform.gameObject;
            point = hit.point;
        }
        else
        {
            obj = null;
            point = Vector3.zero;
        }

        return hitSuccess;
    }

    void SelectAnObject()
    {
        if (eventSystem.IsPointerOverGameObject())
            return;

        GameObject selectedObject;
        Vector3 hitPoint;

        bool axesHit = MouseSelectObject(out selectedObject, out hitPoint, LayerMask.GetMask("Axes"));
        
        if(axesHit)
        {
            ResetAxis();

            axis = selectedObject;
            axisBehavior = axis.GetComponent<AxisBehavior>();
            if(axisBehavior != null)
            {
                // Select axis and save orientation
                switch (axisBehavior.Select())
                {
                    case 1:
                        curManipAxis = manipAxis.xAxis;
                        break;
                    case 2:
                        curManipAxis = manipAxis.yAxis;
                        break;
                    case 3:
                        curManipAxis = manipAxis.zAxis;
                        break;
                    default:
                        curManipAxis = manipAxis.nullAxis;
                        break;
                }
            }
            return;
        }

        //see if we hit any furniture
        bool hit = MouseSelectObject(out selectedObject, out hitPoint, LayerMask.GetMask("Furniture"));

        //we hit a piece of furniture
        if (hit) //hit vertex and axes
        {
            //otherwise lets attach a Manipulator to this furniture
			Furniture selectedFurniture = selectedObject.GetComponent<Furniture>();

            if(selectedFurniture == null)
            {
                Debug.Log("selectedFurniture = null!");
            }
            ResetManipulator();
            manipulator.Select(selectedFurniture);
            selectedFurniture.AxisFrame = manipulator.transform;
            return;
        }

        //if we didn't hit anything deselect the furniture
        //Deselect any previous selection
        ResetManipulator();
    }

    private void DragManipulator()
    {
        if (eventSystem.IsPointerOverGameObject())
            return;

        //find the delta mouse
        Vector3 deltaMouse;
        deltaMouse.x = Input.GetAxis("Mouse X");
        deltaMouse.y = Input.GetAxis("Mouse Y");
        deltaMouse.z = Input.GetAxis("Mouse ScrollWheel");     //Input.mouseposition only stores in x, y

        Vector3 mousePos = Input.mousePosition;
        //find a vector in world space corresponding to the deltaMouse
        Vector3 vEnd = MainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, MainCamera.nearClipPlane));
        Vector3 vStart = MainCamera.ScreenToWorldPoint(new Vector3(mousePos.x - 100 * deltaMouse.x, mousePos.y - 100 * deltaMouse.y, MainCamera.nearClipPlane));
        Vector3 worldDir = vEnd - vStart;

        if (manipulator == null)
            return;

        switch (curManipAxis)
        {
            case manipAxis.xAxis:
                manipulator.MoveX(worldDir);
                break;
            case manipAxis.yAxis:
                manipulator.MoveY(worldDir);
                break;
            case manipAxis.zAxis:
                manipulator.MoveZ(worldDir);
                break;
            case manipAxis.nullAxis:
                break;
        }
    }

    void ResetAxis()
    {
        if(axisBehavior != null)
        {
            axisBehavior.Deselect();
            axisBehavior = null;
            axis = null;
        }
    }

    void ResetManipulator()
    {
        ResetAxis();

        if(manipulator != null)
        {
            manipulator.Deselect();
        }
    }

}

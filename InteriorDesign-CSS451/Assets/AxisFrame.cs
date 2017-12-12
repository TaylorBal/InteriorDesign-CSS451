using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisFrame : MonoBehaviour {

    public GameObject axisX;
    public GameObject axisY;
    public GameObject axisZ;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetActiveState(bool[] enabled)
    {
        axisX.SetActive(enabled[0]);
        axisY.SetActive(enabled[1]);
        axisZ.SetActive(enabled[2]);
    }
}

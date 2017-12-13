using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatNameIndicator : MonoBehaviour {

    private Material curMat = null;
    private Text theText = null;
	// Use this for initialization
	void Start () {
        theText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMat(Material newMat)
    {
        curMat = newMat;

        if(newMat == null)
        {
            theText.text = "Material: ";
        }
        else
        {
            theText.text = "Material: " + newMat.name;
        }
    }
}

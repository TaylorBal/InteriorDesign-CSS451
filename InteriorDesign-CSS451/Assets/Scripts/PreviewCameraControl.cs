using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCameraControl : MonoBehaviour {

    GameObject previewObject = null;
    Vector3 offset = new Vector3(0, -1, -4);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SnapObjectToCamera(GameObject previewObject)
    {
        previewObject.transform.localPosition = transform.localPosition + offset;
    }
}

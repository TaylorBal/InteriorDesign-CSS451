using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewCameraControl : MonoBehaviour {

    GameObject previewObject = null;
    GameObject originalObject = null;
    Vector3 offset = new Vector3(0, -1, -4);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Clone object in preview layer.
    /// </summary>
    /// <param name="inObject"></param>
    public void OpenPreviewObject(GameObject inObject)
    {
        originalObject = inObject;
        
        
    }

    /// <summary>
    /// Apply any valid changes. Destroy preview clone.
    /// </summary>
    public void ClosePreviewObject()
    {

    }
}

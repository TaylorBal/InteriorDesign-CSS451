using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDetector : MonoBehaviour {

    public Furniture parent = null;
    public BoxCollider detectionCollider;
    public Vector3 detectorOffset = Vector3.zero;

	// Use this for initialization
	void Start () {
        Debug.Assert(parent != null);

        detectionCollider = GetComponent<BoxCollider>();
	}

    public void SetCenter(Vector3 center)
    {
        detectionCollider.center = center + detectorOffset;
    }

    public void SetSize(Vector3 size)
    {
        detectionCollider.size = size;
    }

    public Furniture GetParent()
    {
        return parent;
    }
}

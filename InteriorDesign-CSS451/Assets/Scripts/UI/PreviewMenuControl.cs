using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewMenuControl : MonoBehaviour {

    public Button ApplyButton;
    public Button ChangeTexButton;
    public Button DeleteButton;

    public delegate void ApplyCallbackDelegate();
    public delegate void ChangeTexCallbackDelegate();
    public delegate void DeleteCallbackDelegate();

    private ApplyCallbackDelegate applyCallback = null;
    private ChangeTexCallbackDelegate texCallback = null;
    private DeleteCallbackDelegate delCallback = null;

    // Use this for initialization
    void Start () {
        ApplyButton.onClick.AddListener(ApplyChanges);
        DeleteButton.onClick.AddListener(DeleteFurniture);
        ChangeTexButton.onClick.AddListener(ChangeTex);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetApplyListener(ApplyCallbackDelegate listener)
    {
        applyCallback = listener;
    }

    public void SetTexListener(ChangeTexCallbackDelegate listener)
    {
        texCallback = listener;
    }

    public void SetDeleteListener(DeleteCallbackDelegate listener)
    {
        delCallback = listener;
    }

    public void ApplyChanges()
    {
        if(applyCallback != null)
        {
            applyCallback();
        }
    }

    public void ChangeTex()
    {
        if (texCallback != null)
        {
            texCallback();
        }
    }

    public void DeleteFurniture()
    {
        if (delCallback != null)
            delCallback();
    }


}

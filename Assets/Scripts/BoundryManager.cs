using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundryManager : MonoBehaviour {

    public GameObject Right;
    public GameObject Left;
    private int prevWidth;
	// Use this for initialization
	void Start () {
        int prevWidth = Camera.main.pixelWidth;
        adjustSize();
    }
	
	// Update is called once per frame
	void Update () {
        if (prevWidth != Camera.main.pixelWidth) {
            prevWidth = Camera.main.pixelWidth;
            adjustSize();
        }
		
	}

    void adjustSize() {
        Vector3 posR = Right.transform.position;
        int pixelWidth = Camera.main.pixelWidth;
        posR.x = pixelWidth * Manager.unitsPerPixel() / 2f + Right.transform.localScale.x / 4f;
        Right.transform.position = posR;

        Vector3 posL = Left.transform.position;
        posL.x = -pixelWidth * Manager.unitsPerPixel() / 2f - Left.transform.localScale.x / 4f;
        Left.transform.position = posL;
    }
}

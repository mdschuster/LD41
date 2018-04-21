using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    GameObject[] blocks;
    int numBlocks;
    int cols=6;
    int rows=1;

	// Use this for initialization
	void Start () {
        numBlocks = this.transform.childCount;
        blocks = new GameObject[numBlocks];
        for (int i = 0; i < numBlocks; i++) {
            blocks[i] = this.transform.GetChild(i).gameObject;
        }

        arrange();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void arrange() {
        float colWidth = Camera.main.pixelWidth / (cols*1.0f+6f);
        Vector3 blockScale = this.transform.localScale;
        blockScale.x = colWidth * Manager.unitsPerPixel()/1.5f;
        for (int j = 0; j < rows; j++) {
            for (int i = 0; i < cols; i++) {
                GameObject theBlock = blocks[j * cols + i];
                theBlock.transform.localScale = blockScale;
                float xpos = ((i - cols / 2f) * colWidth) * Manager.unitsPerPixel()+theBlock.transform.localScale.x/2f;
                float ypos=4f;
                Vector2 pos = new Vector2(xpos, ypos);
                if (j * cols + i >= numBlocks) return;
                theBlock.transform.position = pos;

            }
        }
    }


}

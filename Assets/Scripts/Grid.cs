using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    GameObject[] blocks;
    public GameObject blockPrefab;
    int numBlocks;
    int rows;
    int[] cols;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setup() {

        cols = Manager.loadLevelLayout();
        rows = cols.Length;

        int numBlocks = 0;
        for (int i = 0; i < cols.Length; i++) {
            numBlocks += cols[i];
        }
        for (int i = 0; i < numBlocks; i++) {
            GameObject temp = Instantiate(blockPrefab);
            temp.transform.parent = this.transform;
        }

        blocks = new GameObject[numBlocks];
        for (int i = 0; i < numBlocks; i++) {
            blocks[i] = this.transform.GetChild(i).gameObject;
        }

        arrange();
    }

    public void arrange() {


        float rowWidth = blocks[0].transform.localScale.y + 0.1f;
        int numBlocks = 0;
        for (int i = 0; i < cols.Length; i++) {
            numBlocks += cols[i];
        }
        int count = 0;
        for (int j = 0; j < rows; j++) {
            //float colWidth = Camera.main.pixelWidth / (cols[j] * 1.0f);
            //200 pix with 150 per unit on block sprite (get sprite width below)
            float spriteWidth=blocks[0].GetComponent<Renderer>().bounds.size.x+0.5f;
            float colWidth = spriteWidth;
            float totalWidth = (colWidth) * cols[j];
            for (int i = 0; i < cols[j]; i++) {
                GameObject theBlock = blocks[count];
                float xpos = (i * colWidth) + spriteWidth/2f-totalWidth/2f ;// -centerShift/2f;// + (theBlock.transform.localScale.x + 0.5f) / 2f-centerShift;
                float ypos = ((j - rows / 2f+4f) * rowWidth) + theBlock.transform.localScale.y / 2f;
                Vector2 pos = new Vector2(xpos, ypos);
                if (count >= numBlocks) return;
                theBlock.transform.position = pos;
                count++;

            }
        }
    }


    public int Rows {
        get {
            return rows;
        }

        set {
            rows = value;
        }
    }

    public int[] Cols {
        get {
            return cols;
        }

        set {
            cols = value;
        }
    }
}

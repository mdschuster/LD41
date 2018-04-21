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
        Vector3 blockScale = this.transform.localScale;
        int numBlocks = 0;
        for (int i = 0; i < cols.Length; i++) {
            numBlocks += cols[i];
        }
        int count = 0;
        for (int j = 0; j < rows; j++) {
            float colWidth = Camera.main.pixelWidth / (cols[j] * 1.0f + 6f);
            blockScale.x = colWidth * Manager.unitsPerPixel() / 1.5f;
            for (int i = 0; i < cols[j]; i++) {
                GameObject theBlock = blocks[count];
                theBlock.transform.localScale = blockScale;
                float xpos = ((i - cols[j] / 2f) * colWidth) * Manager.unitsPerPixel()+theBlock.transform.localScale.x/2f;
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

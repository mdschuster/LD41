using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour {
     
    //for text grid
    GameObject textGrid;
    GameObject Paddle;
    Text[] theText;
    int prevH;
    int prevW;

	// Use this for initialization
	void Start () {
        textGrid = GameObject.Find("TextGrid");
        Paddle = GameObject.Find("Paddle");
        textGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Camera.main.pixelWidth / 6f, Camera.main.pixelHeight / 12f);
        theText = textGrid.GetComponentsInChildren<Text>();
        theText[0].text = "Zero";
        theText[1].text = "One";
        theText[2].text = "Two";
        theText[3].text = "Three";
        theText[4].text = "Four";
        theText[5].text = "Five";


        float screenWidth=Camera.main.pixelWidth / 6f;
        Vector3 paddleScale = Paddle.transform.localScale;
        paddleScale.x = screenWidth * unitsPerPixel();
        Paddle.transform.localScale = paddleScale;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (prevH != Camera.main.pixelHeight || prevW != Camera.main.pixelWidth) {
            //change the text grid to still be proportional to the screensize
            textGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Camera.main.pixelWidth / 6f, Camera.main.pixelHeight / 12f);

            //change paddle size
            float screenWidth = Camera.main.pixelWidth / 6f;
            Vector3 paddleScale = Paddle.transform.localScale;
            paddleScale.x = screenWidth * unitsPerPixel();
            Paddle.transform.localScale = paddleScale;
        }



    }


    public static float unitsPerPixel() {
        var p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        return Vector3.Distance(p1, p2);
    }

    public static float pixelsPerUnit() {
        return 1 / unitsPerPixel();
    }
}

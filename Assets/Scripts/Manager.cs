using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour {
     
    //for text grid
    GameObject textGrid;
    GameObject thePaddle;
    InputField theInput;
    Text[] theText;
    int prevH;
    int prevW;
    bool sizeFlag = true;

    public enum Phases {BALL_ATTACHED, BALL_MOVING, PAUSED};
    public Phases currentPhase = new Phases();

	// Use this for initialization
	void Start () {

        currentPhase = Phases.BALL_MOVING;
        prevH = Camera.main.pixelHeight;
        prevW = Camera.main.pixelWidth;
        textGrid = GameObject.Find("TextGrid");
        thePaddle = GameObject.Find("Paddle");
        theInput = thePaddle.GetComponentInChildren<InputField>();
        textGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Camera.main.pixelWidth / 6f, Camera.main.pixelHeight / 12f);
        theText = textGrid.GetComponentsInChildren<Text>();
        theText[0].text = "Zero";
        theText[1].text = "One";
        theText[2].text = "Two";
        theText[3].text = "Three";
        theText[4].text = "Four";
        theText[5].text = "Five";


        float screenWidth=Camera.main.pixelWidth / 6f;
        Vector3 paddleScale = thePaddle.transform.localScale;
        paddleScale.x = screenWidth * unitsPerPixel();
        thePaddle.transform.localScale = paddleScale;

        positionPaddle(3);
    }
	
	// Update is called once per frame
	void Update () {

        theInput.ActivateInputField();

        if (prevH != Camera.main.pixelHeight || prevW != Camera.main.pixelWidth || sizeFlag == true) {
            prevH = Camera.main.pixelHeight;
            prevW = Camera.main.pixelWidth;
            //change the text grid to still be proportional to the screensize
            textGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Camera.main.pixelWidth / 6f, Camera.main.pixelHeight / 12f);

            //change paddle size
            float screenWidth = Camera.main.pixelWidth / 6f;
            Vector3 paddleScale = thePaddle.transform.localScale;
            paddleScale.x = screenWidth * unitsPerPixel();
            thePaddle.transform.localScale = paddleScale;
            sizeFlag = false;
        }



    }

    public void positionPaddle(int num) {
        Vector3 pos = thePaddle.transform.position;
        //the num-3 assumes 6 words
        pos.x = (num-3)*Camera.main.pixelWidth / 6f * unitsPerPixel() + thePaddle.transform.localScale.x / 2f;
        thePaddle.transform.position = pos;
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

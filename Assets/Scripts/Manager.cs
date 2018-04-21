using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour {
     
    //for text grid
    GameObject textGrid;
    GameObject thePaddle;
    InputField theInput;
    GameObject theBall;
    Text[] theText;
    GameObject goLives;
    GameObject goScore;
    int score=0;
    int lives=3;
    int prevH;
    int prevW;
    bool sizeFlag = true;
    string input = null;
    static float numWords = 4;

    public enum Phases {BALL_ATTACHED, BALL_MOVING, PAUSED};
    public Phases currentPhase = new Phases();

	// Use this for initialization
	void Start () {

        currentPhase = Phases.BALL_ATTACHED;
        prevH = Camera.main.pixelHeight;
        prevW = Camera.main.pixelWidth;
        textGrid = GameObject.Find("TextGrid");
        thePaddle = GameObject.Find("Paddle");
        theBall = GameObject.Find("Ball");
        goLives = GameObject.Find("Lives");
        goScore = GameObject.Find("Score");

        theInput = thePaddle.GetComponentInChildren<InputField>();
        textGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Camera.main.pixelWidth / numWords, Camera.main.pixelHeight / 12f);
        theText = textGrid.GetComponentsInChildren<Text>();
        theText[0].text = "Zero";
        theText[1].text = "One";
        theText[2].text = "Two";
        theText[3].text = "Three";



        float screenWidth=Camera.main.pixelWidth / numWords;
        Vector3 paddleScale = thePaddle.transform.localScale;
        paddleScale.x = screenWidth * unitsPerPixel();
        thePaddle.transform.localScale = paddleScale;

        positionPaddle(3);

        goLives.GetComponent<Text>().text = "Lives: " + lives;
        goScore.GetComponent<Text>().text = "Score: " + score;

    }

    // Update is called once per frame
    void Update () {

        activateInput(true);

        if (prevH != Camera.main.pixelHeight || prevW != Camera.main.pixelWidth || sizeFlag == true) {
            prevH = Camera.main.pixelHeight;
            prevW = Camera.main.pixelWidth;
            //change the text grid to still be proportional to the screensize
            textGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Camera.main.pixelWidth / numWords, Camera.main.pixelHeight / 12f);

            //change paddle size
            float screenWidth = Camera.main.pixelWidth / numWords;
            Vector3 paddleScale = thePaddle.transform.localScale;
            paddleScale.x = screenWidth * unitsPerPixel();
            thePaddle.transform.localScale = paddleScale;
            sizeFlag = false;
        }


        movePaddle();



    }

    public void activateInput(bool state) {
        if (state == true) {
            theInput.ActivateInputField();
            theInput.interactable = true;
        } else {
            theInput.DeactivateInputField();
            theInput.interactable = false;
        }
    }

    public void movePaddle() {
        if (input == null || input=="") {
            return;
        }
        input = input.Trim();
        for (int i = 0; i < theText.Length; i++) {
            if (input.ToLower().Equals(theText[i].text.ToLower())) {
                Vector3 pos = positionPaddle(i);
                thePaddle.GetComponent<PaddleMover>().updateTargetPosition(pos);
                theInput.text = "";
                input = null;
                
                return;
            }
        }

        if (input.ToLower().Equals("faster")) {
            theBall.GetComponent<Mover>().speed++;
            theInput.text = "";
            input = null;
            return;
        }
        if (input.ToLower().Equals("slower")) {
            theBall.GetComponent<Mover>().speed--;
            theInput.text = "";
            input = null;
            return;
        }

        //for they typed the wrong thing!
        input = null;

    }

    public Vector3 positionPaddle(int num) {
        Vector3 pos = thePaddle.transform.position;
        //the num-* assumes 6 words
        pos.x = (num-numWords/2f)*Camera.main.pixelWidth / numWords * unitsPerPixel() + thePaddle.transform.localScale.x / 2f;
        //thePaddle.transform.position = pos;
        return pos;
    }


    public void readInput(InputField input) {
        this.input = input.text;
    }

    public void updateScore(int num) {
        score += num;
        goScore.GetComponent<Text>().text = "Score: " + score;
    }

    public void updateLives(int num) {
        lives += num;
        goLives.GetComponent<Text>().text = "Lives: " + lives;
    }

    public int getLives() {
        return lives;
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

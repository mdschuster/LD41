using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour {
     
    //for text grid
    GameObject textGrid;
    GameObject thePaddle;
    InputField theInput;
    public GameObject theBall;
    GameObject theBlockGrid;
    Text[] theText;
    GameObject goLives;
    GameObject goScore;
    public Text centerText;
    int score=0;
    int lives=3;
    int prevH;
    int prevW;
    bool sizeFlag = true;
    string input = null;
    static float numWords = 4;
    public static int level;

    //sound
    public AudioSource source;
    public AudioClip[] bounce;
    public AudioClip music;

    public enum Phases {BALL_ATTACHED, BALL_MOVING, PAUSED, LEVEL_COMPLETE,GAME_OVER};
    public Phases currentPhase = new Phases();
    public Phases prevPhase = new Phases();

	// Use this for initialization
	void Start () {

        level = 1; 
        currentPhase = Phases.BALL_ATTACHED;
        prevH = Camera.main.pixelHeight;
        prevW = Camera.main.pixelWidth;
        textGrid = GameObject.Find("TextGrid");
        thePaddle = GameObject.Find("Paddle");
        theBall = GameObject.Find("Ball");
        goLives = GameObject.Find("Lives");
        goScore = GameObject.Find("Score");
        theBlockGrid = GameObject.Find("BlockGrid");
        centerText = GameObject.Find("CenterText").GetComponent<Text>();

        centerText.text = "";

        theInput = thePaddle.GetComponentInChildren<InputField>();
        textGrid.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Camera.main.pixelWidth / numWords, Camera.main.pixelHeight / 12f);
        theText = textGrid.GetComponentsInChildren<Text>();
        theText[0].text = "One";
        theText[1].text = "";
        theText[2].text = "";
        theText[3].text = "Four";



        float screenWidth=Camera.main.pixelWidth / numWords;
        Vector3 paddleScale = thePaddle.transform.localScale;
        paddleScale.x = screenWidth * unitsPerPixel();
        thePaddle.transform.localScale = paddleScale;

        positionPaddle(3);

        goLives.GetComponent<Text>().text = "Lives: " + lives;
        goScore.GetComponent<Text>().text = "Score: " + score;
        theBlockGrid.GetComponent<Grid>().setup();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (currentPhase == Phases.PAUSED) {
                currentPhase = prevPhase;
                centerText.text = "";
                activateInput(true);
                return;
            }
            prevPhase = currentPhase;
            currentPhase = Phases.PAUSED;
            centerText.text = "Paused";
            activateInput(false);
            return;
        }

        if(currentPhase == Phases.LEVEL_COMPLETE) {
            if (Input.GetKeyUp(KeyCode.Space)){
                theBlockGrid.GetComponent<Grid>().setup();
                centerText.text = "";
                currentPhase = Phases.BALL_ATTACHED;
                activateInput(true);
            }
            return;
        }

        if(currentPhase == Phases.GAME_OVER) {
            if (Input.GetKeyUp(KeyCode.Space)) {

                Debug.Log(theBlockGrid.transform.childCount);
                level = 1;
                theBlockGrid.GetComponent<Grid>().setup();
                centerText.text = "";
                currentPhase = Phases.BALL_ATTACHED;
                activateInput(true);
                score = 0;
                lives = 3;
                updateScore(0);
                updateLives(0);


            }
            return;
        }

        theInput.ActivateInputField();

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

        levelUp();
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
        Vector3 pos=this.transform.position;
        if (input.ToLower().Equals(theText[0].text.ToLower())) {
            pos = positionPaddle(0);
        }
        if (input.ToLower().Equals(theText[theText.Length-1].text.ToLower())) {
            pos = positionPaddle(theText.Length-1);
        }

        if (!pos.Equals(this.transform.position)) {
            thePaddle.GetComponent<PaddleMover>().updateTargetPosition(pos);
            theInput.text = "";
            input = null;
            return;
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
        if (input.ToLower().Equals("win")) {
            for (int i = 0; i < theBlockGrid.transform.childCount; i++) {
                Destroy(theBlockGrid.transform.GetChild(i).gameObject);
            }
            theInput.text = "";
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

    public void levelUp() {
        if (theBlockGrid.transform.childCount != 0) {
            return;
        }
        level = level + 1;
        if (level > 10) {
            level = 1;
        }
        centerText.text="Level Complete \n Press Space To Continue...";
        currentPhase = Phases.LEVEL_COMPLETE;
        theBall.GetComponentInChildren<ParticleSystem>().Stop();
        theBall.GetComponent<Mover>().vel = Vector3.zero;
        activateInput(false);
    }

    public static float unitsPerPixel() {
        var p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        return Vector3.Distance(p1, p2);
    }

    public static float pixelsPerUnit() {
        return 1 / unitsPerPixel();
    }

    public void gameOver() {
        for (int i = 0; i < theBlockGrid.transform.childCount; i++) {
            Destroy(theBlockGrid.transform.GetChild(i).gameObject);
        }
        currentPhase = Manager.Phases.GAME_OVER;
        centerText.text = "Game Over \n Press Space To Try Again";
        theBall.GetComponentInChildren<ParticleSystem>().Stop();
        theBall.GetComponent<Mover>().vel = Vector3.zero;
        activateInput(false);
    }

    //HACK maybe put this somewhere else
    public static int[] loadLevelLayout() {
        int[] cols=null;
        if (level == 1) {
            cols = new int[3];
            cols[0] = 4;
            cols[1] = 4;
            cols[2] = 4;
        }
        if (level == 2) {
            cols = new int[4];
            cols[0] = 4;
            cols[1] = 4;
            cols[2] = 2;
            cols[3] = 3;
        }
        if (level == 3) {
            cols = new int[5];
            cols[0] = 5;
            cols[1] = 2;
            cols[2] = 2;
            cols[3] = 1;
            cols[4] = 5;
        }
        if (level == 4) {
            cols = new int[6];
            cols[0] = 2;
            cols[1] = 3;
            cols[2] = 4;
            cols[3] = 5;
            cols[4] = 6;
            cols[4] = 2;
        }
        if (level == 5) {
            cols = new int[5];
            cols[0] = 5;
            cols[1] = 5;
            cols[2] = 5;
            cols[3] = 1;
            cols[4] = 3;
        }
        if (level == 6) {
            cols = new int[4];
            cols[0] = 6;
            cols[1] = 1;
            cols[2] = 6;
            cols[3] = 2;
        }
        if (level == 7) {
            cols = new int[3];
            cols[0] = 6;
            cols[1] = 5;
            cols[2] = 6;
        }
        if (level == 8) {
            cols = new int[5];
            cols[0] = 2;
            cols[1] = 2;
            cols[2] = 2;
            cols[3] = 2;
            cols[4] = 6;
        }
        if (level == 9) {
            cols = new int[4];
            cols[0] = 1;
            cols[1] = 1;
            cols[2] = 6;
            cols[3] = 7;
        }
        if (level == 10) {
            cols = new int[5];
            cols[0] = 1;
            cols[1] = 2;
            cols[2] = 3;
            cols[3] = 4;
            cols[4] = 5;
        }
        if (cols == null){
            //something went wrong
            Debug.LogError("Level Load Error: Level Not Found");
        }
        return cols;
    }

}

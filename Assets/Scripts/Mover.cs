using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    private bool isEnabled = true;
    public Vector2 vel;
    private Vector2 dir;
    private Manager theManager;
    public GameObject thePaddle;
    private float speed;

    // Use this for initialization
    void Start() {
        speed = 5f;
        dir = new Vector2(0f, -1f);
        vel = new Vector2(1f, 1f).normalized * speed;
        theManager = Camera.main.GetComponent<Manager>();
        thePaddle = GameObject.Find("Paddle");
    }

    // Update is called once per frame
    void Update() {
        if (theManager.currentPhase==Manager.Phases.PAUSED) {
            return;
        }
        if (theManager.currentPhase == Manager.Phases.BALL_ATTACHED) {
            //stay attached to the center of the paddle
            Vector3 pos = this.transform.position;
            pos.y = thePaddle.transform.position.y;
            pos.y += (this.transform.localScale.y + thePaddle.transform.localScale.y)/2f+0.01f;
            this.transform.position = pos;

            //click to send off in a direction, for now
            sendOff();

            return;
        }

        move();

    }

    private void move() {
        Vector2 pos = this.transform.position;
        Vector2 newPos = new Vector2(0f, 0f);
        checkBoundry(pos);
        //x
        newPos.x = pos.x + vel.x * Time.deltaTime;
        //y
        newPos.y = pos.y + vel.y * Time.deltaTime;

        this.transform.position = newPos;
    }

    private void checkBoundry(Vector2 myPos) {

        //Debug.Log(pixScale+" "+myPos);
        Vector2 pos = Camera.main.WorldToScreenPoint(myPos);
        if (pos.y < 0+110) {
            vel.y = -vel.y;
        } else if (pos.y > Camera.main.pixelHeight - this.transform.localScale.y/2f*Manager.pixelsPerUnit()) {
            vel.y = -vel.y;
        } else if (pos.x < 0+ this.transform.localScale.y / 2f * Manager.pixelsPerUnit()) {
            vel.x = -vel.x;
        } else if (pos.x > Camera.main.pixelWidth - this.transform.localScale.x/2f * Manager.pixelsPerUnit()) {
            vel.x = -vel.x;
        }
    }

    private void sendOff() {
        if(Input.GetMouseButtonDown(0)) { //leftclick
            Vector2 newVel = Vector2.one;
            newVel.x = -newVel.x;
            vel = newVel.normalized * speed;
            theManager.currentPhase = Manager.Phases.BALL_MOVING;
        }
        if (Input.GetMouseButtonDown(1)) { //rightclick
            Vector2 newVel = Vector2.one;
            vel = newVel.normalized * speed;
            theManager.currentPhase = Manager.Phases.BALL_MOVING;
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Hit");
        vel.y = -vel.y;
        vel.x = vel.x;
        if (other == thePaddle.GetComponent<Collider>()) {
            //theManager.currentPhase = Manager.Phases.BALL_ATTACHED;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public Vector3 vel;
    private Manager theManager;
    public GameObject thePaddle;
    public float speed;

    // Use this for initialization
    void Start() {
        speed = 7f;
        vel = new Vector3(1f, 1f,0f).normalized * speed;
        theManager = Camera.main.GetComponent<Manager>();
        thePaddle = GameObject.Find("Paddle");
    }

    // Update is called once per frame
    void Update() {

        if (theManager.currentPhase == Manager.Phases.BALL_ATTACHED) {
            //theManager.activateInput(false);
            //stay attached to the center of the paddle
            Vector3 pos = this.transform.position;
            pos = thePaddle.transform.position;
            pos.y += (this.transform.localScale.y + thePaddle.transform.localScale.y)/2f+0.01f;
            this.transform.position = pos;
            this.vel = Vector3.zero;

            if(Input.GetKeyDown(KeyCode.Space)) {
                //release ball
                int num = Random.Range(0, 2);
                if (num == 0) {
                    vel= new Vector3(1f, 1f, 0f).normalized * speed;
                } else {
                    vel = new Vector3(-1f, 1f, 0f).normalized * speed;
                }
                theManager.currentPhase = Manager.Phases.BALL_MOVING;
                theManager.activateInput(true);
                this.GetComponentInChildren<ParticleSystem>().Play();
            }

            return;
        }


    }

    void FixedUpdate() {
        if (theManager.currentPhase != Manager.Phases.BALL_MOVING) {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }
        move();
    }

    private void move() {
        Vector2 pos = this.transform.position;
        Vector2 newPos = new Vector2(0f, 0f);
        //checkBoundry(pos);
        vel = vel.normalized * speed;
        this.GetComponent<Rigidbody>().velocity = vel;
        //x
        newPos.x = pos.x + vel.x * Time.deltaTime;
        //y
        newPos.y = pos.y + vel.y * Time.deltaTime;

        //this.transform.position = newPos;
    }

    //private void checkBoundry(Vector2 myPos) {

    //    //Debug.Log(pixScale+" "+myPos);
    //    Vector2 pos = myPos;
    //    float c = Manager.unitsPerPixel();
    //    if (pos.y < 0 + 110 * c) {
    //        vel = Vector3.Reflect(vel, Vector3.up);
    //        pos.y =110*c+shift;
    //    } else if (pos.y > Camera.main.pixelHeight*c - this.transform.localScale.y/2f) {
    //        vel = Vector3.Reflect(vel, Vector3.down);
    //        //vel.y = -vel.y;
    //        pos.y = Camera.main.pixelHeight*c - this.transform.localScale.y / 2f+shift;

    //    } else if (pos.x < 0+ this.transform.localScale.y / 2f) {
    //        vel = Vector3.Reflect(vel, Vector3.right);
    //        //vel.x = -vel.x;
    //        pos.x = this.transform.localScale.y / 2f+shift;

    //    } else if (pos.x > Camera.main.pixelWidth*c - this.transform.localScale.x/2f) {
    //        vel = Vector3.Reflect(vel, Vector3.left);
    //        pos.x = Camera.main.pixelWidth*c - this.transform.localScale.x / 2f+shift;

    //        //vel.x = -vel.x;
    //        // pos.x = Camera.main.pixelWidth - this.transform.localScale.x / 2f * Manager.pixelsPerUnit();
    //    }
    //    this.transform.position = pos;
    //}

    //FIXME something is still up with the collisions and bound checks
    void OnCollisionEnter(Collision other) {
        if(theManager.currentPhase != Manager.Phases.BALL_MOVING) {
            return;
        }

        vel = Vector3.Reflect(vel, -other.contacts[0].normal);
        Vector3 pos = this.transform.position;
        //pos += other.contacts[0].normal*shift;
        //apply velocity modifier
        float velMod = other.gameObject.GetComponent<VelocityModifier>().getVelMod();
        Vector3 addVel = other.gameObject.GetComponent<VelocityModifier>().addVel();
        vel = vel.normalized * speed * velMod + addVel;

        if (Mathf.Abs(vel.normalized.y) < 0.1) {
            //kick it!
            vel.y *= 10f;
            vel = vel.normalized * speed;
        } else if (Mathf.Abs(vel.normalized.x) < 0.1) {
            //kick it!
            vel.x *= 10f;
            vel = vel.normalized * speed;
        }


        //sounds
        if (other.gameObject.Equals(thePaddle)) {
            theManager.source.clip = theManager.bounce[1];
            theManager.source.Play();
        } else if (other.gameObject.tag == "Boundry") {
            theManager.source.clip = theManager.bounce[0];
            theManager.source.Play();
        } else {
            theManager.source.clip = theManager.bounce[2];
            theManager.source.Play();
        }


        //don't go over max speed
        if (vel.normalized.magnitude<speed) {
            vel = vel.normalized * speed;
        }

        this.transform.position = pos;
        if (!other.gameObject.Equals(thePaddle) && other.gameObject.tag!="Boundry") {
            theManager.updateScore(1);
            Destroy(other.gameObject);
        }
        if (other.gameObject.Equals(thePaddle)) {

        }
        if (other.gameObject.Equals(GameObject.Find("Bottom"))) {
            theManager.updateLives(-1);
            if (theManager.getLives() == 0) {
                theManager.gameOver();
                Debug.Log("Game Over Man");
                return;
            }
            theManager.currentPhase = Manager.Phases.BALL_ATTACHED;
            this.GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    void OnCollisionExit(Collision other) {
    }

    void OnCollisionStay(Collision other) {
        //Vector3 pos = this.transform.position;
        //pos += other.contacts[0].normal * 0.1f;
        //this.transform.position = pos;
    }
}

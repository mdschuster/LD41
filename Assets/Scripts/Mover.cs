using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public Vector3 vel;
    private Manager theManager;
    public GameObject thePaddle;
    public float speed;
    private float shift = 0.5f;

    // Use this for initialization
    void Start() {
        speed = 5f;
        vel = new Vector3(1f, 1f,0f).normalized * speed;
        theManager = Camera.main.GetComponent<Manager>();
        thePaddle = GameObject.Find("Paddle");
    }

    // Update is called once per frame
    void Update() {

        if (theManager.currentPhase == Manager.Phases.BALL_ATTACHED) {
            //stay attached to the center of the paddle
            Vector3 pos = this.transform.position;
            pos = thePaddle.transform.position;
            pos.y += (this.transform.localScale.y + thePaddle.transform.localScale.y)/2f+0.01f;
            this.transform.position = pos;
            this.vel = Vector3.zero;

            if(Input.GetKeyDown(KeyCode.Space)) {
                //release ball
                int num = Random.Range(0, 1);
                if (num == 0) {
                    vel= new Vector3(1f, 1f, 0f).normalized * speed;
                } else {
                    vel = new Vector3(-1f, 1f, 0f).normalized * speed;
                }
                theManager.currentPhase = Manager.Phases.BALL_MOVING;
            }

            return;
        }


    }

    void FixedUpdate() {
        if (theManager.currentPhase == Manager.Phases.PAUSED || theManager.currentPhase == Manager.Phases.BALL_ATTACHED) {
            return;
        }
        move();
    }

    private void move() {
        Vector2 pos = this.transform.position;
        Vector2 newPos = new Vector2(0f, 0f);
        //checkBoundry(pos);
        vel = vel.normalized * speed;
        //x
        newPos.x = pos.x + vel.x * Time.deltaTime;
        //y
        newPos.y = pos.y + vel.y * Time.deltaTime;

        this.transform.position = newPos;
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
        vel = Vector3.Reflect(vel, -other.contacts[0].normal);
        Vector3 pos = this.transform.position;
        //pos += other.contacts[0].normal*shift;
        //apply velocity modifier
        float velMod = other.gameObject.GetComponent<VelocityModifier>().getVelMod();
        Vector3 addVel = other.gameObject.GetComponent<VelocityModifier>().addVel();
        vel = vel.normalized * speed * velMod + addVel;

        if(vel.normalized.magnitude<speed) {
            vel = vel.normalized * speed;
        }

        this.transform.position = pos;
        if (!other.gameObject.Equals(thePaddle) && other.gameObject.tag!="Boundry") { 
            //TODO increase score
            Destroy(other.gameObject);
        }
        if (other.gameObject.Equals(thePaddle)) {

        }
        if (other.gameObject.Equals(GameObject.Find("Bottom"))) {
            //TODO implement death
            Debug.Log("killed");
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

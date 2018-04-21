using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public Vector3 vel;
    private Manager theManager;
    public GameObject thePaddle;
    private float speed;
    private float shift = 0.1f;

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
            pos.y = thePaddle.transform.position.y;
            pos.y += (this.transform.localScale.y + thePaddle.transform.localScale.y)/2f+0.01f;
            this.transform.position = pos;

            return;
        }

        //move();

    }

    void FixedUpdate() {
        if (theManager.currentPhase == Manager.Phases.PAUSED) {
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
        if (pos.y <= 0+110) {
            vel = Vector3.Reflect(vel, Vector3.up);
            pos.y =110+shift;
        } else if (pos.y >= Camera.main.pixelHeight - this.transform.localScale.y/2f*Manager.pixelsPerUnit()) {
            vel = Vector3.Reflect(vel, Vector3.down);
            //vel.y = -vel.y;
            pos.y = Camera.main.pixelHeight - this.transform.localScale.y / 2f * Manager.pixelsPerUnit()+shift; 
        } else if (pos.x <= 0+ this.transform.localScale.y / 2f * Manager.pixelsPerUnit()) {
            vel = Vector3.Reflect(vel, Vector3.right);
            //vel.x = -vel.x;
            pos.x = this.transform.localScale.y / 2f * Manager.pixelsPerUnit()+shift; 
        } else if (pos.x >= Camera.main.pixelWidth - this.transform.localScale.x/2f * Manager.pixelsPerUnit()) {
            vel = Vector3.Reflect(vel, Vector3.left);
            pos.x = Camera.main.pixelWidth - this.transform.localScale.x / 2f * Manager.pixelsPerUnit()+shift;
            //vel.x = -vel.x;
            // pos.x = Camera.main.pixelWidth - this.transform.localScale.x / 2f * Manager.pixelsPerUnit();
        }
        this.transform.position = pos;
    }

    void OnCollisionEnter(Collision other) {
        Debug.Log("Hit");
        vel = Vector3.Reflect(vel, other.contacts[0].normal);
        Vector3 pos = this.transform.position;
        pos += other.contacts[0].normal*shift;
        this.transform.position = pos;
        if (!other.gameObject.Equals(thePaddle)) { 
            Destroy(other.gameObject);
        }
    }

    void OnCollisionStay(Collision other) {
        Debug.Log("stay");
        Vector3 pos = this.transform.position;
        pos += other.contacts[0].normal * shift;
        this.transform.position = pos;
    }
}

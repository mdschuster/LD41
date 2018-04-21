using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    private bool isEnabled = true;
    public Vector2 vel;
    private Vector2 dir;
    // Use this for initialization
    void Start() {
        dir = new Vector2(0f, -1f);
        vel = new Vector2(0f, -5f);
    }

    // Update is called once per frame
    void Update() {
        if (!isEnabled) {
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
        Vector2 pixScale = Camera.main.WorldToScreenPoint(new Vector2(0,0.5f));
        //TODO: Ball moves halfway out of screen, fix to have ball appear to hit
        //the boundry at edge

        //Debug.Log(pixScale+" "+myPos);
        Vector2 pos = Camera.main.WorldToScreenPoint(myPos);
        if (pos.y < 0) {
            vel.y = -vel.y;
        } else if (pos.y > Camera.main.pixelHeight) {
            vel.y = -vel.y;
        } else if (pos.x < 0) {
            vel.x = -vel.x;
        } else if (pos.x > Camera.main.pixelWidth) {
            vel.x = -vel.x;
        }
    }

            void OnTriggerEnter(Collider other) {
        Debug.Log("Hit");
        vel.y = -vel.y;
        vel.x = -vel.x;
    }
}

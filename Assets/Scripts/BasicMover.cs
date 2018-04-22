using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMover : MonoBehaviour {

    public Vector3 vel;
    public float speed;
    public MenuManager theManager;

    // Use this for initialization
    void Start() {
        speed = 10f;
        vel = new Vector3(0.9f, 1.4f, 0f).normalized * speed;

    }

    // Update is called once per frame
    void Update() {



    }

    void FixedUpdate() {
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

    }



    //FIXME something is still up with the collisions and bound checks
    void OnCollisionEnter(Collision other) {

        vel = Vector3.Reflect(vel, -other.contacts[0].normal);
        Vector3 pos = this.transform.position;
        //pos += other.contacts[0].normal*shift;
        //apply velocity modifier
        float velMod = other.gameObject.GetComponent<VelocityModifier>().getVelMod();
        Vector3 addVel = other.gameObject.GetComponent<VelocityModifier>().addVel();
        vel = vel.normalized * speed * velMod + addVel;

        //random kick
        int rand1 = Random.Range(0, 2);
        int rand2 = Random.Range(1, 11);

        if (rand1 == 0) {
            vel.y *= rand2;
        } else {
            vel.x *= rand2;
        }
        vel = vel.normalized * speed;

        if (Mathf.Abs(vel.normalized.y) < 0.1) {
            //kick it!
            vel.y *= 100f;
            vel = vel.normalized * speed;
        } else if (Mathf.Abs(vel.normalized.x) < 0.1) {
            //kick it!
            vel.x *= 100f;
            vel = vel.normalized * speed;
        }

        //don't go over max speed
        if (vel.normalized.magnitude < speed) {
            vel = vel.normalized * speed;
        }

        this.transform.position = pos;

        theManager.source.clip = theManager.bounce;
        theManager.source.Play();

    }

    void OnCollisionExit(Collision other) {
    }

    void OnCollisionStay(Collision other) {
        //Vector3 pos = this.transform.position;
        //pos += other.contacts[0].normal * 0.1f;
        //this.transform.position = pos;
    }
}

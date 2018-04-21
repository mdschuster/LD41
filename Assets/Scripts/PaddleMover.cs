using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMover : MonoBehaviour, VelocityModifier {

    public bool isAnimating;
    Vector3 targetPosition;
    public Vector3 vel = Vector3.zero;
    Manager theManager;
    float speed;

	// Use this for initialization
	void Start () {
        speed = 15f;
        targetPosition = this.transform.position;
        theManager = Camera.main.GetComponent<Manager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (theManager.currentPhase == Manager.Phases.PAUSED) {
            return;
        }
        if (isAnimating) {
            Vector3 pos = this.transform.position;
            Vector3 dir = targetPosition - pos;
            vel = dir.normalized;
            vel.x = vel.x * speed;
            pos.x += vel.x * Time.deltaTime;
            this.transform.position = pos;
            if (Mathf.Abs(pos.x - targetPosition.x) < 0.2) {
                isAnimating = false;
                vel = Vector3.zero;
            }
        }
	}

    public void updateTargetPosition(Vector3 targetPos) {
        targetPosition = targetPos;
        isAnimating = true;
    }

    public float getVelMod() {
        return 1f;
    }
    public Vector3 addVel() {
        return vel;
    }
}

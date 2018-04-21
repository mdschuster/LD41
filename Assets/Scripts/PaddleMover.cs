using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMover : MonoBehaviour {

    public bool isAnimating;
    Vector3 targetPosition;
    float speed;

	// Use this for initialization
	void Start () {
        speed = 15f;
        targetPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (isAnimating) {
            Vector3 pos = this.transform.position;
            Vector3 dir = targetPosition - pos;
            pos.x += dir.normalized.x * speed * Time.deltaTime;
            this.transform.position = pos;
            if (Mathf.Abs(pos.x - targetPosition.x) < 0.1) {
                isAnimating = false;
            }
        }
	}

    public void updateTargetPosition(Vector3 targetPos) {
        targetPosition = targetPos;
        isAnimating = true;
    }
}

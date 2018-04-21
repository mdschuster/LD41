using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, VelocityModifier {

    public float velMod = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float getVelMod() {
        return velMod;
    }

    public Vector3 addVel() {
        return Vector3.zero;
    }
}

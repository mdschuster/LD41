using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour, VelocityModifier {

    private float velMod = 0.75f;
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

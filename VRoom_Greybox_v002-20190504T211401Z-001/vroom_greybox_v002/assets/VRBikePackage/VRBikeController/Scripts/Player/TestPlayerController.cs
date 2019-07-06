using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Greg Kilmer
 * Function: Allows for keyboard testing of the player.
 * Last Updated: 5/13/2018
 */

public class TestPlayerController : MonoBehaviour {

	public BikeMovement playerBikeMovement;
	public GameObject steering;
	public float leanIncrement;
	public float curLeanAngle;
	public float maxLeanAngle;
	public float SteeringRotationAmount;

	private GameObject bike;

	// Use this for initialization
	void Start () {
		bike = playerBikeMovement.bike;
		curLeanAngle = 0;
	}
	
	// Update is called once per frame
	void Update () {

        playerBikeMovement.Turn(Input.GetAxis("Horizontal"));
        playerBikeMovement.Accelerate(Input.GetAxis("Vertical"));
        Lean(Input.GetAxis("Lean"));

        if (curLeanAngle != 0)
        {
            playerBikeMovement.LeanLeft(curLeanAngle / maxLeanAngle);
        }

        /*
        if (Input.GetKey (KeyCode.W)) {
			playerBikeMovement.Accelerate ();
		}
		if (Input.GetKey (KeyCode.S)) {
			playerBikeMovement.Decelerate ();
		}
		//TURN
		if (Input.GetKey (KeyCode.A)) {
			RepresentBikeTurnLeft ();
		} else if (Input.GetKey (KeyCode.D)) {
			RepresentBikeTurnRight ();
		} else {
			RepresentBikeTurnReset ();
		}
		//TILT
		if (Input.GetKey (KeyCode.Q)) {
			RepresentBikeTiltLeft ();
		} else if (Input.GetKey (KeyCode.E)) {
			RepresentBikeTiltRight ();
		}
		if (curLeanAngle > 0) {
			playerBikeMovement.LeanLeft (curLeanAngle/maxLeanAngle);
		} else if (curLeanAngle < 0) {
			playerBikeMovement.LeanRight (-curLeanAngle/maxLeanAngle);
		}*/

    }

	public void RepresentBikeTiltLeft() {
		if (curLeanAngle + leanIncrement <= maxLeanAngle) {
			Quaternion currentRotation = bike.transform.rotation;
			Vector3 currentAngles = currentRotation.eulerAngles;
			currentAngles.z += leanIncrement;
			curLeanAngle += leanIncrement;
			bike.transform.rotation = Quaternion.Euler (currentAngles);
		}
	}

	public void RepresentBikeTiltRight() {
		if (curLeanAngle - leanIncrement >= -maxLeanAngle) {
			Quaternion currentRotation = bike.transform.rotation;
			Vector3 currentAngles = currentRotation.eulerAngles;
			currentAngles.z -= leanIncrement;
			curLeanAngle -= leanIncrement;
			bike.transform.localRotation = Quaternion.Euler (currentAngles);
		}
	}

    public void Lean(float percent)
    {
        Quaternion currentRotation = bike.transform.rotation;
        Vector3 currentAngles = currentRotation.eulerAngles;
        currentAngles.z = -percent*maxLeanAngle;
        curLeanAngle = -percent * maxLeanAngle;
        bike.transform.rotation = Quaternion.Euler(currentAngles);
    }

    public void RepresentBikeTurnLeft() {
		Quaternion currentRotation = bike.transform.rotation;
		Vector3 currentAngles = currentRotation.eulerAngles;
		currentAngles.y -= SteeringRotationAmount;
		steering.transform.localRotation = Quaternion.Euler (currentAngles);
		playerBikeMovement.TurnLeft ();
	}

	public void RepresentBikeTurnReset() {

	}

	public void RepresentBikeTurnRight() {
		Quaternion currentRotation = bike.transform.rotation;
		Vector3 currentAngles = currentRotation.eulerAngles;
		currentAngles.y += SteeringRotationAmount;
		steering.transform.rotation = Quaternion.Euler (currentAngles);
		playerBikeMovement.TurnRight ();
	}
}

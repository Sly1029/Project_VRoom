using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorDrivenController : MonoBehaviour {

    public BikeMovement playerBikeMovement;
    public Transform steamVR;
    public GameObject sensor;
    public GameObject steering;
    public float leanIncrement;
    public float curLeanAngle;
    public float maxLeanAngle;
    public float SteeringRotationAmount;
    public float acceleration = 1f;

    private GameObject bike;
    private Vector3 leftPos, centerPos, rightPos;
    private Quaternion leftRot, centerRot, rightRot;
    private GameObject left, center, right;

    // Use this for initialization
    void Start()
    {
        bike = playerBikeMovement.bike;
        curLeanAngle = 0;

        leftPos = ConfigurationManager.GetLeftPosition();
        centerPos = ConfigurationManager.GetCenterPosition();
        rightPos = ConfigurationManager.GetRightPosition();

        leftRot = ConfigurationManager.GetLeftRotation();
        centerRot = ConfigurationManager.GetCenterRotation();
        rightRot = ConfigurationManager.GetRightRotation();

        left = new GameObject("LeftMarker");
        left.transform.parent = steamVR;
        left.transform.position = leftPos;
        left.transform.rotation = leftRot;

        center = new GameObject("CenterMarker");
        center.transform.parent = steamVR;
        center.transform.position = centerPos;
        center.transform.rotation = centerRot;

        right = new GameObject("RightMarker");
        right.transform.parent = steamVR;
        right.transform.position = rightPos;
        right.transform.rotation = rightRot;
    }

    // Update is called once per frame
    void Update()
    {

        playerBikeMovement.Turn(GetSensorDisplacement());
        playerBikeMovement.Accelerate(acceleration);
        Lean(GetSensorRotation());

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

    private float GetSensorDisplacement()
    {
        Vector3 toLeft = left.transform.position - center.transform.position;
        Vector3 toRight = right.transform.position - center.transform.position;
        Vector3 toCur = sensor.transform.position - center.transform.position;
        if (Vector3.Angle(toCur, toLeft) < Vector3.Angle(toCur,toRight))
        {
            return toCur.magnitude / toLeft.magnitude;
        } else
        {
            return toCur.magnitude / toLeft.magnitude;
        }
    }

    private float GetSensorRotation()
    {
        float maxLeft = left.transform.rotation.z;
        float maxRight = right.transform.rotation.z;
        Vector3 toLeft = left.transform.position - center.transform.position;
        Vector3 toRight = right.transform.position - center.transform.position;
        Vector3 toCur = sensor.transform.position - center.transform.position;
        if (Vector3.Angle(toCur, toLeft) < Vector3.Angle(toCur, toRight))
        {
            return Vector3.Lerp(center.transform.rotation.eulerAngles,left.transform.rotation.eulerAngles,(toCur.magnitude)/(toLeft.magnitude)).z;
        }
        else
        {
            return Vector3.Lerp(center.transform.rotation.eulerAngles, right.transform.rotation.eulerAngles, (toCur.magnitude) / (toRight.magnitude)).z;
        }
    }

    public void RepresentBikeTiltLeft()
    {
        if (curLeanAngle + leanIncrement <= maxLeanAngle)
        {
            Quaternion currentRotation = bike.transform.rotation;
            Vector3 currentAngles = currentRotation.eulerAngles;
            currentAngles.z += leanIncrement;
            curLeanAngle += leanIncrement;
            bike.transform.rotation = Quaternion.Euler(currentAngles);
        }
    }

    public void RepresentBikeTiltRight()
    {
        if (curLeanAngle - leanIncrement >= -maxLeanAngle)
        {
            Quaternion currentRotation = bike.transform.rotation;
            Vector3 currentAngles = currentRotation.eulerAngles;
            currentAngles.z -= leanIncrement;
            curLeanAngle -= leanIncrement;
            bike.transform.localRotation = Quaternion.Euler(currentAngles);
        }
    }

    public void Lean(float percent)
    {
        Quaternion currentRotation = bike.transform.rotation;
        Vector3 currentAngles = currentRotation.eulerAngles;
        currentAngles.z = -percent * maxLeanAngle;
        curLeanAngle = -percent * maxLeanAngle;
        bike.transform.rotation = Quaternion.Euler(currentAngles);
    }

    public void RepresentBikeTurnLeft()
    {
        Quaternion currentRotation = bike.transform.rotation;
        Vector3 currentAngles = currentRotation.eulerAngles;
        currentAngles.y -= SteeringRotationAmount;
        steering.transform.localRotation = Quaternion.Euler(currentAngles);
        playerBikeMovement.TurnLeft();
    }

    public void RepresentBikeTurnReset()
    {

    }

    public void RepresentBikeTurnRight()
    {
        Quaternion currentRotation = bike.transform.rotation;
        Vector3 currentAngles = currentRotation.eulerAngles;
        currentAngles.y += SteeringRotationAmount;
        steering.transform.rotation = Quaternion.Euler(currentAngles);
        playerBikeMovement.TurnRight();
    }
}

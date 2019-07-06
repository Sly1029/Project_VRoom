using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Original Author: Greg Kilmer
 * V2 Author: Matthew Napolillo
 * Function: Contols the movement of a bike object.
 * Last Updated: 4/25/2019
 */

public class BikeMovement : MonoBehaviour {
	
	public GameObject bike;
	public float accelerationRate;
	public float decelerationRate;
	public float topSpeed;
    public LayerMask boostLayers;

	public float leanRate;

	public float turnRate;

	public Vector3 curVelocity;

	private Rigidbody bikeRigidBody;

	public LeanAxisControl leanAxisController;

    public float horizInput, vertInput, handbrake;

	[Header("InEditorTesting")]
	public bool keyboardControl = false;
	public GameObject[] disableForTesting;
	public GameObject[] disableForVR;

	private void Awake() 
	{
		if(keyboardControl)
		{
			foreach(GameObject gObj in disableForTesting)
			{
				gObj.SetActive(false);
			}
		}
		else
		{
			foreach(GameObject gObj in disableForVR)
			{
				gObj.SetActive(false);
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		bikeRigidBody = GetComponent<Rigidbody> ();
        bikeRigidBody.velocity = curVelocity;
	}
	
	// Update is called once per frame
	void Update () 
	{
        // Get throttle input
		curVelocity = bikeRigidBody.velocity;
		
		//CubikeRigidBody Velocity to topSpeed
		if (bikeRigidBody.velocity.magnitude >= topSpeed) 
		{
			bikeRigidBody.velocity = bikeRigidBody.velocity.normalized * topSpeed;
		}

		Debug.DrawRay(bike.transform.position, bike.transform.right*10,Color.red);

        // pass the input to the car!
		if(keyboardControl)
		{
			horizInput = Input.GetAxis("Horizontal");
		}
		else
		{
        	horizInput = leanAxisController.combinedAxis;
		}
        vertInput = Input.GetAxis("Vertical");

        // handbrake = Input.GetAxis("Jump");
        Turn(horizInput);
        // Accelerate(vertInput);
        // Decelerate(handbrake);

    }

	//Increase speed in forward direction
	public void Accelerate() 
	{
        if (Physics.Raycast(bike.transform.position, transform.up * -1, 10, boostLayers.value))
        {
            bikeRigidBody.AddForce(bike.transform.forward * accelerationRate * Time.deltaTime);
        } 
		else
        {
           bikeRigidBody.AddForce(bike.transform.forward * accelerationRate * Time.deltaTime);
        }
	}

	//Decrease speed in the forward direction
	public void Decelerate(float input) 
	{
		if(bikeRigidBody.velocity.sqrMagnitude > 1)
		{
			bikeRigidBody.AddForce(bikeRigidBody.velocity.normalized * input * -decelerationRate * Time.deltaTime);
		}
		if(input == 1 && bikeRigidBody.velocity.sqrMagnitude < 1)
		{
			bikeRigidBody.velocity = Vector3.zero;
		}
	}

    public void Accelerate(float percent)
    {
        if (Physics.Raycast(bike.transform.position, bike.transform.up * -1, 10, boostLayers.value))
        {
            bikeRigidBody.AddForce(bike.transform.forward * percent * accelerationRate * Time.deltaTime);
        }
        else
        {
            bikeRigidBody.AddForce(bike.transform.forward * percent * accelerationRate * Time.deltaTime );
        }
    }

    public void Turn(float percent)
    {
        float turnAmt = percent * turnRate;
        transform.RotateAround(bike.transform.position, bike.transform.up, turnAmt);
        bikeRigidBody.velocity = Quaternion.AngleAxis(turnAmt, bike.transform.up) * bikeRigidBody.velocity;
    }

	//Turn left using handle bars
	public void TurnLeft() {
		float turnAmt = -turnRate * bikeRigidBody.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		bikeRigidBody.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * bikeRigidBody.velocity;
	}

	//Turn right using handle bars
	public void TurnRight() {
		float turnAmt = turnRate * bikeRigidBody.velocity.magnitude;
		transform.RotateAround (transform.position, transform.up, turnAmt);
		bikeRigidBody.velocity = Quaternion.AngleAxis (turnAmt, transform.up) * bikeRigidBody.velocity;
	}

	//Slides bike left by leaning left
	public void LeanLeft(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		bikeRigidBody.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * -leanRate * percentageOfLeanRate);
	}

	//Slides bike right by learning right
	public void LeanRight(float percentageOfLeanRate) {
		//TODO revamp to use velocity scaling only in the forward direction
		bikeRigidBody.AddForce ((transform.right + transform.up * Mathf.Tan (transform.rotation.eulerAngles.z)) * leanRate * percentageOfLeanRate);
	}
}

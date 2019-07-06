using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanAxisControl : MonoBehaviour {

    [Header("Control Transforms")]
    public Transform left;
    public Transform right;
    public Transform center;
    public Transform root;
    public Transform handleBarTracker;

    [Header("Active Values")]
    public float leftAxisAmount = 0f;
    public float rightAxisAmount = 0f;
    public float combinedAxis = 0f;

    private float distanceCenterToRight;
    private float distanceCenterToLeft;

    private Calibrate calibrationComponent;

    void Start() 
    {
        calibrationComponent = GetComponent<Calibrate>();
    }

	// Update is called once per frame
	void Update () 
    {
        if (calibrationComponent.calibrateCompleted)
        {
            distanceCenterToLeft = Vector3.Distance(center.position, left.position);
            distanceCenterToRight = Vector3.Distance(center.position, right.position);
            if (distanceCenterToLeft > 0.001f && distanceCenterToRight > 0.001f)
            {
                leftAxisAmount = 1f - Mathf.Clamp(1f, 0f, 
                    (Vector3.Distance(handleBarTracker.position, left.position) / distanceCenterToLeft));

                rightAxisAmount = 1f - Mathf.Clamp(1f, 0f, 
                    (Vector3.Distance(handleBarTracker.position, right.position) / distanceCenterToRight));

                combinedAxis = -1f * leftAxisAmount + rightAxisAmount;
            }
        }
    }
}

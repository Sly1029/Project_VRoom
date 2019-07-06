using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibrate : MonoBehaviour {

    [Header("Important Transforms")]
    public Transform frontTracker;
    public Transform leftMarker;
    public Transform centerMarker;
    public Transform rightMarker;
    public Transform rootMarker;

    [Header("Calibration Checks")]
    public GameObject calibrationCanvas;
    public bool calibrateStarted = false;
    public bool calibrateCompleted = false;
    public bool calibrateOnStart = true;
    public BikeMovement bikeMovementComponent;
    public CalculateBikeModelPositionAndForward bikeAlignment;
    public Timer durationTimer;
    public float centerTrackingCompletion, rightTrackingCompletion, leftTrackingCompletion;
    public string GuideText = "Begin \n Calibration";

    [Header("Config")]
    public float neglibleDelta = 0.01f;
    public float continuousHeldTime = 0f;
    public float calibrationStartedThreshold = 0.05f;
    public float handlebarTrackerHeight = 0.5f;
    
    private Vector3 frontPoint;

    void Start() 
    {
        if (!calibrateStarted && calibrateOnStart)
        {
            StartCoroutine(BeginCalibration());
        }
    }

    void Update() 
    {
        if (!calibrateStarted && calibrateOnStart) 
        {
            StartCoroutine(BeginCalibration());
        }
    }

    public void BeginCalibrating() 
    {
        StartCoroutine(BeginCalibration());
    }


    private IEnumerator BeginCalibration()
    {
        calibrateStarted = true;
        frontPoint = frontTracker.position;
        Vector3 previousFrontPoint = frontPoint;

        Debug.Log("Dont move keep the front at the center");
        GuideText = "Keep the bike still in the center";
        continuousHeldTime = 0f;
        centerTrackingCompletion = 0f;

        while (continuousHeldTime < 1f) 
        {
            if (Vector3.Distance(previousFrontPoint, frontTracker.position) < neglibleDelta)
            {
                continuousHeldTime += Time.deltaTime / 5f;
            } 
            else 
            {
                continuousHeldTime = 0f;
            }

            centerTrackingCompletion = Mathf.Clamp01(continuousHeldTime);
            previousFrontPoint = frontTracker.position;
            yield return new WaitForEndOfFrame();
        }

        centerMarker.position = previousFrontPoint;
        GuideText = "Lean all the way to the right";
        
        Debug.Log("Lean all the way to the right");
        while (Vector3.Distance(frontTracker.position, previousFrontPoint) < calibrationStartedThreshold)
        {
            yield return new WaitForEndOfFrame();
        }
        continuousHeldTime = 0f;
        rightTrackingCompletion = 0f;
        while (continuousHeldTime < 1f) {
            if (Vector3.Distance(previousFrontPoint, frontTracker.position) < neglibleDelta) {
                    continuousHeldTime += Time.deltaTime / 5f;
            } else {
                    continuousHeldTime = 0f;
            }
            rightTrackingCompletion = Mathf.Clamp01(continuousHeldTime);
            previousFrontPoint = frontTracker.position;
            yield return new WaitForEndOfFrame();
        }
        rightMarker.position = previousFrontPoint;
        GuideText = "Now lean all the way to the left";
        Debug.Log("Now lean all the way to the left");
        while (Vector3.Distance(frontTracker.position, previousFrontPoint) < calibrationStartedThreshold) {
            yield return new WaitForEndOfFrame();
        }
        continuousHeldTime = 0f;
        leftTrackingCompletion = 0f;
        while (continuousHeldTime < 1f) {
            if (Vector2.Distance(previousFrontPoint, frontTracker.position) < neglibleDelta) {
                continuousHeldTime += Time.deltaTime / 5f;
            } else {
                continuousHeldTime = 0f;
            }
            leftTrackingCompletion = Mathf.Clamp01(continuousHeldTime);
            previousFrontPoint = frontTracker.position;
            yield return new WaitForEndOfFrame();
        }
        leftMarker.position = previousFrontPoint;
        Debug.Log("Okay chill");

        Vector3 rootPosition = centerMarker.position;
        rootPosition.y = rootPosition.y - handlebarTrackerHeight;
        rootMarker.position = rootPosition;
        calibrateCompleted = true;
        calibrationCanvas.SetActive(false);
        bikeMovementComponent.enabled = true;
        durationTimer.bikeCalibrated = true;
       // bikeMovementComponent.gameObject.GetComponent<Rigidbody>().velocity = bikeMovementComponent.transform.forward * 20f;
        bikeAlignment.Align();
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(frontTracker.position, frontTracker.position + calibrationStartedThreshold * transform.right);

        Gizmos.DrawLine(frontTracker.position, frontTracker.position - handlebarTrackerHeight * Vector3.up);
    }
}

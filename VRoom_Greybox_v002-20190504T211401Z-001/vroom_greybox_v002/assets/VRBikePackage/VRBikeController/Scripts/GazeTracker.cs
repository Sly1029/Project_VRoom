using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GazeTracker : MonoBehaviour {
    public LayerMask gazableLayers;
    public float durationUntilTriggered;
    public UnityEvent onGaze;
    public GameObject lastGazed;


    public float currentDurationGazed = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // shoot the raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, gazableLayers)) {
            if (lastGazed != null && lastGazed != hit.collider.gameObject) {
                currentDurationGazed = 0f;
            }
            lastGazed = hit.collider.gameObject;
            currentDurationGazed += Time.deltaTime;
        } else {
            lastGazed = null;
            currentDurationGazed = 0f;
        }

        if (currentDurationGazed >= durationUntilTriggered) {
            onGaze.Invoke();
            currentDurationGazed = 0f;
        }
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * 50f);
    }
}

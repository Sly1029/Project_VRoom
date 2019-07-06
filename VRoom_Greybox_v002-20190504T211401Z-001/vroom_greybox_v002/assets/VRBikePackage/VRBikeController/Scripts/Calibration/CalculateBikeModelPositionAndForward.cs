using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateBikeModelPositionAndForward : MonoBehaviour {
    public Transform left, right, center, root, bike;
    public Vector3 bikeToCenterOffset;
    public Vector3 forward;

	public void Align() {
        bike.position = center.position + bikeToCenterOffset;
        Vector3 leftToRight = right.position - left.position;
        forward = Quaternion.AngleAxis(-90f, Vector3.up) * leftToRight;
        bike.LookAt(bike.position + forward);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(center.position, center.position + forward);
    }
}

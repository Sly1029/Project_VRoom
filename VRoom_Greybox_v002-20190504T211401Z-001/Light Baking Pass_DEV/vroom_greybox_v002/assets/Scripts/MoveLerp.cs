using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLerp : MonoBehaviour
{

    public float bounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 localPosOffset = new Vector3(transform.position.x - bounds, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, localPosOffset, 0.05f);
        }


    }
}

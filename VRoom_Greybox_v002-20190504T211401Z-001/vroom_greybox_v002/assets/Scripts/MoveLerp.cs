using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLerp : MonoBehaviour
{
    GameObject right,left;
    public float bounds;
    // Start is called before the first frame update
    void Start()
    {
        right = GameObject.FindGameObjectsWithTag("bound")[1];
        left = GameObject.FindGameObjectsWithTag("bound")[0];
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = transform.parent.rotation;
        if (Input.GetKey(KeyCode.A))
        {
            //Vector3 localPosOffset = new Vector3(transform.parent.position.x , transform.parent.position.y+3f, transform.parent.position.z - bounds);
           
            // GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // g.transform.position = left.transform.position;
            transform.position = Vector3.Lerp(transform.position, left.transform.position, 0.05f);
        }
        else if (Input.GetKey(KeyCode.D)){
             //Vector3 localPosOffset = new Vector3(transform.parent.position.x , transform.parent.position.y+3f, transform.parent.position.z + bounds);
           
            //   GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //    g.transform.position = right.transform.position;
             transform.position = Vector3.Lerp(transform.position, right.transform.position, 0.05f);
        }


    }
}

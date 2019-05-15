using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSide : MonoBehaviour
{
    GameObject[] bounds;
    public float coef_decrease;
    float interpolateRight, interpolateLeft;
    // Start is called before the first frame update
    void Start()
    {
        interpolateLeft = 0f;
        interpolateRight = 0f;
        bounds = GameObject.FindGameObjectsWithTag("Bounds");
        Debug.Log(bounds[0]);
        Debug.Log(bounds[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("Called");
            transform.position = Vector3.Lerp(transform.position, bounds[0].transform.position, coef_decrease);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            transform.position = Vector3.Lerp(transform.position, bounds[1].transform.position, coef_decrease);
        }
    }



    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        if (interpolateLeft < 1)
    //        {
    //            transform.position = Vector3.Lerp(transform.position, bounds[0].transform.position, interpolateLeft);
    //            interpolateRight = (interpolateRight < 0) ? 0 : interpolateRight + coef_decrease;
    //            interpolateLeft += coef_decrease;
    //        }
    //        else
    //        {
    //            interpolateLeft -= coef_decrease;
    //        }
    //    }
    //    else if (Input.GetKey(KeyCode.D))
    //    {
    //        if (interpolateRight < 1)
    //        {
    //            //Take each object and lerp from current position to the end boundary Gameobject position

            //            transform.position = Vector3.Lerp(transform.position, bounds[1].transform.position, interpolateRight);
            //            interpolateLeft = (interpolateLeft <= 0) ? 0 : interpolateLeft + coef_decrease;
            //            interpolateRight += coef_decrease;
            //        }
            //        else
            //        {
            //            interpolateRight -= coef_decrease;
            //        }
            //    }


            //}
    }

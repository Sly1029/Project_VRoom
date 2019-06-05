using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    List<GameObject> taggedObjsInScene;
    public Transform viewer;
    public int viewDist;
    Vector3 viewerPosition;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("chunkable");
        taggedObjsInScene = new List<GameObject>(objs);
        // foreach (GameObject x in taggedObjsInScene){
        //      Debug.Log(x);
        // }
       
    }

    // Update is called once per frame
    void Update()
    {
        viewerPosition = new Vector3(viewer.position.x, viewer.position.y, viewer.position.z);
        CheckEnabledDisabled();



    }
    void CheckEnabledDisabled()
    {
        for (int i = 0; i < taggedObjsInScene.Count; i++)
        {
            if (Vector3.Distance(viewerPosition, taggedObjsInScene[i].transform.position) <= viewDist)
            {
                taggedObjsInScene[i].SetActive(true);

            }
            else
            {
                taggedObjsInScene[i].SetActive(false);
            }
        }




    }
}

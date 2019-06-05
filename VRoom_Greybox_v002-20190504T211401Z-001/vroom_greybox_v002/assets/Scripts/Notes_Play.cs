using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Notes_Play : MonoBehaviour
{
    GameObject[] bike;
    // Start is called before the first frame update
    void Start()
    {
        bike = GameObject.FindGameObjectsWithTag("notes");
        InvokeRepeating("UpdateRotation", 2.0f, 0.25f);
    }

    // Update is called once per frame
    void UpdateRotation()
    {
        Parallel.ForEach(bike, note =>
        {
            note.transform.LookAt(transform);
            //note.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(note.transform.position, transform.position, 10f, 0.0f));
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");   

       other.GetComponentInParent<AudioSource>().Play();
    }
}

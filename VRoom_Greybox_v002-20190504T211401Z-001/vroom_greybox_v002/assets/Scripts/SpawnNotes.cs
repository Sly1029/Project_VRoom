using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class Vector3Data{

public List<string>data = new List<string>();


}



public class SpawnNotes : MonoBehaviour
{
    public GameObject noteGeneric;
    float startTime;

    List<Vector3> noteLocations;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        InvokeRepeating("noteSpawn", 0f, 2.0f);
        noteLocations = new List<Vector3>();

        Debug.Log(Directory.GetCurrentDirectory());
    }
    bool isDone = false;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > 120f && !isDone){
            SaveVectors();
            isDone = true;
        }
        
    }

    void SaveVectors()
    {
     BinaryFormatter bf = new BinaryFormatter();
     FileStream file = File.Create(Directory.GetCurrentDirectory() + @"\assets\resources\notepossiblelocations_hilltrack.save");
     Vector3Data vectordata = new Vector3Data();
     foreach(Vector3 x in noteLocations){
        vectordata.data.Add(x.x+","+x.y+","+x.z);
     }



    bf.Serialize(file, vectordata);
    file.Close();    
    ReadVectors();
    }
    
    public static  Vector3Data ReadVectors(){
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Directory.GetCurrentDirectory()+ @"\assets\resources\notepossiblelocations_hilltrack.save", FileMode.Open);
        Vector3Data vec = (Vector3Data)bf.Deserialize(file);
        Debug.Log(vec.data[0]);
        file.Close();

        return vec;
    }


    IEnumerator noteSpawn()
    {

        //if ((Time.time - startTime) % 2.0f == 0f)
            
            //Instantiate(noteGeneric, transform.position, transform.rotation);
            noteLocations.Add(transform.position);  
        return null;
    }
}

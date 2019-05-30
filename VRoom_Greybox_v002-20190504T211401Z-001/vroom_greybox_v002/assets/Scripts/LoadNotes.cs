using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadNotes : MonoBehaviour
{
    public GameObject prefabSpawn;

    // Start is called before the first frame update

    List<Vector3> locations;
    List<Note> notes;
    void Start()
    {
        notes = new List<Note>();
        locations = new List<Vector3>();
        List<string> locations_strings= SpawnNotes.ReadVectors().data;
        convert(locations_strings);
        LoadLocations();
    }

//Converts string vectors from file into usable vectors with abstractions
    void convert(List<string> locations_strings){
        foreach(string x in locations_strings){
            string[] loc = x.Split(',');
            locations.Add(new Vector3(float.Parse(loc[0]), float.Parse(loc[1]),float.Parse(loc[2])));
        }


    }

//Instantiates Objects at given locations with files @TODO


    void spawnNotes(){

        foreach (Note n in notes){

        }



    }




    void LoadLocations(){
        using(var reader = new StreamReader(Directory.GetCurrentDirectory() + @"\assets\resources\Note_Spawn.csv"))
        {
            for ( int x = 0; x<2; x++){
              var line = reader.ReadLine();
              string [] values = line.Split(',');
              
            //Time parsing should not be needed later @TODO
              int time_value = int.Parse(values[0].Substring(2));
            if (!values[0].StartsWith("0")){
              time_value+=60*int.Parse(values[0][0]+"");
            }

            notes.Add(new Note(time_value, values[1], values[2][0]));
            }

        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

struct Note{

    int time;
    string file;
    char path;

    public Note(int t, string s, char p){
        time = t;
        file = s;
        path = p;
    }
}




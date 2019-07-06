using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfigurationManager : MonoBehaviour {

    [System.Serializable]
    public class ConfigData
    {
        public ConfigData(Vector3 p, Quaternion r)
        {
            pos = p;
            rot = r;
        }

        public Vector3 pos;
        public Quaternion rot;
    }

    public Material success,start;
    public Image centerBar,leftBar,rightBar;
    public GameObject loadMessage;
    public float loadTime;
    public float tolerance;
    public bool configuringCenter,configuringLeft,configuringRight;
    public float configPercent;
    public GameObject sensor;

    private List<ConfigData> tmp_data;
    private float loadComplete;

	// Use this for initialization
	void Start () {
        tmp_data = new List<ConfigData>();
        centerBar.fillAmount = 0;
        leftBar.fillAmount = 0;
        rightBar.fillAmount = 0;
    }
	
	// Update is called once per frame
	void Update () {
		if (configuringCenter)
        {
            if (Time.time < loadComplete)
            {
                ConfigData cd = new ConfigData(sensor.transform.position,sensor.transform.rotation);
                tmp_data.Add(cd);
                configPercent = 1f-(loadComplete-Time.time)/loadTime;
                centerBar.fillAmount = configPercent;
            } else
            {
                EndConfigCenter();
            }
        }
        if (configuringLeft)
        {
            if (Time.time < loadComplete)
            {
                ConfigData cd = new ConfigData(sensor.transform.position, sensor.transform.rotation);
                tmp_data.Add(cd);
                configPercent = 1f - (loadComplete - Time.time) / loadTime;
                leftBar.fillAmount = configPercent;
            }
            else
            {
                EndConfigLeft();
            }
        }
        if (configuringRight)
        {
            if (Time.time < loadComplete)
            {
                ConfigData cd = new ConfigData(sensor.transform.position, sensor.transform.rotation);
                tmp_data.Add(cd);
                configPercent = 1f - (loadComplete - Time.time) / loadTime;
                rightBar.fillAmount = configPercent;
            }
            else
            {
                EndConfigRight();
            }
        }
    }

    //CENTER
    public void ConfigCenter()
    {
        centerBar.material = start;
        loadComplete = Time.time + loadTime;
        tmp_data = new List<ConfigData>();
        configuringCenter = true;
        loadMessage.SetActive(configuringCenter);
    }

    private void EndConfigCenter()
    {
        int dataSize = tmp_data.Count;
        Debug.Log(dataSize);
        Vector3 avgPos = new Vector3();
        Vector3 avgAngles = new Vector3();
        foreach(ConfigData data in tmp_data)
        {
            avgPos += data.pos;
            avgAngles += data.rot.eulerAngles;
        }
        avgPos /= (float)dataSize;
        avgAngles /= (float)dataSize;

        PlayerPrefs.SetFloat("cx", avgPos.x);
        PlayerPrefs.SetFloat("cy", avgPos.y);
        PlayerPrefs.SetFloat("cz", avgPos.z);
        PlayerPrefs.SetFloat("cax", avgAngles.x);
        PlayerPrefs.SetFloat("cay", avgAngles.y);
        PlayerPrefs.SetFloat("caz", avgAngles.z);

        configuringCenter = false;
        centerBar.material = success;
        loadMessage.SetActive(configuringCenter);
    }

    public static Vector3 GetCenterPosition()
    {
        return new Vector3(PlayerPrefs.GetFloat("cx"), PlayerPrefs.GetFloat("cy"), PlayerPrefs.GetFloat("cz"));
    }

    public static Quaternion GetCenterRotation()
    {
        return Quaternion.Euler(new Vector3(PlayerPrefs.GetFloat("cax"), PlayerPrefs.GetFloat("cay"), PlayerPrefs.GetFloat("caz")));
    }

    // LEFT
    public void ConfigLeft()
    {
        leftBar.material = start;
        loadComplete = Time.time + loadTime;
        tmp_data = new List<ConfigData>();
        configuringLeft = true;
        loadMessage.SetActive(configuringLeft);
    }

    private void EndConfigLeft()
    {
        int dataSize = tmp_data.Count;
        Debug.Log(dataSize);
        Vector3 avgPos = new Vector3();
        Vector3 avgAngles = new Vector3();
        foreach (ConfigData data in tmp_data)
        {
            avgPos += data.pos;
            avgAngles += data.rot.eulerAngles;
        }
        avgPos /= (float)dataSize;
        avgAngles /= (float)dataSize;

        PlayerPrefs.SetFloat("lx", avgPos.x);
        PlayerPrefs.SetFloat("ly", avgPos.y);
        PlayerPrefs.SetFloat("lz", avgPos.z);
        PlayerPrefs.SetFloat("lax", avgAngles.x);
        PlayerPrefs.SetFloat("lay", avgAngles.y);
        PlayerPrefs.SetFloat("laz", avgAngles.z);

        configuringLeft = false;
        leftBar.material = success;
        loadMessage.SetActive(configuringLeft);
    }

    public static Vector3 GetLeftPosition()
    {
        return new Vector3(PlayerPrefs.GetFloat("lx"), PlayerPrefs.GetFloat("ly"), PlayerPrefs.GetFloat("lz"));
    }

    public static Quaternion GetLeftRotation()
    {
        return Quaternion.Euler(new Vector3(PlayerPrefs.GetFloat("lax"), PlayerPrefs.GetFloat("lay"), PlayerPrefs.GetFloat("laz")));
    }

    //RIGHT
    public void ConfigRight()
    {
        rightBar.material = start;
        loadComplete = Time.time + loadTime;
        tmp_data = new List<ConfigData>();
        configuringRight = true;
        loadMessage.SetActive(configuringRight);
    }

    private void EndConfigRight()
    {
        int dataSize = tmp_data.Count;
        Debug.Log(dataSize);
        Vector3 avgPos = new Vector3();
        Vector3 avgAngles = new Vector3();
        foreach (ConfigData data in tmp_data)
        {
            avgPos += data.pos;
            avgAngles += data.rot.eulerAngles;
        }
        avgPos /= (float)dataSize;
        avgAngles /= (float)dataSize;

        PlayerPrefs.SetFloat("rx", avgPos.x);
        PlayerPrefs.SetFloat("ry", avgPos.y);
        PlayerPrefs.SetFloat("rz", avgPos.z);
        PlayerPrefs.SetFloat("rax", avgAngles.x);
        PlayerPrefs.SetFloat("ray", avgAngles.y);
        PlayerPrefs.SetFloat("raz", avgAngles.z);

        configuringRight = false;
        rightBar.material = success;
        loadMessage.SetActive(configuringRight);
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(1);
    }

    public static Vector3 GetRightPosition()
    {
        return new Vector3(PlayerPrefs.GetFloat("rx"), PlayerPrefs.GetFloat("ry"), PlayerPrefs.GetFloat("rz"));
    }

    public static Quaternion GetRightRotation()
    {
        return Quaternion.Euler(new Vector3(PlayerPrefs.GetFloat("rax"), PlayerPrefs.GetFloat("ray"), PlayerPrefs.GetFloat("raz")));
    }
}

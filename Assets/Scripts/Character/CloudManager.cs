using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{

    public ParticleSystem cloud;

    public int currentWeather;
    public int nextWeather;
    //public int currentSpeed;


    // Start is called before the first frame update
    void Start()
    {
        currentWeather = GameController.Instance.weather;
        ChangeCloud(currentWeather);
        //currentSpeed = GameController.Instance.speed;
    }

    // Update is called once per frame
    void Update()
    {
        nextWeather = GameController.Instance.weather;
        if (currentWeather != nextWeather)
        {
            ChangeCloud(nextWeather);
            currentWeather = nextWeather;
        }

    }

    void ChangeCloud(int now)
    {
        ParticleSystem.MainModule main = cloud.main;

        if (now == 1)
        {
            //StartColor = new Color(100,100,100, 255);
            StartColor = new Color(0,0,0,255);
        }
        else
        {
            StartColor = new Color(255, 255, 255, 255);
        }
    }

    public Color StartColor
    {
        set
        {
            var main = cloud.main;
            main.startColor = value;
        }
    }
}

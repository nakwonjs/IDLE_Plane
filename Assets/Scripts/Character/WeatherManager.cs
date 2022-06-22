using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public ParticleSystem[] weatherSet;
    public ParticleSystem CWP;
    public AudioClip rainSoundEffect;

    public int currentWeather;
    public int nextWeather;
    public int currentSpeed;

    

    // Start is called before the first frame update
    void Start()
    {
        currentWeather = GameController.Instance.weather;
        ChangeWeather(currentWeather, currentWeather);
        currentSpeed = GameController.Instance.speed;

    }

    

    // Update is called once per frame
    void Update()
    {
        //if(currentSpeed != GameController.Instance.speed)
        //{
        //    currentSpeed = GameController.Instance.speed;
        //}



        nextWeather = GameController.Instance.weather;
        if(currentWeather != nextWeather)
        {
            ChangeWeather(currentWeather,nextWeather);
            currentWeather = nextWeather;
            
        }
    }

    void ChangeWeather(int now, int next)
    {
        if(now != 0)
        {
            //weatherSet[now].Stop();
            CWP.Stop();
        }
        if (next != 0)
        {
            //weatherSet[next].Play();
            CWP = weatherSet[next];
            CWP.Play();

        }


    }


}

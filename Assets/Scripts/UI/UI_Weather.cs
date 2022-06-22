using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Weather : MonoBehaviour
{
    [SerializeField]
    private GameObject[] weatherArray = new GameObject[4];


    [SerializeField]
    private Text myWeather;
    [SerializeField]
    private Text myWeatherGas;
    [SerializeField]
    private Text myWeatherGold;
    [SerializeField]
    private Text myWeatherSpeed;


    public int currentWeather;
    //public int preWeather = 0;

    public double changeWeather;

    // Start is called before the first frame update
    void Start()
    {
        changeWeather = 0;

        SetWeather();

        SetWeatherGasText();
        SetWeatherGoldText();
        SetWeatherSpeedText();
    }

    // Update is called once per frame
    void Update()
    {
        changeWeather += Time.deltaTime;

        if (changeWeather > 100)
        {
            weatherArray[currentWeather].SetActive(false);

            //GameController.Instance.SetStatus();

            SetWeather();

            SetWeatherGasText();
            SetWeatherGoldText();
            SetWeatherSpeedText();
            changeWeather = 0;
        }

        else if(currentWeather != GameController.Instance.weather)
        {
            weatherArray[currentWeather].SetActive(false);
            currentWeather = GameController.Instance.weather;
            weatherArray[currentWeather].SetActive(true);


            SetWeatherGasText();
            SetWeatherGoldText();
            SetWeatherSpeedText();
            changeWeather = 0;
        }
    }

    void SetWeather()
    {
        currentWeather = Random.Range(0, 3);
        weatherArray[currentWeather].SetActive(true);

        GameController.Instance.weather = currentWeather;
        GameController.Instance.SetStatus();
    }


    void SetWeatherGasText()
    {
        if (GameController.Instance.weatherValue[currentWeather, 0] > 0)
            myWeatherGas.text = "+";
        else
            myWeatherGas.text = "-";
    }
    void SetWeatherGoldText()
    {
        if (GameController.Instance.weatherValue[currentWeather, 1] > 0)
            myWeatherGold.text = "+";
        else
            myWeatherGold.text = "-";
        //myWeatherGold.text = "+ X " + weatherValue[currentWeather, 1].ToString();
    }
    void SetWeatherSpeedText()
    {
        if (GameController.Instance.weatherValue[currentWeather, 2] > 0)
            myWeatherSpeed.text = "+";
        else
            myWeatherSpeed.text = "-";
        //myWeatherSpeed.text = "+ X " + weatherValue[currentWeather, 2].ToString();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int geograpyIndex = 0;
    public int MAX_gas = 100;
    public int gas;
    
    public int gold;

    public int BASE_speed = 10;
    public int speed;

    public double currentSpeed;
    public double currentGold;
    public double currentGas;

    public double speedLevel = 1.0;
    public double goldLevel = 1.0;
    public double gasLevel = 0;

    public double speedWeather = 0;
    public double goldWeather = 0;
    public double gasWeather = 0;

    public double speedItem = 0;
    public double goldItem = 0;
    public double gasItem = 0;

    public double[,] weatherValue = new double[4, 3] { { 0.2, 0.4, 0 }, { -0.2, 0.2, 0.6 }, { -0.2, 0.8, -0.2 }, { 0.2, 0.2, 0.2 } };
    public int weather = 0;

    public float dtime = 0f;

    public int maxIdx = 0;


    void Start()
    {
        gas = MAX_gas;
        speed = BASE_speed;
        gasLevel = 0;
    }

    public void SetStatus()
    {
        gasWeather = weatherValue[weather, 0];
        goldWeather = weatherValue[weather, 1];
        speedWeather = weatherValue[weather, 2];

        currentGas = (speedLevel + goldLevel) * 0.5 - gasLevel - gasWeather;
        currentGold = goldLevel + goldWeather;
        currentSpeed = speedLevel + speedWeather;
        speed = (int)(BASE_speed * currentSpeed);
    }




    //싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static GameController _instance;

    //인스턴스에 접근하기 위한 프로퍼티
    public static GameController Instance
    {
        get
        {
            //인스턴스가 없는 경우에 접근하려 하면 인스턴스 할당
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameController)) as GameController;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Speed : MonoBehaviour
{
    //private int BaseSpeed;
    private int speed;
    private double speedLevel;

    [SerializeField]
    private Text mySpeed;
    [SerializeField]
    private Text mySpeedLevel;


    // Start is called before the first frame update
    void Start()
    {
        //BaseSpeed = GameController.Instance.speed;
    }

    // Update is called once per frame
    void Update()
    {       
        speedLevel = GameController.Instance.currentSpeed;
        speed = GameController.Instance.speed;
       
        SetSpeedText();
        SetSpeedLevelText();
    }

    void SetSpeedText()
    {
        mySpeed.text = speed.ToString();
    }
    void SetSpeedLevelText()
    {
        mySpeedLevel.text = "X " + speedLevel.ToString();
    }
}

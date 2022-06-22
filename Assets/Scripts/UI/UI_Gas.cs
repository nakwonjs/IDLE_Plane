using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Gas : MonoBehaviour
{
    public int gas;
    private double gasLevel;

    [SerializeField]
    private Text myGas;
    [SerializeField]
    private Text myGasLevel;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gas = GameController.Instance.gas;
        if (gas > 0)
            SetGasText();
        else
            SetGasRefuling();

        gasLevel = GameController.Instance.currentGas;
        gasLevel = System.Math.Round(gasLevel * 100) / 100;
        SetGasLevelText();
    }

    void SetGasText()
    {
        myGas.text = gas.ToString();
    }
    void SetGasRefuling()
    {
        myGas.text = "Refueling...";
    }
    void SetGasLevelText()
    {
        myGasLevel.text = "X " + gasLevel.ToString();
    }
}

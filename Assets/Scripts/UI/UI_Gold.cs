using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Gold: MonoBehaviour
{
    public int gold;
    private double goldLevel;

    [SerializeField]
    private Text myGold;
    [SerializeField]
    private Text myGoldLevel;



    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    void Update()
    {
        gold = GameController.Instance.gold;
        SetGoldText();
        goldLevel = GameController.Instance.currentGold;
        SetGoldLevelText();
    }

    void SetGoldText()
    {
        myGold.text = gold.ToString();
    }
    void SetGoldLevelText()
    {
        myGoldLevel.text = "X "+goldLevel.ToString();
    }
}

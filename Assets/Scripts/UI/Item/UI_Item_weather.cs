using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item_weather : MonoBehaviour
{

    public int cost = 1000;
    public int weather;

    [SerializeField]
    private Text myCost;


    // Start is called before the first frame update
    void Start()
    {
        myCost.text = cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClicked()
    {
        if (GameController.Instance.gold > cost)
        {
            GameController.Instance.gold -= cost;
            GameController.Instance.weather = weather;
            GameController.Instance.SetStatus();

        }
    }
}

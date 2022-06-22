using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item_speed : MonoBehaviour
{
    public int cost = 1000;
    public int burstSpeed = 50;
    public int burstAmount = 5;
    private bool onBurst = false;

    public float burstTime = 0;
    float currentTime = 0;

    
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
        if(currentTime > burstTime && onBurst)
        {
            GameController.Instance.speed -= burstSpeed;
            currentTime = 0;
            burstTime = 0;
            onBurst = false;
        }
        else
        {
            currentTime += Time.deltaTime;
        }

    }

    public void OnClicked()
    {
        if (GameController.Instance.gold > cost)
        {
            GameController.Instance.gold -= cost;
            burstTime += burstAmount;
            if (!onBurst)
            {
                GameController.Instance.speed += burstSpeed;
                onBurst = true;
            }
        }
    }
}

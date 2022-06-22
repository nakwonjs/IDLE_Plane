using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    Transform transform;

    private int speed;
    private double gold = 0;
    private int MAX_gas;
    private double gas;


    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        speed = GameController.Instance.speed;
        MAX_gas = GameController.Instance.MAX_gas;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.gas > 0)
        {
            speed = GameController.Instance.speed;
            Vector3 move = new Vector3(0, 0, speed * Time.deltaTime);
            transform.Translate(move);

            gold += speed * Time.deltaTime * GameController.Instance.currentGold;
            gas += Time.deltaTime * GameController.Instance.currentGas;

            if (gold > 1)
            {
                GameController.Instance.gold += (int)gold;
                gold = 0;
            }
            if (gas > 1)
            {
                GameController.Instance.gas -= (int)gas;
                gas = 0;
            }
        }
        else
        {
            MAX_gas = GameController.Instance.MAX_gas;

            gas += Time.deltaTime * 5;
            if (gas > MAX_gas)
            {
                GameController.Instance.gas = MAX_gas;
                gas = 0;
            }
        }

    }  
}

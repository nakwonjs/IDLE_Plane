using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneAnimator : MonoBehaviour
{
    Animator animator;
    public int currentWeather;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentWeather = GameController.Instance.weather;
        SetAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWeather != GameController.Instance.weather)
        {
            currentWeather = GameController.Instance.weather;
            SetAnimation();
           
        }      
    }

    void SetAnimation()
    {
        animator.SetInteger("weather", currentWeather);        
    }
}

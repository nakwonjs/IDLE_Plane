using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGeo : MonoBehaviour
{
    public GameObject comming;
    public GameObject disappear;
    public float changetime = 5f;

    private UnityEngine.Video.VideoPlayer commingV;
    private UnityEngine.Video.VideoPlayer disV;
    private MeshRenderer commesh;
    private MeshRenderer dismesh;

    private bool Isplay = false;
    private bool IsplayDis = false;
    private bool SqOn = true;

    private float dtime = 0f;
    // Update is called once per frame
    int speed = 10;
    private void Start()
    {
        
        commingV = comming.GetComponent<UnityEngine.Video.VideoPlayer>(); 
        disV = disappear.GetComponent<UnityEngine.Video.VideoPlayer>();
        commesh = comming.GetComponent<MeshRenderer>();
        dismesh = disappear.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        dtime += Time.deltaTime;
        changeCloud();
    }

    private void changeData()
    {

        int geoIdx = GameController.Instance.geograpyIndex;


        if (dtime > 8f)
        {
            dismesh.enabled = false;
            dtime = 0;
        }
        else if (dtime > 6.1f)
        {
            if (!disV.isPlaying)
            {
                disV.Play();
                Debug.Log("play");
            }
            
            
        }
        else if (dtime > 6f)
        {
            dismesh.enabled = true;
            commesh.enabled = false;
        }

        else if (dtime > 3.1f)
        {
            commesh.enabled = true;

        }
        else if (dtime > 3f)
        {
            if (!commingV.isPlaying)
            {
                commingV.Play();
            }
        }
    }
    private void changeCloud()
    { 
        if(dtime > changetime && SqOn)
        {
            
            if (!commingV.isPlaying)
            {
                if (!Isplay)
                {
                    commingV.Play();
                }
                else
                {
                    Isplay = false;
                    SqOn = false;
                    dtime = 0;
                    changeMap();
                }
                return;
            }
            else if (dtime > (changetime + 0.1))
            {
                commesh.enabled = true;
                Isplay = true;
            }

        }
    else if(dtime > 3.2f && !SqOn)
        {
            if (!disV.isPlaying)
            {
                if (!Isplay)
                {
                    GameController.Instance.speed = speed;
                    disV.Play();
                }
                else
                {
                    Isplay = false;
                    dismesh.enabled = false;
                    SqOn = true;
                    dtime = 0;
                }
                return;
            }
            else if (dtime > 3.3)
            {
                commesh.enabled = false;
                dismesh.enabled = true;
                Isplay = true;
            }
        }
    }

    private void changeMap()
    {
        speed = GameController.Instance.speed;
        if((GameController.Instance.geograpyIndex+1) >= GameController.Instance.maxIdx)
        {
            GameController.Instance.geograpyIndex = 0;
        }
        else
        {
            GameController.Instance.geograpyIndex++;
        }
        GameController.Instance.speed = 500;
        
    }
}

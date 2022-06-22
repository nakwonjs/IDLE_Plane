using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float sprint;
    public float time = 5f;

    // Start is called before the first frame update
    void Start() {
        sprint = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            if (GameController.Instance.gas > 0)
            {
                Vector3 move = new Vector3(0, 0, sprint * Time.deltaTime);
                transform.Translate(move);
                time -= Time.deltaTime;
            }
        }
        
    }
}

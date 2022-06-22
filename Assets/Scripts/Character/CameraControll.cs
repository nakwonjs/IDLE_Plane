using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public float speed = 10.0f;
    public float Max;
    public float Min;

    private Camera camera;
    private Transform transform;




    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * speed;

        if (camera.fieldOfView <= Min && scroll > 0)
            camera.fieldOfView = Min;
        else if (camera.fieldOfView >= Max && scroll < 0)
            camera.fieldOfView = Max;
        else
        {
            camera.fieldOfView -= scroll;
        }
        

        //if (transform.localPosition.z < Min && scroll > 0)
        //    scroll = 0;
        //else if (transform.localPosition.z > Max && scroll < 0)
        //    scroll = 0;
        //Vector3 move = new Vector3(scroll,0,0);
        //transform.Translate(move);
    }
}

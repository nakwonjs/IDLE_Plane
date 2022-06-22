using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    Transform transform;
    public int drone;
    public double goldLevel;

    public GameObject[] droneArray;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent< Transform >();
        drone = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        goldLevel = GameController.Instance.goldLevel;
        if (goldLevel > drone*0.6+1.2)
        {
            //GameObject I = Instantiate(droneArray[drone%6], new Vector3(transform.position.x +25f + ((drone/ 6)*5), transform.position.y , transform.position.z + (5 * (drone % 6 - 2))), Quaternion.identity);
            GameObject I = Instantiate(droneArray[drone % 6]);

            I.transform.parent = this.transform;
            I.transform.localPosition = new Vector3((5 * (drone % 6 - 2)), 0, -10f + ((drone / 6) * -5));

            //I.transform.SetParent(this.transform);
            I.transform.Rotate(0, -90, 0);




            drone++;
        }

    }
}

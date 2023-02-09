using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    public GameObject doorL;
    public GameObject doorR;
    public float doorLSpeed;
    public float doorRSpeed;
    public float limitL;
    private float limitLCounter;
    public float limitR;
    private float limitRCounter;    

    public bool activated;
    public bool zAxis = false;

    void FixedUpdate()
    {
        if (activated && !zAxis)
        {
            if (limitLCounter >= limitL)
            {
                doorL.transform.position = new Vector3(doorL.transform.position.x - doorLSpeed, doorL.transform.position.y, doorL.transform.position.z);
                limitLCounter -= doorLSpeed;
            }
            if (limitRCounter <= limitR)
            {
                doorR.transform.position = new Vector3(doorR.transform.position.x + doorRSpeed, doorR.transform.position.y, doorR.transform.position.z);
                limitRCounter += doorRSpeed;
            }
        }
        if (!activated && !zAxis)
        {
            if (limitLCounter <= 0)
            {
                doorL.transform.position = new Vector3(doorL.transform.position.x + doorLSpeed, doorL.transform.position.y, doorL.transform.position.z);
                limitLCounter += doorLSpeed;
            }
            if (limitRCounter >= 0)
            {
                doorR.transform.position = new Vector3(doorR.transform.position.x - doorRSpeed, doorR.transform.position.y, doorR.transform.position.z);
                limitRCounter -= doorRSpeed;
            }
        }

        if (activated && zAxis)
        {
            if (limitLCounter >= limitL)
            {
                doorL.transform.position = new Vector3(doorL.transform.position.x, doorL.transform.position.y, doorL.transform.position.z - doorLSpeed);
                limitLCounter -= doorLSpeed;
            }
            if (limitRCounter <= limitR)
            {
                doorR.transform.position = new Vector3(doorR.transform.position.x, doorR.transform.position.y, doorR.transform.position.z + doorRSpeed);
                limitRCounter += doorRSpeed;
            }
        }

        if (!activated && zAxis)
        {
            if (limitLCounter <= 0)
            {
                doorL.transform.position = new Vector3(doorL.transform.position.x, doorL.transform.position.y, doorL.transform.position.z + doorLSpeed);
                limitLCounter += doorLSpeed;
            }
            if (limitRCounter >= 0)
            {
                doorR.transform.position = new Vector3(doorR.transform.position.x, doorR.transform.position.y, doorR.transform.position.z - doorRSpeed);
                limitRCounter -= doorRSpeed;
            }
        }

    }
}

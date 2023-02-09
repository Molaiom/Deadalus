using System.Collections;
using UnityEngine;

public class WardrobeDoor : DetectableByJanitor
{
    public GameObject[] doors;

    // Use this for initialization
    void Start()
    {
        
    }

    public void OpenDoor(float rot)
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door2>().RotateDoor(rot);
        }

    }

    public bool CheckOpen()
    {
        bool isOpen = false;

        foreach (GameObject door in doors)
        {
            if (door.GetComponent<Door2>().GetAngle() > 15f)
            {
                isOpen = true;
            }
        }

        return isOpen;

    }

}

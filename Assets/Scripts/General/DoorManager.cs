using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void OpenCloseDoor(int id, bool opened)
    {
        if (id == 0)
        {
            if (opened)
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    doorObject.OpenDoor();

                }
            }
            else
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    doorObject.CloseDoor();

                }
            }
        }
        else
        {
            if (opened)
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    if (doorObject.id == id)
                    {
                        doorObject.OpenDoor();
                    }
                }
            }
            else
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    if (doorObject.id == id)
                    {
                        doorObject.CloseDoor();
                    }
                }
            }
        }
    }

    public void OpenCloseDoorALittle(int id, bool opened)
    { 
        if (opened)
        {
            Door2[] doors = FindObjectsOfType<Door2>();

            for (int i = 0; i < doors.Length; i++)
            {
                GameObject o = doors[i].gameObject;
                Door2 doorObject = o.GetComponent<Door2>();

                if (doorObject.id == id)
                {
                    doorObject.OpenDoorALittle();
                }
            }
        }
        else
        {
            Door2[] doors = FindObjectsOfType<Door2>();

            for (int i = 0; i < doors.Length; i++)
            {
                GameObject o = doors[i].gameObject;
                Door2 doorObject = o.GetComponent<Door2>();

                if (doorObject.id == id)
                {
                    doorObject.CloseDoor();
                }
            }
        }
    }

    public void LockUnlockDoor(int id, bool unlocked)
    {
        // ALL DOORS
        if (id == 0)
        {
            if (unlocked)
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    if (doorObject.type == Types.Glass)
                    {
                        doorObject.UnlockDoor();
                    }

                    doorObject.UnlockDoor();
                }
            }
            else
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    doorObject.LockDoor();

                }
            }
        }

        // SPECIFIC ID'S
        else
        {
            if (unlocked)
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    if (doorObject.id == id)
                    {
                        doorObject.UnlockDoor();
                    }
                }
            }
            else
            {
                Door2[] doors = FindObjectsOfType<Door2>();

                for (int i = 0; i < doors.Length; i++)
                {
                    GameObject o = doors[i].gameObject;
                    Door2 doorObject = o.GetComponent<Door2>();

                    if (doorObject.id == id)
                    {
                        doorObject.LockDoor();
                    }
                }
            }
        }
    }
}

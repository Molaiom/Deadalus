using UnityEngine;

public class SequenceTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "Awaking":
                SequenceManager.instance.Sequence0Trigger();
                break;

            case "FlashLight Rolling":
                SequenceManager.instance.Sequence1Trigger();
                break;

            case "Janitor On Corridor":
                if(FindObjectOfType<JanitorController>() != null)
                    SequenceManager.JanitorCorridorEvent += FindObjectOfType<JanitorController>().CorridorEvent;

                SequenceManager.instance.Sequence2Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Janitor On Duct":
                JanitorSpawner.instance.SpawnJanitorOnRoomBeforeDuct();                                    
                SequenceManager.instance.Sequence5Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Corridor Chase Left":
                SequenceManager.instance.Sequence7TriggerLeft();
                break;

            case "Corridor Chase Right":
                SequenceManager.instance.Sequence7TriggerRight();
                break;

            case "Janitor Walking to Lab 9":
                if (gameObject.GetComponent<PlayerController>().tapes.Contains(2))                    
                SequenceManager.instance.Sequence9Trigger();
                break;

            case "Room 2 Trigger 1":
                SequenceManager.instance.Sequence11Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Room 9994 Trigger":
                SequenceManager.instance.Sequence15Trigger();
                other.gameObject.SetActive(false);
                break;

            case "After Room 9994 Trigger":
                SequenceManager.instance.Sequence16Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Room 9991 Trigger":
                SequenceManager.instance.Sequence17Trigger();
                other.gameObject.SetActive(false);
                break;

            case "After Room 9991 Trigger":
                SequenceManager.instance.Sequence18Trigger();
                other.gameObject.SetActive(false);
                break;

            case "ThirdFloorElevator":
                SequenceManager.instance.Sequence20Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Reaching Second Floor":
                SequenceManager.instance.Sequence21Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Janitor In Front Of Consultory":
                SequenceManager.instance.Sequence23Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Front of Consultory Aggro Trigger":
                SequenceManager.instance.Sequence24Trigger();                
                break;

            case "Consultory":
                if (FindObjectOfType<JanitorController>() != null)
                {
                    DestroyObject(FindObjectOfType<JanitorController>().gameObject);                    
                }
                StartCoroutine(FindObjectOfType<PlayerController>().AudioSourceFade(false, 2.5f, 0));
                DoorManager.instance.OpenCloseDoor(-5, false);
                DoorManager.instance.LockUnlockDoor(-5, false);
                other.gameObject.SetActive(false);
                break;

            case "Destroy Janitor":
                if(FindObjectOfType<JanitorController>() != null)
                {
                    DestroyObject(FindObjectOfType<JanitorController>().gameObject);
                }
                FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().clip = Sons.instance.MusicaPreenchimento1;
                FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().Play();

                DoorManager.instance.OpenCloseDoor(14, false);
                DoorManager.instance.LockUnlockDoor(14, false);
                other.gameObject.SetActive(false);
                break;

            case "Dummies On Corridor":
                SequenceManager.instance.Sequence25Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Elevator Trigger":
                FindObjectOfType<PlayerController>().ChangeRootState(true);
                SequenceManager.instance.secondFloorElevatorPanel.CloseElevator();
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                break;

            case "Third Floor Trigger":
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                SequenceManager.instance.thirdFloorElevatorPanel.ChangeActiveState(true);
                SequenceManager.instance.thirdFloorElevatorPanel.CloseElevator();
                SequenceManager.instance.thirdFloorElevatorPanel.ChangeActiveState(false);

                DoorManager.instance.OpenCloseDoor(0, false);
                DoorManager.instance.LockUnlockDoor(0, false);

                DoorManager.instance.LockUnlockDoor(1, true);
                DoorManager.instance.OpenCloseDoor(1, true);

                DoorManager.instance.LockUnlockDoor(18, true);

                EventManager.instance.Event4Trigger();

                Generator[] g = FindObjectsOfType<Generator>();
                for (int i = 0; i < g.Length; i++)
                {
                    if(g[i].id == 3)
                    {
                        g[i].InteractGenerator();
                    }
                }
                break;

            case "Reaching Ground Floor":
                SequenceManager.instance.Sequence29Trigger();
                other.gameObject.SetActive(false);
                break;

            case "Ground Floor Elevator Trigger":
                SequenceManager.instance.Sequence30Trigger();
                break;

            default:
                break;
        }
    }
}

using UnityEngine;

public class EventTrigger : MonoBehaviour
{

    public static bool listenTape3 = false;
    public static bool listenTape4 = false;

    public static EventTrigger instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "LampFall":
                if (gameObject.GetComponent<PlayerController>().tapes.Contains(2)) {
                    EventManager.instance.Event1Trigger();
                }
                break;
            case "OpenFireDoorTape3":
                if (listenTape3)
                {
                    EventManager.instance.Event5Trigger();
                }
                break;
            case "CloseFireDoor":
                if (listenTape3)
                {
                    EventManager.instance.Event6Trigger();                                        
                    other.gameObject.SetActive(false);
                }
                break;

            case "MannequinFall":
                if (listenTape4)
                {
                    EventManager.instance.Event7Trigger();
                }
                break;
            case "MannequinLockCorridor":
                EventManager.instance.Event8Trigger();
                EventManager.instance.Event9Trigger();

                EventManager.instance.mannequinShovel.SetActive(true);
                break;
            case "ShovelFall":
                if (listenTape4)
                {
                    EventManager.instance.Event10Trigger();
                }
                break;
            case "Landslip":
                if (gameObject.GetComponent<PlayerController>().tapes.Contains(5)){
                    gameObject.GetComponent<PlayerController>().openingDoor = false;
                    gameObject.GetComponent<PlayerController>().door = null;
                    EventManager.instance.Event11Trigger();
                }
                break;
            case "CloseInicialDoor":
                EventManager.instance.Event12Trigger();
                break;
            default:
                break;
        }
    }
}

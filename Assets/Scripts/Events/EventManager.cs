using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EventManager : MonoBehaviour
{
    #region Attributes
    public static EventManager instance;

    public GameObject mannequinCorridor;
    public GameObject mannequinShovel;
    #endregion

    #region DELEGATES
    public delegate IEnumerator Event1Action();
    public static event Event1Action LampFall;

    public delegate void Event2Action();
    public static event Event2Action ClinicAlarm;

    public delegate void Event3Action();
    public static event Event3Action LockThirdFloorDoors;

    public delegate void Event4Action();
    public static event Event4Action ElevatorFall;

    public delegate void Event5Action();
    public static event Event5Action OpenFireDoorTape3;

    public delegate void Event6Action();
    public static event Event6Action CloseFireDoor;

    public delegate void Event7Action();
    public static event Event7Action MannequinFall;

    public delegate void Event8Action();
    public static event Event8Action MannequinLockCorridor;

    public delegate void Event9Action();
    public static event Event9Action OpenFireDoor;

    public delegate void Event10Action();
    public static event Event10Action ShovelFall;

    public delegate IEnumerator Event11Action();
    public static event Event11Action Landslip;

    public delegate void Event12Action();
    public static event Event12Action CloseInicialDoor;

    #endregion 

    #region EVENTS
    public void Event1Trigger() // LAMPADA CAINDO
    {
        if (LampFall != null)
            StartCoroutine(LampFall());
    }

    public void Event2Trigger() // ALARME TOCANDO QUANDO ABRE ALGUMA PORTA DA CLINICA
    {
        if (ClinicAlarm != null)
            ClinicAlarm();
    }

    public void Event3Trigger() 
    {
        if (LockThirdFloorDoors != null)
            LockThirdFloorDoors();
    }

    public void Event4Trigger()
    {
        if (ElevatorFall != null)
            ElevatorFall();
    }

    public void Event5Trigger()
    {
        if (OpenFireDoorTape3 != null)
            OpenFireDoorTape3();
        
    }
    
    public void Event6Trigger()
    {
        if (CloseFireDoor != null)
            CloseFireDoor();
    }

    public void Event7Trigger()
    {
        if (MannequinFall != null)
            MannequinFall();
    }

    public void Event8Trigger()
    {
        if (MannequinLockCorridor != null)
            MannequinLockCorridor();
    }

    public void Event9Trigger()
    {
        if (OpenFireDoor != null)
            OpenFireDoor();
    }

    public void Event10Trigger()
    {
        if (ShovelFall != null)
            ShovelFall();
    }

    public void Event11Trigger()
    {
        if (Landslip != null)
            StartCoroutine(Landslip());
    }

    public void Event12Trigger()
    {
        if (CloseInicialDoor != null)
            CloseInicialDoor();
    }
    #endregion

    #region Methods
    private void Awake()
    {
        instance = this;
    }
    #endregion
}

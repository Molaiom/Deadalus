using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : DetectableByJanitor
{

    public int id;
    public bool isActive = false;
    public GameObject door;
    public ElevatorPanel elevator;
    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SequenceManager.AfterRoom9994Event += AfterRoom9994Event;
        SequenceManager.ReachingSecondFloor += AfterFirstElevatorMethod;
        isActive = false;
    }

    public void InteractGenerator()
    {
        if (!isActive)
        {
            isActive = true;
            DetectionValue = 5;
            audioSource.clip = Sons.instance.GeradorLigado;
            audioSource.loop = true;

            if (!audioSource.isPlaying)
                audioSource.Play();

            UnlockDoor();
            if(elevator != null)
                elevator.ChangeActiveState(true);

            if (id == 1)
                SequenceManager.instance.Sequence14Trigger();

            if (id == 2)
            {
                SequenceManager.instance.Sequence19Trigger();                
            }

            if (id == 3)
            {                
                SequenceManager.instance.ElevatorTrigger.GetComponent<BoxCollider>().enabled = true;
            }

            if (id == 4)
            {                
                SequenceManager.instance.ElevatorTrigger3.SetActive(true);
            }

            print("Generator " + id + " Activated");
        }
        /*
        else
        {
            isActive = false;
            DetectionValue = 0;
            audioSource.Stop();
            LockDoor();

            if(elevator != null)            
                elevator.ChangeActiveState(false);
            
            if(id == 3)
            {
                SequenceManager.instance.ElevatorTrigger.GetComponent<BoxCollider>().enabled = true;
            }
            if(id == 4)
            {
                SequenceManager.instance.ElevatorTrigger3.SetActive(true);
            }

            print("Generator " + id + " Deactivated");
        }
        */
        
    }

    public void UnlockDoor()
    {
        if (door.gameObject != null && door.GetComponent<Door2>() != null)
        {
            door.GetComponent<Door2>().isLocked = false;
        }
    }

    public void LockDoor()
    {
        if (door.gameObject != null && door.GetComponent<Door2>() != null)
        {
            door.GetComponent<Door2>().isLocked = true;
        }
    }

    public void AfterRoom9994Event() // MÉTODO PARA SEQUENCIA AO SAIR DA SALA 9994 (SEQUENCIA 16)
    {
        if (id == 1)
        {
            SequenceManager.AfterRoom9994Event -= AfterRoom9994Event;
            DoorManager.instance.OpenCloseDoor(9, false);
            InteractGenerator();
        }
    }

    public void AfterFirstElevatorMethod() // MÉTODO PARA QUANDO O PLAYER SAI DO PRIMEIRO ELEVADOR E CHEGA NO SEGUNDO ANDAR
    {
        if (id == 2)
        {
            SequenceManager.ReachingSecondFloor -= AfterFirstElevatorMethod;
            InteractGenerator();

            SequenceManager.instance.secondFloorElevatorPanel.CloseElevator();
            SequenceManager.instance.secondFloorElevatorPanel.ChangeActiveState(false);

            for (int i = 0; i < SequenceManager.instance.appearAfterSecondRoom.Length; i++)
            {
                SequenceManager.instance.appearAfterSecondRoom[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < SequenceManager.instance.disappearAfterSecondRoom.Length; i++)
            {
                SequenceManager.instance.disappearAfterSecondRoom[i].gameObject.SetActive(false);
            }

        }
    }
}

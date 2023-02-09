using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class SequenceManager : MonoBehaviour
{
    #region Attributes
    public static SequenceManager instance;
    
    [Header("--- Salas Vivas 1 ---")]
    [Header("")]
    public GameObject firstRoomDuct;   

    public GameObject[] appearOnFirstRoom;
    public GameObject[] disappearOnFirstRoom;

    public GameObject[] appearOnFirstRoom2;
    public GameObject[] disappearOnFirstRoom2;
    
    public GameObject[] disappearOnFirstRoom3;

    public GameObject[] appearOnFirstRoom4;
    public GameObject[] disappearOnFirstRoom4;

    [Header("--- Salas Vivas 2 ---")]
    [Header("")]
    public GameObject[] appearOnSecondRoom;
    public GameObject[] disappearOnSecondRoom;

    public GameObject appearAtRoom9994;
    public GameObject appearAtRoom9991;

    public GameObject[] appearOnSecondRoomFinal;
    public GameObject[] disappearOnSecondRoomFinal;

    public GameObject[] appearAfterSecondRoom;
    public GameObject[] disappearAfterSecondRoom;

    [Header("--- Janitor Cutscene Destinations ---")]
    [Header("")]

    public Transform ductSceneFirstDestination;
    public Transform ductSceneSecondDestination;
    public Transform destinationAfterLab9;
    public GameObject destinationAfterTRoom;
    public Transform secondFloorCorridorDestination;

    [Header("--- Misc ---")]
    [Header("")]
    public NavMeshSurface navMeshSurface;
    public GameObject salaDeRadio;
    public GameObject salinha;
    public GameObject SecFloorRandTargets;
    public GameObject dummiesToAppear;
    public GameObject dummiesToDisappear;
    public GameObject dummiesToAppear2;
    public GameObject dummiesToDisappear2;
    public AudioSource dummiesAudio;
    public GameObject janitorAggroTrigger;
    public ElevatorPanel thirdFloorElevatorPanel;
    public Transform thirdFloorElevatorDestination;
    public ElevatorPanel secondFloorElevatorPanel;
    public ElevatorPanel secondFloorElevatorPanel2;
    public GameObject ElevatorTrigger;
    public GameObject ElevatorTrigger2;
    public Transform secondFloorElevatorDestination;
    public ElevatorPanel groundFloorElevatorPanel1;
    public ElevatorPanel groundFloorElevatorPanel2;
    public GameObject ElevatorTrigger3;    
    public GameObject groundFloorElevator;
    public GameObject salaDeRadioDoor;
    public Image blackScreen;

    private bool listenTape5 = false;
    #endregion

    #region DELEGATES
    public delegate void Sequence0Action();
    public static event Sequence0Action AwakingEvent;

    public delegate void Sequence1Action();
    public static event Sequence1Action FlashlightEvent;

    public delegate IEnumerator Sequence2Action();
    public static event Sequence2Action JanitorCorridorEvent;

    public delegate void Sequence3Action();
    public static event Sequence3Action RadioRoomOpening;

    public delegate void Sequence4Action();
    public static event Sequence4Action FirstRoom;

    public delegate IEnumerator Sequence5Action();
    public static event Sequence5Action JanitorOnDuct;

    public delegate void Sequence6Action();
    public static event Sequence6Action CorridorAppear;

    public delegate void Sequence7Action(bool leftSide);
    public static event Sequence7Action CorridorChase;

    public delegate void Sequence8Action();
    public static event Sequence8Action EndOfFirstRoom;

    public delegate IEnumerator Sequence9Action();
    public static event Sequence9Action JanitorOnLab9;

    public delegate void Sequence10Action();
    public static event Sequence10Action SecondRoom;

    public delegate void Sequence11Action();
    public static event Sequence11Action SecondRoomRadioRoomClose;

    public delegate IEnumerator Sequence12Action();
    public static event Sequence12Action JanitorAppearOnSecondRoom;

    public delegate IEnumerator Sequence13Action();
    public static event Sequence13Action JanitorGoesAwayAfterTRoom;

    public delegate void Sequence14Action();
    public static event Sequence14Action FirstGeneratorActivated;

    public delegate IEnumerator Sequence15Action();
    public static event Sequence15Action Room9994Event;

    public delegate void Sequence16Action();
    public static event Sequence16Action AfterRoom9994Event;

    public delegate void Sequence17Action();
    public static event Sequence17Action JanitorLeavesRoom9991;

    public delegate void Sequence18Action();
    public static event Sequence18Action PlayerEscapesRoom9991;

    public delegate IEnumerator Sequence19Action();
    public static event Sequence19Action EndOfSecondRoom;

    public delegate IEnumerator Sequence20Action();
    public static event Sequence20Action ElevatorAfterSecondRoom;

    public delegate void Sequence21Action();
    public static event Sequence21Action ReachingSecondFloor;

    public delegate IEnumerator Sequence22Action();
    public static event Sequence22Action SecondFloorCorridor;

    public delegate void Sequence23Action();
    public static event Sequence23Action JanitorInFrontOfConsultory1;

    public delegate void Sequence24Action();
    public static event Sequence24Action JanitorInFrontOfConsultory2;

    public delegate void Sequence25Action();
    public static event Sequence25Action DummiesOnCorridor;

    public delegate void Sequence26Action();
    public static event Sequence26Action DummiesOnCorridor2;

    public delegate IEnumerator Sequence27Action();
    public static event Sequence27Action ThirdTapeEvent;

    public delegate IEnumerator Sequence28Action();
    public static event Sequence28Action JanitorAttackOnElevator;

    public delegate void Sequence29Action();
    public static event Sequence29Action ReachingGroundFloor;

    public delegate void Sequence30Action();
    public static event Sequence30Action EnterGroundElevator;

    public delegate IEnumerator Sequence31Action();
    public static event Sequence31Action GroundElevatorGoesToRadioRoom;

    public delegate void Sequence32Action();
    public static event Sequence32Action ListenToFourthTape;

    public delegate IEnumerator Sequence33Action();
    public static event Sequence33Action ListenToFifthTape;

    public delegate IEnumerator Sequence34Action();
    public static event Sequence34Action LastSequence;
    #endregion

    #region EVENTS
    public void Sequence0Trigger() // PLAYER ACORDANDO
    {
        if (AwakingEvent != null)
            AwakingEvent();
    }

    public void Sequence1Trigger() // LANTERNA ARRASTANDO NA FRENTE DA PORTA
    {
        if (FlashlightEvent != null)            
            FlashlightEvent();
    }

    public void Sequence2Trigger() // ZELADOR APARECENDO NO FINAL DO CORREDOR
    {
        if (JanitorCorridorEvent != null)
            StartCoroutine (JanitorCorridorEvent());
    }

    public void Sequence3Trigger() // SALA DE RÁDIO ABRE
    {
        if (RadioRoomOpening != null)
            RadioRoomOpening();
    }

    public void Sequence4Trigger() // SALA VIVA 1 APARECE EM FRENTE A SALA DE RÁDIO
    {
        if (FirstRoom != null)
            FirstRoom();
    }

    public void Sequence5Trigger() // ZELADOR VERIFICA O DUTO NA PRIMEIRA SALA VIVA
    {
        if (JanitorOnDuct != null)
            StartCoroutine (JanitorOnDuct());
    }
    
    public void Sequence6Trigger() // CORREDOR DA SALA VIVA 1 APARECE E PAREDE COM DUTO SOME
    {
        if (CorridorAppear != null)
            CorridorAppear();
    }
    
    public void Sequence7TriggerLeft() // SPAWNA O ZELADOR NO FINAL DO CORREDOR MISTERIOSO
    {
        if (CorridorChase != null)
            CorridorChase(true);
    }

    public void Sequence7TriggerRight() // SPAWNA O ZELADOR NO FINAL DO CORREDOR MISTERIOSO
    {
        if (CorridorChase != null)
            CorridorChase(false);
    }
    
    public void Sequence8Trigger() // SALA VIVA 1 DESAPARECE E AMBIENTE RETORNA AO NORMAL
    {
        if(EndOfFirstRoom != null)
        {
            EndOfFirstRoom();
        }
    }
    
    public void Sequence9Trigger() // ZELADOR PASSA EM FRENTE AO LAB 9
    {
        if (JanitorOnLab9 != null)
            StartCoroutine (JanitorOnLab9());
    }
    
    public void Sequence10Trigger() // SALA VIVA 2 APARECE EM FRENTE A SALA DE RÁDIO
    {
        if (SecondRoom != null)
            SecondRoom();
    }

    public void Sequence11Trigger() // SALA DE RADIO FECHA
    {
        if (SecondRoomRadioRoomClose != null)
            SecondRoomRadioRoomClose();
    }
    
    public void Sequence12Trigger() // ZELADOR APARECE NA SALA VIVA 2 PELA PRIMEIRA VEZ
    {
        if (JanitorAppearOnSecondRoom != null)
            StartCoroutine(JanitorAppearOnSecondRoom());
    }
    
    public void Sequence13Trigger() // ZELADOR VAI EMBORA DA SALA VIVA 2 APÓS JORNAL QUEIMAR
    {
        if (JanitorGoesAwayAfterTRoom != null)
            StartCoroutine(JanitorGoesAwayAfterTRoom());
    }
    
    public void Sequence14Trigger() // PRIMEIRO GERADOR É ATIVADO E ZELADOR APARECE
    {
        if (FirstGeneratorActivated != null)
            FirstGeneratorActivated();
    }
    
    public void Sequence15Trigger() // PLAYER ENTRA NA SALA 9994
    {
        if (Room9994Event != null)
            StartCoroutine(Room9994Event());
    }
    
    public void Sequence16Trigger() // PLAYER SAI DA SALA 9994
    {
        if (AfterRoom9994Event != null)
            AfterRoom9994Event();
    }
    
    public void Sequence17Trigger() // PLAYER QUEIMA O JORNAL NA SALA 9992 OU ENTRA NA SALA 9991
    {
        if (JanitorLeavesRoom9991 != null)
            JanitorLeavesRoom9991();
    }
    
    public void Sequence18Trigger() // PLAYER ESCAPA DA SALA 9991 E ENTRA NO CUBÍCULO COM GERADOR
    {
        if (PlayerEscapesRoom9991 != null)
            PlayerEscapesRoom9991();
    }
    
    public void Sequence19Trigger() // SALA VIVA 2 DESAPARECE E AMBIENTE RETORNA AO NORMAL
    {
        if (EndOfSecondRoom != null)
           StartCoroutine(EndOfSecondRoom());
    }
    
    public void Sequence20Trigger() // ELEVADOR DESCE SOZINHO APÓS SALA VIVA 2
    {
        if (ElevatorAfterSecondRoom != null)
            StartCoroutine(ElevatorAfterSecondRoom());
    }
    
    public void Sequence21Trigger() // PLAYER CHEGA NO SEGUNDO ANDAR APÓS SAIR DO ELEVADOR
    {
        if (ReachingSecondFloor != null)
            ReachingSecondFloor();
    }
    
    public void Sequence22Trigger() // PLAYER QUEBRA O VRIDRO DE ALGUMA PORTA DO SEGUNDO ANDAR
    {
        if (SecondFloorCorridor != null)
           StartCoroutine(SecondFloorCorridor());
    }
    
    public void Sequence23Trigger() // JANITOR SPAWNS IN FRONT OF CONSULTORY
    {        
        if (JanitorInFrontOfConsultory1 != null)
            JanitorInFrontOfConsultory1();
    }

    public void Sequence24Trigger() // JANITOR START'S CHASING THE PLAYER IN FRONT OF CONSULTORY
    {
        if (JanitorInFrontOfConsultory2 != null)
            JanitorInFrontOfConsultory2();
    }
    
    public void Sequence25Trigger() // DUMMIES ON TIGHT CORRIDOR
    {
        if (DummiesOnCorridor != null)
            DummiesOnCorridor();
    }

    public void Sequence26Trigger() // DUMMIES ON TIGHT CORRIDOR 2
    {
        if (DummiesOnCorridor2 != null)
            DummiesOnCorridor2();
    }

    public void Sequence27Trigger() // DUMMIES ON THE DUMMY ROOM
    {
        if (ThirdTapeEvent != null)
            StartCoroutine(ThirdTapeEvent());
    }
   
    public void Sequence28Trigger() // JANITOR ATTACK ON THE ELEVATOR
    {
        if (JanitorAttackOnElevator != null)
            StartCoroutine(JanitorAttackOnElevator());
    }
    
    public void Sequence29Trigger() // PLAYER REACHES GROUND FLOOR
    {
        if (ReachingGroundFloor != null)
            ReachingGroundFloor();
    }
    
    public void Sequence30Trigger() // PLAYER ENTERS GROUND FLOOR ELEVATOR
    {
        if (EnterGroundElevator != null)
            EnterGroundElevator();
    }
   
    public void Sequence31Trigger() // ELEVATOR GOES TO THE RADIO ROOM
    {
        if (GroundElevatorGoesToRadioRoom != null)
            StartCoroutine(GroundElevatorGoesToRadioRoom());
    }
   
    public void Sequence32Trigger() // PLAYER LISTENS TO THE FOURTH TAPE AND THE ELEVATOR DISAPPEARS
    {
        if (ListenToFourthTape != null)
            ListenToFourthTape();
    }

    public void Sequence33Trigger() // PLAYER LISTENS TO THE FIFTH TAPE
    {
        if (ListenToFifthTape != null)
            StartCoroutine(ListenToFifthTape());
    }

    public void Sequence34Trigger() // AFTER PLAYER LISTENS TO THE FIFTH TAPE
    {
        if (LastSequence != null)
            StartCoroutine(LastSequence());
    }
    #endregion

    #region Methods
    private void Awake()
    {
        instance = this;
        FirstRoom += FirstRoomMethod;
        SecondRoom += SecondRoomMethod;
        SecondRoomRadioRoomClose += SecondRoomMethod2;
        JanitorAppearOnSecondRoom += SecondRoomMethod3;
        JanitorGoesAwayAfterTRoom += SecondRoomMethod4;
        JanitorLeavesRoom9991 += SecondRoomMethod5;
        PlayerEscapesRoom9991 += SecondRoomMethod6;
        EndOfSecondRoom += SecondRoomMethod7;
        ElevatorAfterSecondRoom += ElevatorMethod1;
        DummiesOnCorridor += DummiesMethod;
        ThirdTapeEvent += DummiesMethod3;
        JanitorAttackOnElevator += ElevatorMethod2;
        ReachingGroundFloor += ReachingGroundFloorMethod;
        EnterGroundElevator += ElevatorMethod3;
        GroundElevatorGoesToRadioRoom += ElevatorMethod4;
        ListenToFourthTape += ElevatorMethod5;
    }

    public void FirstRoomMethod() // SEQUENCE 4 
    {
        FirstRoom -= FirstRoomMethod;
        CorridorAppear += FirstRoomMethod2;

        for (int i = 0; i < appearOnFirstRoom.Length; i++)
        {
            appearOnFirstRoom[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < disappearOnFirstRoom.Length; i++)
        {
            disappearOnFirstRoom[i].gameObject.SetActive(false);
        }

        DoorManager.instance.OpenCloseDoor(1, false);
        LightManager.instance.SetLightState(3, true);

        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        player.transform.SetParent(salaDeRadio.transform);

        salaDeRadio.transform.position = new Vector3(103.05f, 15.52301f, 5.56f);
        salaDeRadio.transform.Rotate(new Vector3(0, 90, 0));

        player.transform.parent = null;

        BuildNavMesh();
       
    }
    
    public void FirstRoomMethod2() // SEQUENCE 6 
    {
        CorridorAppear -= FirstRoomMethod2;

        for (int i = 0; i < appearOnFirstRoom2.Length; i++)
        {
            appearOnFirstRoom2[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < disappearOnFirstRoom2.Length; i++)
        {
            disappearOnFirstRoom2[i].gameObject.SetActive(false);
        }

        DoorManager.instance.LockUnlockDoor(4, true);

        BuildNavMesh();
    }

    public void FirstRoomMethod3() // SEQUENCE 8 
    {
        EndOfFirstRoom -= FirstRoomMethod3;        

        // RESTORES THE LEVEL
        for (int i = 0; i < appearOnFirstRoom4.Length; i++)
        {
            appearOnFirstRoom4[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < disappearOnFirstRoom4.Length; i++)
        {
            disappearOnFirstRoom4[i].gameObject.SetActive(false);
        }

        salaDeRadio.transform.position = new Vector3(95.97f, 15.52301f, -9.119999f);
        salaDeRadio.transform.rotation = transform.rotation;

        // DESTROYS THE JANITOR
        if (FindObjectOfType<JanitorController>().gameObject != null)
        DestroyObject(FindObjectOfType<JanitorController>().gameObject);

        DoorManager.instance.LockUnlockDoor(0, false);
        DoorManager.instance.OpenCloseDoor(0, false);
        DoorManager.instance.LockUnlockDoor(4, true);
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().Stop();

        print("Evento");
        BuildNavMesh();
    }

    public void SecondRoomMethod() // SEQUENCE 10 
    {
        SecondRoom -= SecondRoomMethod;

        for (int i = 0; i < appearOnSecondRoom.Length; i++)
        {
            appearOnSecondRoom[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < disappearOnSecondRoom.Length; i++)
        {
            disappearOnSecondRoom[i].gameObject.SetActive(false);
        }

        DoorManager.instance.OpenCloseDoor(1, false);

        BuildNavMesh();
    }

    public void SecondRoomMethod2() // SEQUENCE 11 
    {
        SecondRoomRadioRoomClose -= SecondRoomMethod2;
        DoorManager.instance.OpenCloseDoor(1, false);
        DoorManager.instance.LockUnlockDoor(1, false);
    }

    public IEnumerator SecondRoomMethod3() // SEQUENCE 12 
    {
        JanitorAppearOnSecondRoom -= SecondRoomMethod3;

        //SPAWNS THE JANITOR
        JanitorSpawner.instance.SpawnJanitorOnRoom2_1();

        yield return new WaitForSeconds(0.5f);

        // OPENS AND CLOSES THE DOOR        
        DoorManager.instance.OpenCloseDoor(6, true);
        yield return new WaitForSeconds(1f);
        DoorManager.instance.OpenCloseDoor(6, false);

    }

    public IEnumerator SecondRoomMethod4() // SEQUENCE 13 
    {
        JanitorGoesAwayAfterTRoom -= SecondRoomMethod4;

        destinationAfterTRoom.SetActive(true);
        yield return new WaitForSeconds(6f);

        destinationAfterTRoom.SetActive(false);        
        DestroyObject(FindObjectOfType<JanitorController>().gameObject);
    }    

    public void SecondRoomMethod5() // SEQUENCE 17 
    {
        JanitorLeavesRoom9991 -= SecondRoomMethod5;

        appearAtRoom9991.gameObject.SetActive(true);
        DoorManager.instance.LockUnlockDoor(10, true);
        FindObjectOfType<JanitorController>().SetJanitorState("Neutral");
    }

    public void SecondRoomMethod6() // SEQUENCE 18 
    {
        PlayerEscapesRoom9991 -= SecondRoomMethod6;
        StartCoroutine(FindObjectOfType<PlayerController>().AudioSourceFade(false, 4f, 0));

        DoorManager.instance.LockUnlockDoor(-4, false);
        DoorManager.instance.OpenCloseDoor(-4, false);
        DestroyObject(FindObjectOfType<JanitorController>().gameObject);
    }

    public IEnumerator SecondRoomMethod7() // SEQUENCE 19 
    {
        EndOfSecondRoom -= SecondRoomMethod7;

        for (int i = 0; i < appearOnSecondRoomFinal.Length; i++)
        {
            appearOnSecondRoomFinal[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < disappearOnSecondRoomFinal.Length; i++)
        {
            disappearOnSecondRoomFinal[i].gameObject.SetActive(false);
        }

        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        player.transform.SetParent(salinha.transform);

        salinha.transform.parent = null;
        salinha.transform.position = new Vector3(126.8f,  16.19301f,  5.9f);
        salinha.transform.Rotate(new Vector3(0, 180, 0));

        player.transform.parent = null;

        LightManager.instance.SetLightState(0, false);
        LightManager.instance.SetLightState(2, true);        

        FindObjectOfType<PlayerController>().AudioSourceFade(false, 3, 0);
        yield return new WaitForSeconds(3);
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().volume = FindObjectOfType<PlayerController>().OriginalVolume;



        BuildNavMesh();
    }    

    public IEnumerator ElevatorMethod1() // SEQUENCE 20 
    {
        ElevatorAfterSecondRoom -= ElevatorMethod1;
        AudioSource aSourcePlayer = FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>();

        // ROOTS THE PLAYER AND SHUTS THE DOOR
        FindObjectOfType<PlayerController>().ChangeRootState(true);
        thirdFloorElevatorPanel.CloseElevator();
        thirdFloorElevatorPanel.ChangeActiveState(false);
        yield return new WaitForSeconds(4);

        // STARTS THE ELEVATOR
        aSourcePlayer.PlayOneShot(Sons.instance.ElevadorInicio, aSourcePlayer.volume);
        yield return new WaitForSeconds(2.376f);

        aSourcePlayer.clip = Sons.instance.ElevadorMovimentaçao;
        aSourcePlayer.Play();

        // TELEPORTS THE PLAYER
        FindObjectOfType<PlayerController>().gameObject.transform.position = thirdFloorElevatorDestination.position;
        LightManager.instance.SetLightState(6, true);
        yield return new WaitForSeconds(7);


        // STOPS THE ELEVATOR
        aSourcePlayer.Stop();
        aSourcePlayer.PlayOneShot(Sons.instance.ElevadorPara, aSourcePlayer.volume);
        yield return new WaitForSeconds(2.064f);

        secondFloorElevatorPanel.ChangeActiveState(true);       
        secondFloorElevatorPanel.OpenElevator();        
        aSourcePlayer.clip = Sons.instance.MusicaPreenchimento1;
        aSourcePlayer.Play();

        FindObjectOfType<PlayerController>().ChangeRootState(false);        
    }    

    public void DummiesMethod() // SEQUENCE 25 
    {
        DummiesOnCorridor -= DummiesMethod;
        DummiesOnCorridor2 += DummiesMethod2;
        dummiesAudio.PlayOneShot(Sons.instance.PortaCortafogoBatendoforte, dummiesAudio.volume);
        StartCoroutine(FindObjectOfType<PlayerController>().AudioSourceFade(false, 1.5f, 0));
        FindObjectOfType<PlayerController>().ChangeRootState(true);
        dummiesToDisappear.gameObject.SetActive(false);
    }

    public void DummiesMethod2() // SEQUENCE 26
    {
        DummiesOnCorridor2 -= DummiesMethod2;
        FindObjectOfType<PlayerController>().ChangeRootState(false);
        dummiesToAppear.gameObject.SetActive(true);
    }

    public IEnumerator DummiesMethod3() // SEQUENCE 27
    {
        ThirdTapeEvent -= DummiesMethod3;

        // LIGHTS OFF
        AudioSource playerAudio = FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>();
        playerAudio.PlayOneShot(Sons.instance.LampadaApagar, playerAudio.volume);
        LightManager.instance.SetLightState(6, false);
        LightManager.instance.SetFlashlightState(false);
        LightManager.instance.SetZippoState(false);
        dummiesToDisappear2.SetActive(false);
        DoorManager.instance.LockUnlockDoor(15, false);
        DoorManager.instance.OpenCloseDoor(15, false);

        yield return new WaitForSeconds(2f);

        // LIGHTS ON
        playerAudio.PlayOneShot(Sons.instance.LampadaAcender, playerAudio.volume);
        LightManager.instance.SetLightState(6, true);
        LightManager.instance.SetFlashlightState(true);
        LightManager.instance.SetZippoState(true);
        dummiesToAppear2.SetActive(true);

        // NEXT ROOM
        LightManager.instance.SetLightState(7, true);
        DoorManager.instance.LockUnlockDoor(16, true);

    }

    public IEnumerator ElevatorMethod2() // SEQUENCE 28
    {
        JanitorAttackOnElevator -= ElevatorMethod2;        
        yield return new WaitForSeconds(0.2f);
        AudioSource aSourcePlayer = FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>();
        float originalVolume = aSourcePlayer.volume;

        // ELEVATOR STARTS
        aSourcePlayer.PlayOneShot(Sons.instance.ElevadorInicio, aSourcePlayer.volume);
        yield return new WaitForSeconds(Sons.instance.ElevadorInicio.length);
        LightManager.instance.SetLightState(1, true);
        JanitorSpawner.instance.SpawnJanitorInFrontOfElevator();

        // ELEVATOR STOPS
        secondFloorElevatorPanel2.elevator.limitL = -0.04f;
        secondFloorElevatorPanel2.elevator.limitR = 0.04f;
        secondFloorElevatorPanel2.elevator.activated = true;        

        // JANITOR ATTACK
        aSourcePlayer.clip = (Sons.instance.MusicaDesespero1);
        aSourcePlayer.Play();
        aSourcePlayer.PlayOneShot(Sons.instance.PaBatendoelevador, aSourcePlayer.volume);
        yield return new WaitForSeconds(1);
        aSourcePlayer.PlayOneShot(Sons.instance.PaDebatendoelevador, aSourcePlayer.volume);

        // ELEVATOR GOES AWAY
        yield return new WaitForSeconds(4);        
        secondFloorElevatorPanel2.elevator.activated = false;
        aSourcePlayer.PlayOneShot(Sons.instance.ElevadorMovimentaçao, aSourcePlayer.volume);
        yield return new WaitForSeconds(0.1f);
        DestroyObject(FindObjectOfType<JanitorController>().gameObject);

        // ELEVATOR OPENS
        StartCoroutine (FindObjectOfType<PlayerController>().AudioSourceFade(false, 4, 0));
        yield return new WaitForSeconds(4);
        aSourcePlayer.Stop();
        aSourcePlayer.volume = originalVolume;

        FindObjectOfType<PlayerController>().gameObject.transform.position = secondFloorElevatorDestination.position;      
        thirdFloorElevatorPanel.ChangeActiveState(true);
        thirdFloorElevatorPanel.OpenElevator();       
        thirdFloorElevatorPanel.ChangeActiveState(false);
        FindObjectOfType<PlayerController>().ChangeRootState(false);
        ElevatorTrigger2.GetComponent<BoxCollider>().enabled = true;        

    }

    public void ReachingGroundFloorMethod() // SEQUENCE 29
    {
        ReachingGroundFloor -= ReachingGroundFloorMethod;
        FindObjectOfType<PlayerController>().AudioSourceFade(false, 3f, 0);
        DoorManager.instance.OpenCloseDoor(18, false);
        DoorManager.instance.LockUnlockDoor(18, false);
    }

    public void ElevatorMethod3() // SEQUENCE 30
    {
        EnterGroundElevator -= ElevatorMethod3;
        groundFloorElevatorPanel1.CloseElevator();        
        groundFloorElevatorPanel1.ChangeActiveState(false);

        groundFloorElevatorPanel2.ChangeActiveState(true);       
        FindObjectOfType<PlayerController>().ChangeRootState(true);
    }

    public IEnumerator ElevatorMethod4() // SEQUENCE 31
    {
        GroundElevatorGoesToRadioRoom -= ElevatorMethod4;

        AudioSource aSourcePlayer = FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>();

        // STARTS THE ELEVATOR
        aSourcePlayer.PlayOneShot(Sons.instance.ElevadorInicio, aSourcePlayer.volume);
        yield return new WaitForSeconds(Sons.instance.ElevadorInicio.length);
        LightManager.instance.SetLightState(2, true);

        // MOVES THE ELEVATOR
        aSourcePlayer.clip = Sons.instance.ElevadorMovimentaçao;
        aSourcePlayer.Play();
        yield return new WaitForSeconds(5.5f);
        salaDeRadioDoor.SetActive(false);

        // TELEPORTS THE PLAYER 
        FindObjectOfType<PlayerController>().gameObject.transform.SetParent(groundFloorElevator.transform);
        yield return new WaitForFixedUpdate();
        groundFloorElevator.transform.position = new Vector3(104.145f, 16.561f, -2.94f);
        groundFloorElevator.transform.rotation = transform.rotation * Quaternion.AngleAxis(270, Vector3.up);
        yield return new WaitForFixedUpdate();
        FindObjectOfType<PlayerController>().gameObject.transform.parent = null;
        FindObjectOfType<PlayerController>().ChangeRootState(false);

        // STOPS THE ELEVATOR
        aSourcePlayer.Stop();
        aSourcePlayer.PlayOneShot(Sons.instance.ElevadorPara, aSourcePlayer.volume);
        groundFloorElevatorPanel2.elevator.zAxis = true;
        groundFloorElevatorPanel2.OpenElevator();        
        yield return new WaitForSeconds(Sons.instance.ElevadorPara.length);
        
    }

    public void ElevatorMethod5() // SEQUENCE 32
    {
        ListenToFourthTape -= ElevatorMethod5;

        groundFloorElevator.SetActive(false);
        salaDeRadioDoor.SetActive(true);
        DoorManager.instance.LockUnlockDoor(1, true);
        DoorManager.instance.OpenCloseDoor(1, false);

        AudioSource playerSource = FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>();
        playerSource.clip = Sons.instance.MusicaPreenchimento3;
        playerSource.PlayDelayed(Sons.instance.Fita4.length);
    }

    public void BuildNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
    #endregion
}

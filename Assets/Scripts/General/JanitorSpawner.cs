using System.Collections;
using UnityEngine;

public class JanitorSpawner : MonoBehaviour
{
    #region Attributes
    public GameObject janitor;
    public static JanitorSpawner instance;

    [Header("Pre Defined Spawn Locations")]
    public Transform corridorLocation;
    public Transform ductSceneLocation;
    public Transform corridorChaseLeft; 
    public Transform corridorChaseRight;
    public Transform lab9Location;
    public Transform doorClinic;
    public Transform Room2FirstLocation;
    public Transform Room2SecondLocation;
    public Transform Room2ThirdLocation;
    public Transform secondFloorCorridorLocation;
    public Transform frontOfConsultoryLocation;
    public Transform elevatorAttackLocation;
    public Transform groundFloorLocation;
    public Transform darkRoomLocation;
    #endregion

    #region Methods
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SequenceManager.FlashlightEvent += SpawnJanitorOnCorridor;        
        SequenceManager.CorridorChase += SpawnJanitorOnMysteriousCorridor;
        SequenceManager.JanitorOnLab9 += SpawnJanitorLab9;
        EventManager.ClinicAlarm += SpawnJanitorOnClinic;
        SequenceManager.FirstGeneratorActivated += SpawnJanitorOnRoom2_2;
        SequenceManager.Room9994Event += SpawnJanitorOnRoom2_3;
        SequenceManager.SecondFloorCorridor += SpawnJanitorOnSecondFloorCorridor;
        SequenceManager.JanitorInFrontOfConsultory1 += SpawnJanitorOnFrontOfConsultory;
        SequenceManager.JanitorInFrontOfConsultory2 += JanitorAggroOnPlayerAfterConsultory;
    }

    private void Spawn(Vector3 position, float rotation, string State) // SPAWNS THE JANITOR
    {
        Quaternion rot = transform.rotation * Quaternion.AngleAxis(rotation, Vector3.up);

        GameObject janitorObject = Instantiate(janitor, position, rot, transform);
        JanitorController janitorScript = janitorObject.GetComponent<JanitorController>();

        if(janitorScript != null)
        {
            janitorScript.SetJanitorState(State);
        }
    }

    private void SpawnCutscene(Vector3 position, float rotation, int animState) // SPAWNS THE JANITOR
    {
        Quaternion rot = transform.rotation * Quaternion.AngleAxis(rotation, Vector3.up);

        GameObject janitorObject = Instantiate(janitor, position, rot, transform);
        JanitorController janitorScript = janitorObject.GetComponent<JanitorController>();

        if (janitorScript != null)
        {
            janitorScript.SetJanitorState("Cutscene");            
            janitorScript.GetComponent<Animator>().SetInteger("State", animState);
            janitorScript.SetBreakCutscene(false);
        }
    }

    public void SpawnJanitorOnCorridor() // TRIGGERS WHEN THE PLAYER LEAVES THE SPAWN ROOM AND SPAWNS THE JANITOR ON THE CORRIDOR
    {
        if (this != null)
        {
            SpawnCutscene(corridorLocation.position, 90, 4);
            SequenceManager.FlashlightEvent -= SpawnJanitorOnCorridor;
        }
    }

    public void SpawnJanitorOnRoomBeforeDuct() // TRIGGERS WHEN THE PLAYER PLAYS THE FIRST TAPE AND THE ROOM CHANGES
    {
        if(this != null)
        {
            SpawnCutscene(ductSceneLocation.position, 0, 1);
        }
    }

    public void SpawnJanitorOnMysteriousCorridor(bool leftSide) // TRIGGERS WHEN THE PLAYER GOES TO THE END OF THE MYSTERIOUS CORRIDOR
    {
        if (this != null)
        {
            SequenceManager.CorridorChase -= SpawnJanitorOnMysteriousCorridor;
            SequenceManager.EndOfFirstRoom += SequenceManager.instance.FirstRoomMethod3;

            FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().clip = Sons.instance.MusicaDesespero2;
            FindObjectOfType<PlayerController>().gameObject.GetComponent<AudioSource>().PlayDelayed(3.3f);

            for (int i = 0; i < SequenceManager.instance.disappearOnFirstRoom3.Length; i++)
            {
                SequenceManager.instance.disappearOnFirstRoom3[i].gameObject.SetActive(false);
            }

            if (leftSide)
            {
                Spawn(corridorChaseLeft.position, 0, "Chasing");
            }
            else
            {
                Spawn(corridorChaseRight.position, 180, "Chasing");
            }
        }
    }

    public IEnumerator SpawnJanitorLab9() // SPAWNS THE JANITOR IN FRONT OF THE LAB 9 AND SENDS HIM TOWARDS THE END OF CORRIDOR
    {
        SequenceManager.JanitorOnLab9 -= SpawnJanitorLab9;

        // SPAWNS THE JANITOR
        SpawnCutscene(lab9Location.position, 0, 1);
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().clip = Sons.instance.MusicaPreenchimento1;
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().Play();
        yield return new WaitForFixedUpdate();

        // MOVES THE JANITOR
        if(FindObjectOfType<JanitorController>() != null)
        FindObjectOfType<JanitorController>().SetCurrentDestination(SequenceManager.instance.destinationAfterLab9.position);        

        // LIGHTS THE CORRIDOR AND OPENS THE RADIO ROOM
        LightManager.instance.SetLightState(0, false);
        LightManager.instance.SetLightState(4, true);
        LightManager.instance.SetLightState(2, true);

        // SAME ROOM'S DOOR
        DoorManager.instance.LockUnlockDoor(-2, false);
        DoorManager.instance.OpenCloseDoor(-2, false);

        // 3501 ROOM'S DOOR
        DoorManager.instance.LockUnlockDoor(4, false);
        DoorManager.instance.OpenCloseDoor(4, false);

        // RADIO ROOM'S DOOR
        DoorManager.instance.LockUnlockDoor(1, true);
        DoorManager.instance.OpenCloseDoor(1, true);        

        // DESTROYS THE JANITOR
        yield return new WaitForSeconds(8);        
        if (FindObjectOfType<JanitorController>() != null)
            DestroyObject(FindObjectOfType<JanitorController>().gameObject);


    }

    public void SpawnJanitorOnClinic() // SPAWNS THE JANITOR ON CLINIC DOOR
    {
        if (this != null)
        {
            EventManager.ClinicAlarm -= SpawnJanitorOnClinic;            
            Spawn(doorClinic.position, 0, "Chasing");
        }
    }

    public void SpawnJanitorOnRoom2_1() // SPAWNS THE JANITOR BEFORE THE T ROOM
    {
        if (this != null)
        {
            Spawn(Room2FirstLocation.position, 0, "Neutral");
            FindObjectOfType<PlayerController>().GetComponent<AudioSource>().clip = Sons.instance.MusicaTensão1;
            FindObjectOfType<PlayerController>().GetComponent<AudioSource>().Play();
        }
    }
   
    public void SpawnJanitorOnRoom2_2() // SPAWNS THE JANITOR WHEN THE FIRST GENERATOR IS ACTIVATED
    {
        SequenceManager.FirstGeneratorActivated -= SpawnJanitorOnRoom2_2;

        Spawn(Room2SecondLocation.position, 180, "Neutral");
        DoorManager.instance.OpenCloseDoor(7, false);
        DoorManager.instance.LockUnlockDoor(7, false);
        DoorManager.instance.LockUnlockDoor(-4, true);
    }

    public IEnumerator SpawnJanitorOnRoom2_3() // SPAWN THE JANITOR ON THE ROOM 9991 WHEN THE PLAYER ENTER THE ROOM 9994
    {
        SequenceManager.Room9994Event -= SpawnJanitorOnRoom2_3;

        // CLOSE AND LOCKS THE DOOR FROM THE PREVIOUS EVENT
        DoorManager.instance.LockUnlockDoor(-4, false);
        DoorManager.instance.OpenCloseDoor(-4, false);
        SequenceManager.instance.appearAtRoom9994.gameObject.SetActive(true);

        // DESTROYS THE JANITOR AND SPAWNS A NEW ONE
        DestroyObject(FindObjectOfType<JanitorController>().gameObject);
        yield return new WaitForFixedUpdate();
        SpawnCutscene(Room2ThirdLocation.position, 270, 4);

        // DOOR MANAGEMENT
        yield return new WaitForSeconds(0.5f);
        DoorManager.instance.OpenCloseDoor(10, true);
        yield return new WaitForSeconds(0.7f);
        DoorManager.instance.OpenCloseDoor(10, false);
    }
    
    public IEnumerator SpawnJanitorOnSecondFloorCorridor() // SPAWN THE JANITOR ON THE CORRIDOR OF THE SECOND FLOOR
    {
        SequenceManager.SecondFloorCorridor -= SpawnJanitorOnSecondFloorCorridor;

        // SPAWNS AND ORDERS THE JANITOR TO MOVE
        Spawn(secondFloorCorridorLocation.position, 180, "Chasing");
        yield return new WaitForFixedUpdate();
        FindObjectOfType<JanitorController>().SetCurrentDestination(SequenceManager.instance.secondFloorCorridorDestination.position);

        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().Stop();
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().clip = Sons.instance.MusicaTensão3;
        FindObjectOfType<PlayerController>().GetComponent<AudioSource>().Play();

        SequenceManager.instance.SecFloorRandTargets.gameObject.SetActive(true);
    }
    
    public void SpawnJanitorOnFrontOfConsultory() // SPAWN THE JANITOR IN FRONT OF THE CONSULTORY
    {
        SequenceManager.JanitorInFrontOfConsultory1 -= SpawnJanitorOnFrontOfConsultory;             
        DestroyObject(FindObjectOfType<JanitorController>().gameObject);
        SequenceManager.instance.janitorAggroTrigger.gameObject.SetActive(true);

        SpawnCutscene(frontOfConsultoryLocation.position, 0, 4);        
        DoorManager.instance.LockUnlockDoor(14, true);
        DoorManager.instance.LockUnlockDoor(15, true);
    }
    
    public void JanitorAggroOnPlayerAfterConsultory() // MAKES THE JANITOR IN FRONT OF THE CONSULTORY CHASE SOMETHING
    {
        SequenceManager.JanitorInFrontOfConsultory2 -= JanitorAggroOnPlayerAfterConsultory;
        FindObjectOfType<JanitorController>().SetJanitorState("Chasing");        
    }

    public void SpawnJanitorInFrontOfElevator() // SPAWN THE JANITOR IN FRONT OF THE ELEVATOR SO HE ATTACKS IT
    {
        SpawnCutscene(elevatorAttackLocation.position, 180, 5);        
    }
    
    public void SpawnJanitorOnGroundFloor() // SPAWN THE JANITOR IN THE GROUND FLOOR CORRIDOR
    {
        Spawn(groundFloorLocation.position, 90, "Cutscene");
    }
   
    public void SpawnJanitorOnDarkRoom() // SPAWN THE JANITOR INTO THE DARK ROOM
    {
        SpawnCutscene(darkRoomLocation.position, 270, 0);
        FindObjectOfType<JanitorController>().GetComponent<AudioSource>().Stop();
    }
    #endregion
}
